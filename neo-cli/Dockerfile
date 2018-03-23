FROM microsoft/aspnetcore-build:2.0 AS build-env
WORKDIR /app

RUN apt-get update
RUN apt-get install -y libleveldb-dev
RUN apt-get install -y sqlite3
RUN apt-get install -y libsqlite3-dev
RUN apt-get install -y libunwind8-dev

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