FROM mcr.microsoft.com/dotnet/aspnet:6.0
COPY ./bin/Release/net6.0 /app
WORKDIR /app
ENTRYPOINT ["dotnet", "WoodenWorkshop.PublicApi.Web.dll"]