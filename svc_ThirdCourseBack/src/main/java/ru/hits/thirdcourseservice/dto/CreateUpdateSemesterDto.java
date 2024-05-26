package ru.hits.thirdcourseservice.dto;

import lombok.*;

import java.time.LocalDateTime;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class CreateUpdateSemesterDto {

    private Integer year;

    private Integer semester;

    private UUID seasonId;

    private LocalDateTime documentsDeadline;

}
