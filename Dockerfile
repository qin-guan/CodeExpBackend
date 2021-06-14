FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CodeExpBackend.csproj", "./"]
RUN dotnet restore "CodeExpBackend.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "CodeExpBackend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeExpBackend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeExpBackend.dll"]
