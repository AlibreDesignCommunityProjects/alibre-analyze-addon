Imports AlibreX
Imports System.Globalization
Imports System.Text

Namespace ADCP.Tools.Analyze

    ''' <summary>
    ''' Collects read-only data about the active part from AlibreX and formats it into
    ''' the grouped report sections shown by <see cref="AnalyzeDialog"/>. Performs no edits.
    ''' </summary>
    Public Class AnalyzeBuilder

        Private ReadOnly _session As IADPartSession
        Private Shared ReadOnly Ci As CultureInfo = CultureInfo.InvariantCulture

        Public Sub New(session As IADPartSession)
            _session = session
        End Sub

        Public Function BuildReport() As IReadOnlyList(Of ReportSection)
            If _session Is Nothing Then Throw New InvalidOperationException("No active part session.")
            Dim sections As New List(Of ReportSection) From {
                Identity(),
                Physical(),
                Topology(),
                Features(),
                Parameters()
            }
            Return sections
        End Function

        Private Function Identity() As ReportSection
            Dim s As New ReportSection("Identity")
            s.Add("Name", _session.Name)
            s.Add("Identifier", _session.Identifier)
            s.Add("File path", If(String.IsNullOrEmpty(_session.FilePath), "(unsaved)", _session.FilePath))
            s.Add("Session type", _session.SessionType.ToString())
            s.Add("Time stamp", _session.TimeStamp.ToString(Ci))
            s.Add("Has errors", _session.HasErrors.ToString())
            Try
                Dim props = _session.DesignProperties
                s.Add("Number", props.Number)
                s.Add("Description", props.Description)
                s.Add("Material", props.Material)
                s.Add("Density", Num(props.Density))
                s.Add("Model units", props.ModelUnits.ToString())
                s.Add("Mass units", props.MassUnits.ToString())
                s.Add("Length units", props.LengthDisplayUnits.ToString())
                s.Add("Angle units", props.AngleDisplayUnits.ToString())
            Catch ex As Exception
                s.Add("Design properties", "unavailable: " & ex.Message)
            End Try
            Return s
        End Function

        Private Function Physical() As ReportSection
            Dim s As New ReportSection("Physical Properties")
            Try
                Dim pp = _session.PhysicalProperties(ADAccuracySetting.AD_HIGH)
                s.Add("Mass", Num(pp.Mass))
                s.Add("Volume", Num(pp.Volume))
                s.Add("Surface area", Num(pp.SurfaceArea))
                s.Add("Material", pp.Material)
                Dim cx, cy, cz As Double
                pp.GetCenterOfGravity(cx, cy, cz)
                s.Add("Center of gravity", $"({Num(cx)}, {Num(cy)}, {Num(cz)})")
                Dim lower As IADPoint = Nothing, upper As IADPoint = Nothing
                pp.GetExtents(lower, upper)
                s.Add("Bounding box min", $"({Num(lower.X)}, {Num(lower.Y)}, {Num(lower.Z)})")
                s.Add("Bounding box max", $"({Num(upper.X)}, {Num(upper.Y)}, {Num(upper.Z)})")
                s.Add("Bounding box size", $"({Num(upper.X - lower.X)}, {Num(upper.Y - lower.Y)}, {Num(upper.Z - lower.Z)})")
                Dim xx, yy, zz, yz, zx, xy As Double
                pp.GetMomentsOfInertia(xx, yy, zz, yz, zx, xy)
                s.Add("Moments of inertia", $"XX={Num(xx)} YY={Num(yy)} ZZ={Num(zz)} YZ={Num(yz)} ZX={Num(zx)} XY={Num(xy)}")
                Dim m1, m2, m3 As Double
                pp.GetPrincipalMomentsOfInertia(m1, m2, m3)
                s.Add("Principal moments", $"{Num(m1)}, {Num(m2)}, {Num(m3)}")
            Catch ex As Exception
                s.Add("Physical properties", "unavailable: " & ex.Message)
            End Try
            Return s
        End Function

        Private Function Topology() As ReportSection
            Dim s As New ReportSection("Topology")
            Try
                Dim bodies = _session.Bodies
                s.Add("Body count", bodies.Count.ToString(Ci))
                For i As Integer = 0 To bodies.Count - 1
                    Dim b = bodies.Item(i)
                    Dim ts = b.TopologySummary
                    s.Add($"Body {i + 1} topology type", b.TopologyType.ToString())
                    s.Add($"Body {i + 1} faces", ts.FacesCount.ToString(Ci))
                    s.Add($"Body {i + 1} edges", ts.EdgesCount.ToString(Ci))
                    s.Add($"Body {i + 1} vertices", ts.VerticesCount.ToString(Ci))
                    s.Add($"Body {i + 1} lumps", ts.LumpsCount.ToString(Ci))
                    s.Add($"Body {i + 1} shells", ts.ShellsCount.ToString(Ci))
                    s.Add($"Body {i + 1} loops", ts.LoopsCount.ToString(Ci))
                Next
            Catch ex As Exception
                s.Add("Topology", "unavailable: " & ex.Message)
            End Try
            Return s
        End Function

        Private Function Features() As ReportSection
            Dim s As New ReportSection("Feature Tree")
            Try
                Dim feats = _session.Features
                s.Add("Feature count", feats.Count.ToString(Ci))
                For i As Integer = 0 To feats.Count - 1
                    Dim f = feats.Item(i)
                    Dim flags As New List(Of String)
                    If f.IsSuppressed Then flags.Add("suppressed")
                    If f.HasError Then flags.Add("error")
                    If f.IsSheetMetalFeature Then flags.Add("sheet-metal")
                    Dim suffix = If(flags.Count > 0, " [" & String.Join(", ", flags) & "]", "")
                    s.Add($"{i + 1}. {f.Name}", f.FeatureType.ToString() & suffix)
                Next
            Catch ex As Exception
                s.Add("Features", "unavailable: " & ex.Message)
            End Try
            Return s
        End Function

        Private Function Parameters() As ReportSection
            Dim s As New ReportSection("Parameters")
            Try
                Dim ps = _session.Parameters
                s.Add("Parameter count", ps.Count.ToString(Ci))
                For Each obj As Object In ps
                    Dim p = TryCast(obj, IADParameter)
                    If p Is Nothing Then Continue For
                    Dim eq = If(String.IsNullOrEmpty(p.Equation), "", " = " & p.Equation)
                    s.Add(p.Name, $"{Num(p.Value)} {p.Units}{eq}")
                Next
            Catch ex As Exception
                s.Add("Parameters", "unavailable: " & ex.Message)
            End Try
            Return s
        End Function

        Private Shared Function Num(value As Double) As String
            Return value.ToString("G6", Ci)
        End Function

    End Class

    ''' <summary>
    ''' One named group of key/value rows in the analysis report.
    ''' </summary>
    Public Class ReportSection

        Public ReadOnly Property Title As String
        Public ReadOnly Property Rows As New List(Of KeyValuePair(Of String, String))

        Public Sub New(title As String)
            Me.Title = title
        End Sub

        Public Sub Add(key As String, value As String)
            Rows.Add(New KeyValuePair(Of String, String)(key, If(value, "")))
        End Sub

        Public Function ToPlainText() As String
            Dim sb As New StringBuilder()
            sb.AppendLine(Title)
            For Each r In Rows
                sb.AppendLine($"  {r.Key}: {r.Value}")
            Next
            Return sb.ToString()
        End Function

    End Class

End Namespace
