dotnet publish -r linux-arm .\RusbeBot\RusbeBot.csproj
Remove-Item .\RusbeBot\bin\Debug\net6.0\linux-arm\publish\_config.yml
ssh pi@raspberrypi "pgrep RusbeBot | xargs kill"
scp .\RusbeBot\bin\Debug\net6.0\linux-arm\publish\* pi@raspberrypi:/home/pi/RusbeBot
ssh pi@raspberrypi ./RusbeBot/RusbeBot