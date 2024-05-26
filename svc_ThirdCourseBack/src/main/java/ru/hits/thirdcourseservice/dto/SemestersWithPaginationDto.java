package ru.hits.thirdcourseservice.dto;

import lombok.*;

import java.util.List;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class SemestersWithPaginationDto {

    private PageInfoDto pageInfo;

    private List<SemesterDto> data;

}
