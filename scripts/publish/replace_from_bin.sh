#!/usr/bin/env bash

sleep 1

mv -rf ./bin/* ./
rm -rf ./bin
../../CoomerBot || exit 1