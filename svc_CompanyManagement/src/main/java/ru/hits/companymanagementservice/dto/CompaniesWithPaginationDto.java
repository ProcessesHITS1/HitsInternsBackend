package ru.hits.companymanagementservice.dto;

import lombok.*;

import java.util.List;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class CompaniesWithPaginationDto {

    private PageInfoDto pageInfo;

    private List<CompanyDto> data;

}
