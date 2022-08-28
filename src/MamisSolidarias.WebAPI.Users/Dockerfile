FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build-env
WORKDIR /app
# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore src/MamisSolidarias.WebAPI.Users/MamisSolidarias.WebAPI.Users.csproj
# Build and publish a release
RUN dotnet publish src/MamisSolidarias.WebAPI.Users/MamisSolidarias.WebAPI.Users.csproj  \
    -c Release \
    -o out \
    --no-restore 

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet","MamisSolidarias.WebAPI.Users.dll"]

