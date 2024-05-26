package ru.hits.thirdcourseservice.dto;

import com.fasterxml.jackson.annotation.JsonFormat;
import lombok.*;

import java.time.LocalDateTime;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Builder
@Setter
public class SemesterDto {

    private UUID id;

    private Integer year;

    private Integer semester;

    private UUID seasonId;

    @JsonFormat(pattern = "yyyy-MM-dd HH:mm:ss")
    private LocalDateTime documentsDeadline;

}
