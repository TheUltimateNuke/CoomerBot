cd /workspace/CoomerBot/bot

# Requires provided .env file with token in it
docker run --env-file ../.env -it --rm -w /opt/app -v $PWD:/opt/app mcr.microsoft.com/dotnet/sdk:6.0 dotnet run