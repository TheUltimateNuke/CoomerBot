#!/usr/bin/env bash

if [ -d /home/gitpod/dotnet/sdk/6.0.423 ] ; then 
    cd /workspace/CoomerBot/ || exit 1
    dotnet publish --output ./Staging -c Release
    cp -r ./scripts/publish/ ./Staging/scripts/publish/
else
    bash ./scripts/src/gitpod_setup.sh
fi

