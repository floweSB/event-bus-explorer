# syntax=docker/dockerfile:1.5
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

COPY ./src/Server/**/*.csproj ./proj/

# https://github.com/moby/moby/issues/15858#issuecomment-735490068
RUN for file in $(ls ./proj); do mkdir /src/${file%.*} && mv ./proj/${file} /src/${file%.*}/${file}; done

RUN dotnet restore "EventBusExplorer.Server.API/EventBusExplorer.Server.API.csproj"

COPY ./src/Server/ .
COPY --link ./.editorconfig .

WORKDIR /src/EventBusExplorer.Server.API

RUN dotnet build "EventBusExplorer.Server.API.csproj" \
  --configuration Release \
  --no-restore \
  --no-self-contained

RUN dotnet publish "EventBusExplorer.Server.API.csproj" \
  --configuration Release \
  --no-restore \
  --no-build \
  --no-self-contained \
  --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS final

WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT [ "dotnet", "EventBusExplorer.Server.API.dll" ]
