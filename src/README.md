### Avp VideoPlayer

## Build steps
# Prerequisites:
 * Windows 10 or higher
 * .net SDK 6.0
   https://dotnet.microsoft.com/en-us/download/dotnet/6.0
 * Wix Installer toolset
   https://wixtoolset.org/

# Building
Open a powershell developer prompt (with dotnet, msbuild and wix in the PATH variable)
and to execute the build pipeline run:

.\scripts\build.ps1

In order to regenerate the Wix installer definition (after changing the files list for example) add the "-regenerateInstaller" switch:

.\scripts\build.ps1 -regenerateInstaller

# After build
Generated assets and the msi installer will be published to the folder .\build\

