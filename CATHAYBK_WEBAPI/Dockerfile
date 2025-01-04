#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["CATHAYBK_WEBAPI/CATHAYBK_WEBAPI.csproj", "CATHAYBK_WEBAPI/"]
COPY ["BasicEIP_Core/BasicEIP_Core.csproj", "BasicEIP_Core/"]
COPY ["CATHAYBK_Model/CATHAYBK_Model.csproj", "CATHAYBK_Model/"]
COPY ["CATHAYBK_Service/CATHAYBK_Service.csproj", "CATHAYBK_Service/"]
RUN dotnet restore "CATHAYBK_WEBAPI/CATHAYBK_WEBAPI.csproj"
COPY . .
WORKDIR "/src/CATHAYBK_WEBAPI"
RUN dotnet build "CATHAYBK_WEBAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CATHAYBK_WEBAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CATHAYBK_WEBAPI.dll"]
