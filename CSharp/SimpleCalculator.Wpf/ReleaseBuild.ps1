# ==============================================================
# Release re-build SimpleCalculator
# ==============================================================

$VSVer = 2022
$VSEdition = "Community"
$SlnPath = ".\SimpleCalculator.Wpf.sln"

$MSBuild17_FilePathx86 = "C:\Program Files (x86)\Microsoft Visual Studio\${VSVer}\${VSEdition}\MSBuild\Current\Bin\MSBuild.exe"
$MSBuild17_FilePath = "C:\Program Files\Microsoft Visual Studio\${VSVer}\${VSEdition}\MSBuild\Current\Bin\MSBuild.exe"

# Find MSBuild.exe

function Find-MSBuild {
    # Define potential MSBuild locations
    $paths = @(
        $MSBuild17_FilePath,
        $MSBuild17_FilePathx86
    )

    # Iterate through the array of paths
    foreach ($path in $paths) {
        if (Test-Path $path) {
            Write-Debug("Found MSBuild.exe: ${path}")
            return $path
        }
    }

    Write-Output ("Not found MSBuild.exe")
    exit
}

# Execute build
$MSBuildPath = Find-MSBuild
$BuildProcess = Start-Process -FilePath "${MSBuildPath}" -ArgumentList "${SlnPath} /t:Rebuild /p:Configuration=Release" -Wait -PassThru
$ExitCode = $BuildProcess.ExitCode
Write-Host ("Build has been completed (ExitCode: ${ExitCode})")

pause