package ru.hits.thirdcourseservice.dto;

import lombok.*;

import java.util.List;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class TransferStudentsToThirdCourseDto {

    private Integer year;

    private UUID seasonId;

    private List<StudentTransferToThirdCourseDto> students;

}
