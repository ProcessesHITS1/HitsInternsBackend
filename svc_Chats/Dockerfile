FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ./NETLib ./NETLib
COPY ./svc_Chats ./svc_Chats

RUN dotnet restore "./svc_Chats/Interns.Chats.App" && dotnet publish "./svc_Chats/Interns.Chats.App" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "Interns.Chats.App.dll"]