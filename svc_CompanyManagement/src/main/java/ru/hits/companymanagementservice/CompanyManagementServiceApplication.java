package ru.hits.companymanagementservice;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.context.properties.ConfigurationPropertiesScan;

@ConfigurationPropertiesScan("ru.hits.companymanagementservice")
@SpringBootApplication
public class CompanyManagementServiceApplication {

	public static void main(String[] args) {
		SpringApplication.run(CompanyManagementServiceApplication.class, args);
	}

}
