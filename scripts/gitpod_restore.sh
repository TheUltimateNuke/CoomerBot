cd ~/workspace/CoomerBot/bot
docker run -it --rm -w /opt/app -v $PWD:/opt/app mcr.microsoft.com/dotnet/sdk:6.0 dotnet restore