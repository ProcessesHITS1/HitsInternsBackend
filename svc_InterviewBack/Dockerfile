FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app


COPY ./NETLib ./NETLib

# Copy csproj and restore
COPY ./svc_InterviewBack/src/*.csproj ./svc_InterviewBack/src/
RUN dotnet restore "./svc_InterviewBack/src"

# Copy everything else and build
COPY ./svc_InterviewBack/src ./svc_InterviewBack/src
RUN dotnet publish "./svc_InterviewBack/src" -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "svc_InterviewBack.dll"]