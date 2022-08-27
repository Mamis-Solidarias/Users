FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine  AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS publish
WORKDIR /src
COPY ["MamisSolidarias.WebAPI.Users/MamisSolidarias.WebAPI.Users.csproj", "MamisSolidarias.WebAPI.Users/"]
RUN dotnet restore "MamisSolidarias.WebAPI.Users/MamisSolidarias.WebAPI.Users.csproj"
COPY . .
WORKDIR "/src/MamisSolidarias.WebAPI.Users"
RUN dotnet publish "MamisSolidarias.WebAPI.Users.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ls .
ENTRYPOINT ["./MamisSolidarias.WebAPI.Users"]
