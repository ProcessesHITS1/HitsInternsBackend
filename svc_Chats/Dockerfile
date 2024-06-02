FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY . .
RUN dotnet restore "./svc_Chats/Interns.Chats.App/Interns.Chats.App.csproj" --disable-parallel \
&& dotnet publish "./svc_Chats/Interns.Chats.App/Interns.Chats.App.csproj" -c debug -o /app --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Interns.Chats.App.dll"]