param(
    [Parameter(HelpMessage="Rebuild the application")]
    [switch]$skipBuildSolution = $False,
    [Parameter(HelpMessage="Regenerate installer definition")]
    [switch]$regenerateInstaller = $False,
    [Parameter(HelpMessage="Do not build the installer")]
    [switch]$skipBuildInstaller = $False,
    [Parameter(HelpMessage="Build configuration")]
    [string]$configuration = "Release"
)

# parameters needed to run the build pipeline
$activeconfiguration = ((Get-Culture).TextInfo).ToTitleCase($configuration)
$currentScriptFolder =([System.IO.FileInfo] $MyInvocation.MyCommand.Path).DirectoryName
$rootpath = (Get-Item $currentScriptFolder).Parent.FullName
$mainSolutionFile = Join-Path $rootpath "src\AVPVideoPlayer.Wpf.sln"
$mainExeProjectFile = Join-Path $rootpath "src\Wpf\AvpVideoPlayer.Wpf\AvpVideoPlayer.Wpf.csproj"
$installerSolutionFile = Join-Path $rootpath "installer\AVPVideoPlayer.Installer.sln"
$mainSolutionPublishFolder = Join-Path $rootpath "build"
$mainSolutionPublishBinFolder = Join-Path $mainSolutionPublishFolder $activeconfiguration
$installerBuildArtifact = Join-Path $rootpath "installer\AvpVideoPlayer.Installer\bin\$activeconfiguration\AvpVideoPlayer.Setup.msi"
if ($activeconfiguration -ne "Release") {
    $publishedInstallerArtifact = Join-Path $mainSolutionPublishFolder "SetupAvpVideoPlayer-$activeconfiguration.msi"
} else {
    $publishedInstallerArtifact = Join-Path $mainSolutionPublishFolder "SetupAvpVideoPlayer.msi"
}
$installerGeneratorProjectPath = "installer\AvpVideoPlayer.Installer.GenerateWix\"
$installerGeneratorProjectFile = Join-Path $installerGeneratorProjectPath "AvpVideoPlayer.Installer.GenerateWix.csproj"
$installerGeneratorBinDir = Join-Path $installerGeneratorProjectPath "bin\$activeconfiguration\net6.0\publish"
$installerGeneratorExe = Join-Path $installerGeneratorBinDir "AvpVideoPlayer.Installer.GenerateWix.exe"
$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
$stopwatch2 = [System.Diagnostics.Stopwatch]::StartNew()

function Write-Prompt ($msg) {
    Write-Host -ForegroundColor Yellow $msg
}

function CleanBuildOutput {
    Write-Prompt "Cleaning..."
    dotnet clean $mainSolutionFile --configuration $activeconfiguration /v:q
    if (-not $skipBuildSolution) {
    if (Test-Path $mainSolutionPublishBinFolder) {
        Remove-Item  -Recurse -Force $mainSolutionPublishBinFolder
        }
        New-Item -Path $mainSolutionPublishBinFolder -ItemType directory 
    }
    if (-not $skipBuildInstaller) {
        if (Test-Path $publishedInstallerArtifact) {
            Remove-Item -Force $publishedInstallerArtifact
        }
    }
    Write-Elapsed "Cleaned"
}

function Write-Elapsed($prompt)  {
    $elapsed = $stopwatch.Elapsed.TotalSeconds;
    Write-Host "$prompt in $elapsed sec" 
    ([System.Diagnostics.Stopwatch]$stopwatch).Restart()
}

function BuildAndPublishMainSolution {
    Write-Prompt "* Building main solution in $activeconfiguration mode"
    dotnet build $mainSolutionFile --configuration $activeconfiguration /v:q *>&1
    Write-Elapsed "Built main solution"
    if($LASTEXITCODE -ne 0) { throw "Error building main solution" }

    Write-Prompt "* Running unit tests"
    dotnet test $mainSolutionFile --configuration $activeconfiguration /v:q *>&1
    Write-Elapsed "Ran unit tests"
    if($LASTEXITCODE -ne 0) { throw "Error running unit tests" }

    Write-Prompt "* Publishing build artifacts"
    dotnet publish $mainExeProjectFile --configuration $activeconfiguration --no-build /v:q --output $mainSolutionPublishBinFolder  *>&1
    Write-Elapsed "Published build artifacts"
    if ($LASTEXITCODE -ne 0) {
         throw "Error publishing main solution" 
    }
    if ($activeconfiguration -eq "Release") {
        Remove-Item "$mainSolutionPublishBinFolder\*.pdb"
    }
}

function RegenerateInstaller {
    Write-Prompt "* Regenerating msi installer definition"
    dotnet publish $installerGeneratorProjectFile --configuration $activeconfiguration /v:q *>&1   
    Write-Elapsed "published Wix installer generator to $installerGeneratorExe"
    if($LASTEXITCODE -ne 0) { throw "Error publishing installer generator" }

    & $installerGeneratorExe $mainSolutionPublishBinFolder
    Write-Elapsed "Generated Wix installer definition"
    if ($LastExitCode -ne 0) { throw "Error generating installer definition, check template and settings.json in $installerGeneratorBinDir" }
}

    
function BuildAndDeployInstaller {
    Write-Prompt "* Building msi installer"
    msbuild $installerSolutionFile /p:Configuration=$activeconfiguration /v:q *>&1
    Write-Elapsed "Built msi installer"
    if($LASTEXITCODE -ne 0) { throw "Error building msi installer" }
    
    Write-Prompt "* Copying msi installer to $publishedInstallerArtifact"
    Copy-Item -Path $installerBuildArtifact -Destination $publishedInstallerArtifact
    Write-Elapsed "Copied msi installer"
}

try {
    CleanBuildOutput

    if (-not $skipBuildSolution)  {
        BuildAndPublishMainSolution
    }
    if (-not $skipBuildInstaller) {
        if ($regenerateInstaller) {
            RegenerateInstaller
        }

        BuildAndDeployInstaller
    }
    $totalelapsed = $stopwatch2.Elapsed.TotalSeconds;
    Write-Prompt "Build completed in $totalelapsed sec. Artifacts can be found in $mainSolutionPublishFolder"
} catch {
    Write-Error "An exception occurred during the build pipeline. Check the logs, correct the issue and try again."
}