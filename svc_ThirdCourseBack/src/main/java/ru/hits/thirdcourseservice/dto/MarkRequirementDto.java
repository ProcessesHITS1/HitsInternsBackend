package ru.hits.thirdcourseservice.dto;

import lombok.*;

import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class MarkRequirementDto {
    private UUID id;
    private String description;
}
