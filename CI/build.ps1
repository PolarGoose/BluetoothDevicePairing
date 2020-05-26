Function Info($msg) {
    Write-Host
    Write-Host -ForegroundColor DarkGreen "INFO: $msg"
    Write-Host
}

Function CheckReturnCodeOfPreviousCommand($errorMsg) {
    if(-Not $?) {
        Write-Host `n`n
        Write-Error $errorMsg
        exit 1
    }
}

Function ResolvePath($path) {
    return $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($path)
}

Function CreateZipArchive($fileToArchive, $archiveName) {
    Info "Create zip archive: fileToArchive='${fileToArchive}' archiveName='$archiveName'"
    Compress-Archive -Force -Path $fileToArchive -DestinationPath $archiveName
}

Function Publish($isSelfContained, $destination) {
    Info "Run 'dotnet publish' command: isSelfContained=$isSelfContained destination=$destination"
    dotnet publish `
        --self-contained "$isSelfContained"`
        -r win-x64 `
        -c Release `
        /p:PublishSingleFile=true `
        /p:DebugType=None `
        --output "$destination" `
        "$slnFile"
    CheckReturnCodeOfPreviousCommand "'dotnet publish' command failed"
    CreateZipArchive "$destination/${projectName}.exe" "${destination}.zip"
}

$projectName = "BluetoothDevicePairing"
$publishDir = ResolvePath "$PSScriptRoot/../Build/Publish"
$slnFile = ResolvePath "$PSScriptRoot/../$projectName.sln"

Publish true "$publishDir/${projectName}_selfContained"
Publish false "$publishDir/$projectName"
