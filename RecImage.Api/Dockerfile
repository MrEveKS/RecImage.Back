FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RecImage.Api/RecImage.Api.csproj", "RecImage.Api/"]
COPY ["RecImage.Business/RecImage.Business.csproj", "RecImage.Business/"]
COPY ["RecImage.Infrastructure.Logger/RecImage.Infrastructure.Logger.csproj", "RecImage.Infrastructure.Logger/"]
COPY ["RecImage.Infrastructure.Commons/RecImage.Infrastructure.Commons.csproj", "RecImage.Infrastructure.Commons/"]
COPY ["RecImage.ColoringService/RecImage.ColoringService.csproj", "RecImage.ColoringService/"]
RUN dotnet restore "RecImage.Api/RecImage.Api.csproj"
COPY . .
WORKDIR "/src/RecImage.Api"
RUN dotnet build "RecImage.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RecImage.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RecImage.Api.dll"]