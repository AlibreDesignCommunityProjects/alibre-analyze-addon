# Alibre libraries

AlibreX.dll and AlibreAddOn.dll are proprietary Alibre Design libraries and are never committed to this repository.

The build auto-discovers them from the installed Alibre Design directory (C:\Program Files\Alibre Design*\Program). To override, set the AlibreProgramDir MSBuild property or drop the two DLLs into this folder (git-ignored).
