FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /app
COPY * ./
RUN ls -a
ENTRYPOINT ["dotnet", "Notifications.WebApi.dll"]