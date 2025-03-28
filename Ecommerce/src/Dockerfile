FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Bootstrapper/Ecommerce.Bootstrapper/Ecommerce.Bootstrapper.csproj", "Bootstrapper/Ecommerce.Bootstrapper/"]
COPY ["Modules/Carts/Ecommerce.Modules.Carts.Api/Ecommerce.Modules.Carts.Api.csproj", "Modules/Carts/Ecommerce.Modules.Carts.Api/"]
COPY ["Modules/Carts/Ecommerce.Modules.Carts.Core/Ecommerce.Modules.Carts.Core.csproj", "Modules/Carts/Ecommerce.Modules.Carts.Core/"]
COPY ["Shared/Ecommerce.Shared.Abstractions/Ecommerce.Shared.Abstractions.csproj", "Shared/Ecommerce.Shared.Abstractions/"]
COPY ["Shared/Ecommerce.Shared.Infrastructure/Ecommerce.Shared.Infrastructure.csproj", "Shared/Ecommerce.Shared.Infrastructure/"]
COPY ["Modules/Discounts/Ecommerce.Modules.Discounts.Api/Ecommerce.Modules.Discounts.Api.csproj", "Modules/Discounts/Ecommerce.Modules.Discounts.Api/"]
COPY ["Modules/Discounts/Ecommerce.Modules.Discounts.Core/Ecommerce.Modules.Discounts.Core.csproj", "Modules/Discounts/Ecommerce.Modules.Discounts.Core/"]
COPY ["Modules/Inventory/Ecommerce.Modules.Inventory.Api/Ecommerce.Modules.Inventory.Api.csproj", "Modules/Inventory/Ecommerce.Modules.Inventory.Api/"]
COPY ["Modules/Inventory/Ecommerce.Modules.Inventory.Application/Ecommerce.Modules.Inventory.Application.csproj", "Modules/Inventory/Ecommerce.Modules.Inventory.Application/"]
COPY ["Modules/Inventory/Ecommerce.Modules.Inventory.Domain/Ecommerce.Modules.Inventory.Domain.csproj", "Modules/Inventory/Ecommerce.Modules.Inventory.Domain/"]
COPY ["Modules/Inventory/Ecommerce.Modules.Inventory.Infrastructure/Ecommerce.Modules.Inventory.Infrastructure.csproj", "Modules/Inventory/Ecommerce.Modules.Inventory.Infrastructure/"]
COPY ["Modules/Mails/Ecommerce.Modules.Mails.Api/Ecommerce.Modules.Mails.Api.csproj", "Modules/Mails/Ecommerce.Modules.Mails.Api/"]
COPY ["Modules/Orders/Ecommerce.Modules.Orders.Api/Ecommerce.Modules.Orders.Api.csproj", "Modules/Orders/Ecommerce.Modules.Orders.Api/"]
COPY ["Modules/Orders/Ecommerce.Modules.Orders.Application/Ecommerce.Modules.Orders.Application.csproj", "Modules/Orders/Ecommerce.Modules.Orders.Application/"]
COPY ["Modules/Orders/Ecommerce.Modules.Orders.Domain/Ecommerce.Modules.Orders.Domain.csproj", "Modules/Orders/Ecommerce.Modules.Orders.Domain/"]
COPY ["Modules/Orders/Ecommerce.Modules.Orders.Infrastructure/Ecommerce.Modules.Orders.Infrastructure.csproj", "Modules/Orders/Ecommerce.Modules.Orders.Infrastructure/"]
COPY ["Modules/Users/Ecommerce.Modules.Users.Api/Ecommerce.Modules.Users.Api.csproj", "Modules/Users/Ecommerce.Modules.Users.Api/"]
COPY ["Modules/Users/Ecommerce.Modules.Users.Core/Ecommerce.Modules.Users.Core.csproj", "Modules/Users/Ecommerce.Modules.Users.Core/"]
RUN dotnet restore "Bootstrapper/Ecommerce.Bootstrapper/Ecommerce.Bootstrapper.csproj"
COPY . .
WORKDIR "/src/Bootstrapper/Ecommerce.Bootstrapper"
RUN dotnet build "Ecommerce.Bootstrapper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecommerce.Bootstrapper.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update && apt-get install chromium -y
ENTRYPOINT ["dotnet", "Ecommerce.Bootstrapper.dll"]