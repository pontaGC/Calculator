#==============================================================================================================================
# Build SimpleCalculator
#==============================================================================================================================

#------------------------------------------------------------------------------------------------------------------------------
# Parameters
#------------------------------------------------------------------------------------------------------------------------------

param(
    [ValidateSet(2022, 2026)]
    [int]$VSVersion = 2026,

    [ValidateSet("x86", "x64")]
    [string]$Architecture = "x64",

    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release"
)

$VSEdition = "Community"
$SlnPath = Join-Path $PSScriptRoot "SimpleCalculator.Wpf.sln"

#------------------------------------------------------------------------------------------------------------------------------
# Execute
#------------------------------------------------------------------------------------------------------------------------------

function Get-VSInstallVersion {
    param (
        [int]$VSVer
    )

    if ($VSVer -eq 2026) {
        return "18"
    } else {
        return "$VSVer"
    }
}

function Find-MSBuild {
    param (
        [int]$VSVer
    )

    $VSInstallVersion = Get-VSInstallVersion -VSVer $VSVer

    # Define potential MSBuild locations
    $paths = @(
        "C:\Program Files\Microsoft Visual Studio\${VSInstallVersion}\${VSEdition}\MSBuild\Current\Bin\MSBuild.exe",
        "C:\Program Files (x86)\Microsoft Visual Studio\${VSInstallVersion}\${VSEdition}\MSBuild\Current\Bin\MSBuild.exe"
    )

    # Iterate through the array of paths
    foreach ($path in $paths) {
        if (Test-Path $path) {
            Write-Debug("Found MSBuild.exe: ${path}")
            return $path
        }

        Write-Output("Checked path: ${path} - Not found")
    }

    Write-Output ("Not found MSBuild.exe")
    exit 1
}

$MSBuildPath = Find-MSBuild -VSVer $VSVersion
$BuildArguments = @(
    $SlnPath
    "/t:Rebuild"
    "/p:Configuration=$Configuration"
    "/p:PlatformTarget=$Architecture"
)

# Execute the build process
$BuildProcess = Start-Process -FilePath $MSBuildPath -ArgumentList $BuildArguments -Wait -PassThru
$ExitCode = $BuildProcess.ExitCode

if ($ExitCode -eq 0) {
    Write-Host ("Build succeeded (Configuration: $Configuration, Architecture: $Architecture, ExitCode: ${ExitCode})") -ForegroundColor Green
} else {
    Write-Host ("Build failed (Configuration: $Configuration, Architecture: $Architecture, ExitCode: ${ExitCode})") -ForegroundColor Red
}

pause
exit $ExitCode
