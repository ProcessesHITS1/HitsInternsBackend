FROM eclipse-temurin:11-jdk-alpine as build
WORKDIR /opt/app

COPY ./svc_Auth/.mvn/ .mvn
COPY ./svc_Auth/mvnw ./
COPY ./svc_Auth/pom.xml ./

RUN chmod +x ./mvnw
RUN ./mvnw dependency:go-offline

COPY ./svc_Auth/src ./src

RUN ./mvnw clean install

FROM eclipse-temurin:11-jre-alpine
WORKDIR /opt/app
COPY --from=build /opt/app/target/*.jar /opt/app/*.jar
EXPOSE 8080
ENTRYPOINT ["java", "-jar", "/opt/app/*.jar"]
