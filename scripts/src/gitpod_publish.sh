#!/usr/bin/env bash

if [ -d /home/gitpod/dotnet/sdk/6.0.423 ] ; then 
    cd /workspace/CoomerBot/ || exit 1
    dotnet publish --output ./Staging -c Release
    cp -rf ./scripts/publish/ ./Staging/scripts/
else
    bash ./scripts/src/gitpod_setup.sh
fi

