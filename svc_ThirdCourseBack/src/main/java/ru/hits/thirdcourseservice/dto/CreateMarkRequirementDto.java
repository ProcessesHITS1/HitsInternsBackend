package ru.hits.thirdcourseservice.dto;

import lombok.*;

import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class CreateMarkRequirementDto {
    private String description;

    private UUID semesterId;

}
