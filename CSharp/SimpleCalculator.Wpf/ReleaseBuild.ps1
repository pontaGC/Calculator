#==============================================================================================================================
# Release build wrapper
#==============================================================================================================================

param(
    [ValidateSet(2022, 2026)]
    [int]$VSVersion = 2026,

    [ValidateSet("x86", "x64")]
    [string]$Architecture = "x64"
)

$BuildScript = Join-Path $PSScriptRoot "Build.ps1"
& $BuildScript `
    -VSVersion $VSVersion `
    -Architecture $Architecture `
    -Configuration Release

exit $LASTEXITCODE