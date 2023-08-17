#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ItsCheck.API/ItsCheck.API.csproj", "ItsCheck.API/"]
COPY ["Common.DTO/Common.DTO.csproj", "Common.DTO/"]
COPY ["Common.Functions/Common.Functions.csproj", "Common.Functions/"]
COPY ["ItsCheck.DTO/ItsCheck.DTO.csproj", "ItsCheck.DTO/"]
COPY ["ItsCheck.Domain/ItsCheck.Domain.csproj", "ItsCheck.Domain/"]
COPY ["ItsCheck.Persistence/ItsCheck.Persistence.csproj", "ItsCheck.Persistence/"]
COPY ["ItsCheck.Service/ItsCheck.Service.csproj", "ItsCheck.Service/"]
COPY ["ItsCheck.DataAccess/ItsCheck.DataAccess.csproj", "ItsCheck.DataAccess/"]
COPY ["Common.DataAccess/Common.DataAccess.csproj", "Common.DataAccess/"]
COPY ["Common.Infrastructure/Common.Infrastructure.csproj", "Common.Infrastructure/"]
RUN dotnet restore "ItsCheck.API/ItsCheck.API.csproj"
COPY . .
WORKDIR "/src/ItsCheck.API"
RUN dotnet build "ItsCheck.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ItsCheck.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ItsCheck.API.dll"]