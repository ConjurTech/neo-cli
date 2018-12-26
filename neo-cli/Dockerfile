FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

RUN apt-get update && apt-get install -y --no-install-recommends apt-utils
RUN apt-get install -y \ 
libleveldb-dev \
sqlite3 \
libsqlite3-dev \
libunwind8-dev

# Remove neo blockchain package from neo-cli project, referencing it to the local NeoBlockchain Library
RUN dotnet remove neo-cli.csproj package neo
RUN dotnet sln neo-cli.sln add /opt/neoLib/neo/neo.csproj
RUN dotnet add neo-cli.csproj reference /opt/neoLib/neo/neo.csproj

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "neo-cli.dll"]