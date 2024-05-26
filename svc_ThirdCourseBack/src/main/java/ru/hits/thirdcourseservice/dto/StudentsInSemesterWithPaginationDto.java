package ru.hits.thirdcourseservice.dto;

import lombok.*;

import java.util.List;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class StudentsInSemesterWithPaginationDto {

    private PageInfoDto pageInfo;

    private List<StudentInSemesterDto> data;

}
