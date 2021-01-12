#!/bin/bash

set -e

dotnet restore
dotnet build -c Release
dotnet publish -c Release
7z a ./bin/out.zip ./bin/Release/netcoreapp3.1/publish/*
az webapp deployment source config-zip --resource-group "MultiTranslator-RG" --name "MultiTranslatorApp" --src "./bin/out.zip"