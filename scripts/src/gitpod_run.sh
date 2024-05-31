#!/usr/bin/env bash

cd /workspace/CoomerBot/bot || exit 1

# Requires provided .env file with token in it
docker run --env-file ../.env -it --rm -w /opt/app -v "$PWD":/opt/app mcr.microsoft.com/dotnet/sdk:6.0 dotnet run