FROM microsoft/aspnetcore-build:latest AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY FUNAPI/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY FUNAPI/ ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/aspnetcore:latest
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "FUNAPI.dll"]
EXPOSE 5000