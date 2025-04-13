# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Cài đặt tzdata để quản lí múi giờ
RUN apk add --no-cache tzdata

# Thiết lập múi giờ
ENV TZ=Asia/Ho_Chi_Minh
RUN cp /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
COPY . /src

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["InteractiveFloor/InteractiveFloor.csproj", "InteractiveFloor/"]
COPY ["Service/Service.csproj", "Service/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["DAO/DAO.csproj", "DAO/"]
COPY ["BusinessObjects/BusinessObjects.csproj", "BusinessObjects/"]
RUN dotnet restore "./InteractiveFloor/InteractiveFloor.csproj"
COPY . .
WORKDIR "/src/InteractiveFloor"
RUN dotnet build "./InteractiveFloor.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./InteractiveFloor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final

# Cài đặt tzdata và icu-libs
RUN apk add --no-cache tzdata icu-libs

# Thiết lập múi giờ
ENV TZ=Asia/Ho_Chi_Minh
RUN cp /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
COPY . /src

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InteractiveFloor.dll"]
