FROM eclipse-temurin:11-jdk-alpine as build
WORKDIR /opt/app

COPY ./svc_ThirdCourseBack/.mvn/ .mvn
COPY ./svc_ThirdCourseBack/mvnw ./
COPY ./svc_ThirdCourseBack/pom.xml ./

RUN ls -la
RUN ./mvnw dependency:go-offline

COPY ./svc_ThirdCourseBack/src ./src

RUN ./mvnw clean install

FROM eclipse-temurin:11-jre-alpine
WORKDIR /opt/app
COPY --from=build /opt/app/target/*.jar /opt/app/*.jar
EXPOSE 8080
ENTRYPOINT ["java", "-jar", "/opt/app/*.jar"]
