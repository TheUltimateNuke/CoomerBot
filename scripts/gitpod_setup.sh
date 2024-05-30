#!/usr/bin/env bash

if [ -d /home/gitpod/dotnet/sdk/6.0.423 ] ; then dotnet restore ; exit 0 ; fi

cd /workspace/CoomerBot/ || exit 1
gp validate

# Install dotnet 6
curl https://download.visualstudio.microsoft.com/download/pr/111a63f5-e1d4-4d07-b8b2-98642b5fcc59/389661b982fa5b83b09a1f50b9da247a/dotnet-sdk-6.0.423-linux-x64.tar.gz --output sdk.tar.gz
tar --skip-old-files -zxf sdk.tar.gz -C /home/gitpod/dotnet
rm sdk.tar.gz

sudo apt update && sudo apt install shellcheck

dotnet restore