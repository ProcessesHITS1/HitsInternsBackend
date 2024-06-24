package ru.hits.thirdcourseservice.dto;

import lombok.*;
import ru.hits.thirdcourseservice.entity.MarkRequirementEntity;

import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class MarkRequirementDto {
    private UUID id;
    private String description;
    private UUID semesterId;

    public MarkRequirementDto(MarkRequirementEntity entity) {
        this.id = entity.getId();
        this.description = entity.getDescription();
        this.semesterId = entity.getSemester().getId();
    }
}
