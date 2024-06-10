# HitsInternsBackend
Interviews &amp; intership, other backend stuff

## Service naming  

Use 'svc_' prefix for naming your services. The name should be in CamelCase:
- svc_Auth
- svc_MyBalls
- etc.

## How to run

To start each service, navigate to the service folder and run the following command:
```sh
docker compose up -d
```
To launch only database run:
```sh
cd database && docker compose up -d
```

## Building single service
If you want to get a latest image of the service, you can build it by running, for example:
```sh
docker build https://github.com/ProcessesHITS1/HitsInternsBackend.git#develop -f svc_ThirdCourseBack/Dockerfile
```

## Dotnet services
To configure dotnet services you can specify environment variables in the `.env` file. Then again run
```sh
docker compose up -d
```
The default swagger url should be http://localhost:8081/swagger/index.html

## Java services
Swagger for company service and user service:
- http://localhost:8080/swagger-ui/index.html
- http://localhost:8090/swagger-ui/index.html
