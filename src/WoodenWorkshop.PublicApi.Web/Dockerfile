FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR src/WoodenWorkshop.Users.Api

COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR src/WoodenWorkshop.Users.Api
COPY --from=build-env src/WoodenWorkshop.Users.Api/out .
ENTRYPOINT ["dotnet", "WoodenWorkshop.Users.Api.dll"]