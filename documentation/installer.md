# Installer

- `source/alibre-analyze-addon.iss` builds the Windows installer with Inno Setup.
  - Build the add-on first: `dotnet build source/Plugins/ADCP.Tools.Analyze/ADCP.Tools.Analyze.vbproj -c Debug`.
  - Compile the installer: run `ISCC.exe source/alibre-analyze-addon.iss` from Inno Setup 7.
  - The setup writes files to `Program Files\Alibre Design\Addons\Alibre Analyze Addon`.
  - The setup registers the add-on under `HKLM\SOFTWARE\Alibre Design Add-Ons` with the value name `Alibre Analyze Addon` pointing at the install folder.
  - Uninstall removes the files and the registry value.
  - The compiled setup lands in `source/installer`, which git ignores.
  - Pushing a `v*` tag runs `.github/workflows/build-installer.yml`, which builds the setup and attaches it to a GitHub release.
