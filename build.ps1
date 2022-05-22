dotnet publish -r linux-arm .\src\RusbeBot\RusbeBot.csproj
Remove-Item .\src\RusbeBot\bin\Debug\net6.0\linux-arm\publish\_config.yml
ssh pi@raspberrypi "pgrep RusbeBot | xargs kill"
scp .\src\RusbeBot\bin\Debug\net6.0\linux-arm\publish\* pi@raspberrypi:/home/pi/RusbeBot
ssh pi@raspberrypi ./RusbeBot/RusbeBot