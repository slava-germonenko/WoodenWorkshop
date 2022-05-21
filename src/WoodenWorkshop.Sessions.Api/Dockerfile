FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR src/WoodenWorkshop.Sessions.Api

COPY . ./

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR src/WoodenWorkshop.Sessions.Api
COPY --from=build-env src/WoodenWorkshop.Sessions.Api/out .
ENTRYPOINT ["dotnet", "WoodenWorkshop.Sessions.Api.dll"]