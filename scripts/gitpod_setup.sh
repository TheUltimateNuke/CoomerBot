cd /workspace/CoomerBot/
curl https://download.visualstudio.microsoft.com/download/pr/111a63f5-e1d4-4d07-b8b2-98642b5fcc59/389661b982fa5b83b09a1f50b9da247a/dotnet-sdk-6.0.423-linux-x64.tar.gz --output sdk.tar.gz
tar --skip-old-files -zxf sdk.tar.gz -C /home/gitpod/dotnet
rm sdk.tar.gz
dotnet restore