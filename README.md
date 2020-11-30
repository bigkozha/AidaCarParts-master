# AidaCarParts

![example workflow name](https://github.com/bigkozha/AidaCarParts/workflows/.NET%20Core/badge.svg)


Prerequisites\
.NET Core >= 3.1\
NodeJS >= 8.9\
Vue CLI >= 4.0\
VS 2017/19

docker build -t aidacarparts:latest . 

!For local debug run!\
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p password \
dotnet dev-certs https --trust\
!For local debug run!\
Run the application in a cmd window by this command: \
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_Kestrel__Certificates__Default__Password="password" -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx -v %USERPROFILE%\.aspnet\https:/https/ aidacarparts:latest
