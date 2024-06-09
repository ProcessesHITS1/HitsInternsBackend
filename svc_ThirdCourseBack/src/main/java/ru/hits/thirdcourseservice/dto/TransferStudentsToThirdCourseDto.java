package ru.hits.thirdcourseservice.dto;

import lombok.*;

import java.util.List;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class TransferStudentsToThirdCourseDto {

    private Integer year;

    private List<StudentTransferToThirdCourseDto> students;

}
