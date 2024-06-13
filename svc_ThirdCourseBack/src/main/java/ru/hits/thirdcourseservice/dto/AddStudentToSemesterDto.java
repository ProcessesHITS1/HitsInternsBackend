package ru.hits.thirdcourseservice.dto;

import lombok.*;

import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class AddStudentToSemesterDto {

    private UUID studentId;

    private UUID companyId;

    private UUID semesterId;

}
