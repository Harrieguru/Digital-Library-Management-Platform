#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Set ASP.NET Core environment variable for URL configuration
ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["eBook_manager/eBook_manager.csproj", "eBook_manager/"]
RUN dotnet restore "eBook_manager/eBook_manager.csproj"
COPY . .
WORKDIR "/src/eBook_manager"
RUN dotnet build "eBook_manager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "eBook_manager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eBook_manager.dll"]