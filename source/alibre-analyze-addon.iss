#define MyAppName "Alibre Analyze Addon"
#define MyAppVersion "1.0"
#define MyAppPublisher "Alibre Design Community Projects"
#define MyAppURL "https://github.com/AlibreDesignCommunityProjects/alibre-analyze-addon"
#define MyAppDescription "Standalone Alibre Design add-on that reports all available AlibreX data for a part"
#define BuildDir "Plugins\ADCP.Tools.Analyze\bin\Debug\net481"

[Setup]
AppId={{2BD3CDBB-772B-41F0-89AF-08511DB91822}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\Alibre Design\Addons\{#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=..\LICENSE
OutputDir=installer
OutputBaseFilename=alibre-analyze-addon-setup-v{#MyAppVersion}
SetupIconFile={#BuildDir}\logo.ico
UninstallDisplayIcon={app}\logo.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=admin
MinVersion=6.1sp1

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "{#BuildDir}\ADCP.Tools.Analyze.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BuildDir}\ADCP.Tools.Analyze.pdb"; DestDir: "{app}"; Flags: ignoreversion skipifsourcedoesntexist
Source: "{#BuildDir}\Analyze.adc"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BuildDir}\logo.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#BuildDir}\logo.png"; DestDir: "{app}"; Flags: ignoreversion skipifsourcedoesntexist
Source: "{#BuildDir}\icons\*"; DestDir: "{app}\icons"; Flags: ignoreversion recursesubdirs createallsubdirs skipifsourcedoesntexist
Source: "..\.github\README.md"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\LICENSE"; DestDir: "{app}"; Flags: ignoreversion

[Registry]
Root: HKLM; Subkey: "SOFTWARE\Alibre Design Add-Ons"; ValueType: string; ValueName: "{#MyAppName}"; ValueData: "{app}"; Flags: uninsdeletevalue

[Icons]
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
