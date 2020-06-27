Function Info($msg) {
    Write-Host -ForegroundColor DarkGreen "`nINFO: $msg`n"
}

Function CheckReturnCodeOfPreviousCommand($errorMsg) {
    if(-Not $?) {
        Write-Host `n`n
        Write-Error $errorMsg
        exit 1
    }
}

Function CreateZipArchive($dir) {
    Info "Create zip archive ${dir}.zip"
    Compress-Archive -Force -Path "$dir/*" -DestinationPath "${dir}.zip"
}

Function Publish($isSelfContained, $slnFile, $outDir) {
    Info "Run 'dotnet publish' command: isSelfContained=$isSelfContained outDir=$outDir slnFile=$slnFile"
    dotnet publish `
        --self-contained "$isSelfContained" `
        --runtime win-x64 `
        --configuration Release `
        --output "$outDir" `
        /p:PublishSingleFile=true `
        /p:DebugType=None `
        "$slnFile"
    CheckReturnCodeOfPreviousCommand "'dotnet publish' command failed"
    CreateZipArchive $outDir
}

$root = Resolve-Path "$PSScriptRoot/../.."
$projectName = "BluetoothDevicePairing"
$publishDir = "$root/Build/Publish"
$slnFile = "$root/$projectName.sln"

Publish true $slnFile "$publishDir/${projectName}_selfContained"
Publish false $slnFile "$publishDir/$projectName"
