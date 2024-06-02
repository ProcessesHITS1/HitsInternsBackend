FROM eclipse-temurin:11-jdk-alpine as build
WORKDIR /opt/app

COPY ./svc_CompanyManagement/.mvn/ .mvn
COPY ./svc_CompanyManagement/mvnw ./
COPY ./svc_CompanyManagement/pom.xml ./

RUN ls -la
RUN ./mvnw dependency:go-offline

COPY ./svc_CompanyManagement/src ./src

RUN ./mvnw clean install

FROM eclipse-temurin:11-jre-alpine
WORKDIR /opt/app
COPY --from=build /opt/app/target/*.jar /opt/app/*.jar
EXPOSE 8080
ENTRYPOINT ["java", "-jar", "/opt/app/*.jar"]
