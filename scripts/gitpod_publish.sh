#!/usr/bin/env bash

if [ -d /home/gitpod/dotnet/sdk/6.0.423 ] ; then 
    cd /workspace/CoomerBot/bot || exit 0
    dotnet publish --output ../out -c Release
else
    bash ./gitpod_setup.sh
fi

