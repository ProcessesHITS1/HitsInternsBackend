package ru.hits.thirdcourseservice.dto;

import lombok.*;

import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class StudentTransferToThirdCourseDto {

    private UUID id;

    private UUID companyId;

}
