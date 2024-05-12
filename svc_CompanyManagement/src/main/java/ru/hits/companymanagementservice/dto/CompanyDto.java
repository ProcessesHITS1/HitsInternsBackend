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

    private List<UUID> seasonIds;

    public CompanyDto(CompanyEntity company, List<String> contacts, List<UUID> seasonIds) {
        this.id = company.getId();
        this.name = company.getName();
        this.contacts = contacts;
        this.seasonIds = seasonIds;
    }

}
