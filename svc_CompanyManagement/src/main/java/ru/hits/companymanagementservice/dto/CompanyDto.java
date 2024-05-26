package ru.hits.companymanagementservice.dto;

import lombok.*;
import ru.hits.companymanagementservice.entity.CompanyEntity;

import java.util.List;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
@Builder
public class CompanyDto {

    private UUID id;

    private String name;

    private UUID curatorId;

    private List<String> contacts;

    public CompanyDto(CompanyEntity company, List<String> contacts) {
        this.id = company.getId();
        this.name = company.getName();
        this.curatorId = company.getCuratorId();
        this.contacts = contacts;
    }

}
