package ru.hits.companymanagementservice.dto;

import lombok.*;

import java.util.List;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class CreateUpdateCompanyDto {

    private String name;

    private UUID curatorId;

    private List<String> contacts;

}
