dotnet publish -r linux-arm .\TheLostBot\TheLostBot.csproj
Remove-Item .\TheLostBot\bin\Debug\net6.0\linux-arm\publish\_config.yml
ssh pi@raspberrypi "pgrep TheLost | xargs kill"
scp .\TheLostBot\bin\Debug\net6.0\linux-arm\publish\* pi@raspberrypi:/home/pi/TheLostBot
ssh pi@raspberrypi ./TheLostBot/TheLostBot