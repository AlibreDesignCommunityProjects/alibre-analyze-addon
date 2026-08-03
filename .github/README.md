# alibre-analyze-addon

- A read-only Alibre Design add-on that reports every piece of data the AlibreX API exposes about the open part.
  - Adds one ribbon command, Analyze Part, under a dedicated Analyze tab.
  - Opens a tabbed dialog that reads the active part and never modifies the model.
  - Identity tab shows name, identifier, file path, session type, error state, part number, description, material, density, and the model, mass, length, and angle units.
  - Physical Properties tab shows mass, volume, surface area, center of gravity, bounding box min, max, and size, moments of inertia, and principal moments.
  - Topology tab shows per-body face, edge, vertex, lump, shell, and loop counts along with the topology type.
  - Feature Tree tab lists every feature with its type and any suppressed, error, or sheet-metal flags.
  - Parameters tab lists every parameter with its value, units, and equation.
  - A Copy All button places the full report on the clipboard as plain text.
  - Open a part in Alibre Design, then click Analyze Part on the Analyze ribbon tab to open the report.
  - The add-on implements IAlibreAddOn and loads on its own through its Analyze.adc manifest and logo.ico.
  - The VB.NET source lives in `source/Plugins/ADCP.Tools.Analyze/`, with AnalyzeBuilder.vb collecting the part data and AnalyzeDialog.vb building the tabbed report.
  - Icons live in `source/Plugins/ADCP.Tools.Analyze/icons/`.
  - Reference material and changelogs belong in `documentation/`.
