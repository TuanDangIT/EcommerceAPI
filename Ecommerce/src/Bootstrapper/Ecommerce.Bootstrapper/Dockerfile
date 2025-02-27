#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Bootstrapper/Ecommerce.Bootstrapper/Ecommerce.Bootstrapper.csproj", "src/Bootstrapper/Ecommerce.Bootstrapper/"]
COPY ["src/Modules/Carts/Ecommerce.Modules.Carts.Api/Ecommerce.Modules.Carts.Api.csproj", "src/Modules/Carts/Ecommerce.Modules.Carts.Api/"]
COPY ["src/Modules/Carts/Ecommerce.Modules.Carts.Core/Ecommerce.Modules.Carts.Core.csproj", "src/Modules/Carts/Ecommerce.Modules.Carts.Core/"]
COPY ["src/Shared/Ecommerce.Shared.Abstractions/Ecommerce.Shared.Abstractions.csproj", "src/Shared/Ecommerce.Shared.Abstractions/"]
COPY ["src/Shared/Ecommerce.Shared.Infrastructure/Ecommerce.Shared.Infrastructure.csproj", "src/Shared/Ecommerce.Shared.Infrastructure/"]
COPY ["src/Modules/Discounts/Ecommerce.Modules.Discounts.Api/Ecommerce.Modules.Discounts.Api.csproj", "src/Modules/Discounts/Ecommerce.Modules.Discounts.Api/"]
COPY ["src/Modules/Discounts/Ecommerce.Modules.Discounts.Core/Ecommerce.Modules.Discounts.Core.csproj", "src/Modules/Discounts/Ecommerce.Modules.Discounts.Core/"]
COPY ["src/Modules/Inventory/Ecommerce.Modules.Inventory.Api/Ecommerce.Modules.Inventory.Api.csproj", "src/Modules/Inventory/Ecommerce.Modules.Inventory.Api/"]
COPY ["src/Modules/Inventory/Ecommerce.Modules.Inventory.Application/Ecommerce.Modules.Inventory.Application.csproj", "src/Modules/Inventory/Ecommerce.Modules.Inventory.Application/"]
COPY ["src/Modules/Inventory/Ecommerce.Modules.Inventory.Domain/Ecommerce.Modules.Inventory.Domain.csproj", "src/Modules/Inventory/Ecommerce.Modules.Inventory.Domain/"]
COPY ["src/Modules/Inventory/Ecommerce.Modules.Inventory.Infrastructure/Ecommerce.Modules.Inventory.Infrastructure.csproj", "src/Modules/Inventory/Ecommerce.Modules.Inventory.Infrastructure/"]
COPY ["src/Modules/Mails/Ecommerce.Modules.Mails.Api/Ecommerce.Modules.Mails.Api.csproj", "src/Modules/Mails/Ecommerce.Modules.Mails.Api/"]
COPY ["src/Modules/Orders/Ecommerce.Modules.Orders.Api/Ecommerce.Modules.Orders.Api.csproj", "src/Modules/Orders/Ecommerce.Modules.Orders.Api/"]
COPY ["src/Modules/Orders/Ecommerce.Modules.Orders.Application/Ecommerce.Modules.Orders.Application.csproj", "src/Modules/Orders/Ecommerce.Modules.Orders.Application/"]
COPY ["src/Modules/Orders/Ecommerce.Modules.Orders.Domain/Ecommerce.Modules.Orders.Domain.csproj", "src/Modules/Orders/Ecommerce.Modules.Orders.Domain/"]
COPY ["src/Modules/Orders/Ecommerce.Modules.Orders.Infrastructure/Ecommerce.Modules.Orders.Infrastructure.csproj", "src/Modules/Orders/Ecommerce.Modules.Orders.Infrastructure/"]
COPY ["src/Modules/Users/Ecommerce.Modules.Users.Api/Ecommerce.Modules.Users.Api.csproj", "src/Modules/Users/Ecommerce.Modules.Users.Api/"]
COPY ["src/Modules/Users/Ecommerce.Modules.Users.Core/Ecommerce.Modules.Users.Core.csproj", "src/Modules/Users/Ecommerce.Modules.Users.Core/"]
RUN dotnet restore "./src/Bootstrapper/Ecommerce.Bootstrapper/./Ecommerce.Bootstrapper.csproj"
COPY . .
WORKDIR "/src/src/Bootstrapper/Ecommerce.Bootstrapper"
RUN dotnet build "./Ecommerce.Bootstrapper.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ecommerce.Bootstrapper.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.Bootstrapper.dll"]