FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY . .
WORKDIR ShoppingCart/Hosts/ShoppingCart.Api
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /ShoppingCart/Hosts/ShoppingCart.Api/out ./
ENTRYPOINT ["dotnet", "ShoppingCart.Api.dll"]