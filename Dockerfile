##See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AidaCarParts.csproj", ""]
RUN dotnet restore "./AidaCarParts.csproj"
COPY . .
RUN apt-get update -yq 
RUN apt-get install curl gnupg -yq 
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install -y nodejs
RUN dotnet build "/src/AidaCarParts.csproj" -c Release -o /app/build
WORKDIR "/src/."
RUN dotnet build "AidaCarParts.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AidaCarParts.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AidaCarParts.dll"]


#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
#WORKDIR /src
#COPY ["./AidaCarParts.csproj", "app/"]
#RUN dotnet restore "app/AidaCarParts.csproj"
#COPY . .
#RUN apt-get update -yq 
#RUN apt-get install curl gnupg -yq 
#RUN curl -sL https://deb.nodesource.com/setup_14.x | bash -
#RUN apt-get install -y nodejs
#RUN dotnet build "/src/AidaCarParts.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "/src/AidaCarParts.csproj" -c Release -o /app/publish
#
#FROM base AS runtime
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "AidaCarParts.dll"]