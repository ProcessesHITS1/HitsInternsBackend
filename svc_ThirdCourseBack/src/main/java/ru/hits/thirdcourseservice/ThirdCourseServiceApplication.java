package ru.hits.thirdcourseservice;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.context.properties.ConfigurationPropertiesScan;

@ConfigurationPropertiesScan("ru.hits.thirdcourseservice")
@SpringBootApplication
public class ThirdCourseServiceApplication {

	public static void main(String[] args) {
		SpringApplication.run(ThirdCourseServiceApplication.class, args);
	}

}
