package ru.hits.thirdcourseservice.dto;

import lombok.*;

import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class AddDiaryWithStudentIdDto {

    private UUID documentId;

    private UUID studentId;

}
