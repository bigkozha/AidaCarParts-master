# AidaCarParts
Prerequisites
.NET Core >= 3.1
NodeJS >= 8.9
Vue CLI >= 4.0
Your favourite editor (I prefer VS Code), or VS 2017/19

Docker output
Run the following command in a cmd window to build the docker image: docker build -t <IMAGE_NAME> .
ATTENTION! Do not miss the final dot to build the current directory

dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p password 
dotnet dev-certs https --trust
Run the application in a cmd window by this command: 
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_Kestrel__Certificates__Default__Password="password" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx -v %USERPROFILE%\.aspnet\https:/https/ aidacarparts
