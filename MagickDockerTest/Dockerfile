FROM microestc/aspnetcore:imagemagick.0.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MagickDockerTest/MagickDockerTest.csproj", "MagickDockerTest/"]
COPY ["MagickConverter/MagickConverter.csproj", "MagickConverter/"]
RUN dotnet restore "MagickDockerTest/MagickDockerTest.csproj"
COPY . .
WORKDIR "/src/MagickDockerTest"
RUN dotnet build "MagickDockerTest.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MagickDockerTest.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MagickDockerTest.dll"]
