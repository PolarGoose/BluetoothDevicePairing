Function Info($msg) {
    Write-Host -ForegroundColor DarkGreen "`nINFO: $msg`n"
}

Function Error($msg) {
    Write-Host `n`n
    Write-Error $msg
    exit 1
}

Function CheckReturnCodeOfPreviousCommand($msg) {
    if(-Not $?) {
        Error "${msg}. Error code: $LastExitCode"
    }
}

Function CreateZipArchive($dir) {
    Info "Create zip archive ${dir}.zip"
    Compress-Archive -Force -Path "$dir/*" -DestinationPath "${dir}.zip"
}

Function GetVersion() {
    $gitCommand = Get-Command -ErrorAction Stop -Name git

    $nearestTag = & "$gitCommand" describe --exact-match --tags HEAD
    if(-Not $?) {
        Info "The commit is not tagged. Use 'v0.0.0-dev' instead"
        $nearestTag = "v0.0.0-dev"
    }

    $commitHash = & "$gitCommand" rev-parse --short HEAD
    CheckReturnCodeOfPreviousCommand "Failed to get git commit hash"

    return "$($nearestTag.Substring(1))-$commitHash"
}

Function Publish($isSelfContained, $slnFile, $version, $outDir) {
    Info "Run 'dotnet publish' command: isSelfContained=$isSelfContained outDir=$outDir slnFile=$slnFile"
    $Env:DOTNET_CLI_TELEMETRY_OPTOUT="true"
    dotnet publish `
        --self-contained "$isSelfContained" `
        --runtime win-x64 `
        --configuration Release `
        --output "$outDir" `
        /property:PublishSingleFile=true `
        /property:DebugType=None `
        /property:Version=$version `
        "$slnFile"
    CheckReturnCodeOfPreviousCommand "'dotnet publish' command failed"
    CreateZipArchive $outDir
}

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$root = Resolve-Path "$PSScriptRoot/../.."
$projectName = "BluetoothDevicePairing"
$publishDir = "$root/Build/Publish"
$slnFile = "$root/$projectName.sln"

$version = GetVersion
Publish true $slnFile $version "$publishDir/${projectName}_selfContained"
Publish false $slnFile $version "$publishDir/$projectName"

if ($LastExitCode -ne 0) {
    Error "LastExitCode is $LastExitCode at the end of the script. Should be 0"
}
