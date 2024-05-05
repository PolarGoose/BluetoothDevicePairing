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

Function CreateZipArchive($file, $archiveFile) {
    Info "Archive `n  '$file' `n to `n  '$archiveFile'"
    Compress-Archive -Force -Path $file -DestinationPath $archiveFile
}

Function GetVersion() {
    $gitCommand = Get-Command -Name git

    $nearestTag = & $gitCommand describe --exact-match --tags HEAD
    if(-Not $?) {
        Info "The commit is not tagged. Use 'v0.0-dev' as a tag instead"
        $nearestTag = "v0.0-dev"
    }

    $commitHash = & $gitCommand rev-parse --short HEAD
    CheckReturnCodeOfPreviousCommand "Failed to get git commit hash"

    return "$($nearestTag.Substring(1))-$commitHash"
}

Function ForceCopy($file, $dstFolder) {
    Info "Copy `n  '$file' `n to `n  '$dstFolder'"
    New-Item $publishFolder -Force -ItemType "directory" > $null
    Copy-Item $buildResultExecutable -Destination $publishFolder -Force
}

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$root = Resolve-Path "$PSScriptRoot"
$buildRoot = "$root/build"
$buildResultsFolder = "$buildRoot/Release/net472"
$projectName = "BluetoothDevicePairing"
$buildResultExecutable = "$buildResultsFolder/$projectName.exe"
$publishFolder = "$buildRoot/Publish"
$version = GetVersion

Info "Run 'dotnet build'. version=$version"
dotnet build `
    --configuration Release `
    /property:DebugType=None `
    /property:Version=$version `
    $root/BluetoothDevicePairing.sln
CheckReturnCodeOfPreviousCommand "'dotnet build' command failed"

ForceCopy $buildResultExecutable $publishFolder
CreateZipArchive $publishFolder/BluetoothDevicePairing.exe $publishFolder/BluetoothDevicePairing.zip
