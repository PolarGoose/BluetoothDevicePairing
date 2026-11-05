Function Info($msg) {
  Write-Host -ForegroundColor DarkGreen "`nINFO: $msg`n"
}

Function Error($msg) {
  Write-Host -ForegroundColor Red "`n`nERROR: $msg"
  exit 1
}

Function CheckReturnCodeOfPreviousCommand($msg) {
  if(-Not $?) {
    Error "${msg}. Error code: $LastExitCode"
  }
}

Function GetVersion() {
  try { $commitTag = git describe --exact-match --tags HEAD 2> $null } catch { }
  if(-Not $?) {
    Info "The commit is not tagged. Use 'v0.0-dev' as a tag instead"
    $commitTag = "v0.0-dev"
  }
  return "$($commitTag.Substring(1))"
}

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$root = Resolve-Path "$PSScriptRoot"
$buildDir = "$root/build"
$version = GetVersion

Info "Run 'dotnet build'. version=$version"
dotnet build --nologo `
             --configuration Release `
             -verbosity:minimal `
             /property:DebugType=None `
             /property:Version=$version `
             $root/BluetoothDevicePairing.slnx
CheckReturnCodeOfPreviousCommand "'dotnet build' command failed"

Info "Create zip archive from the executable"
Compress-Archive -Force -Path "$buildDir/AnyCPU/Release/BluetoothDevicePairing/net472/BluetoothDevicePairing.exe" -DestinationPath "$buildDir/BluetoothDevicePairing.zip"
