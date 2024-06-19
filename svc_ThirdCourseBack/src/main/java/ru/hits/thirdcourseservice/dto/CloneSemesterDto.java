package ru.hits.thirdcourseservice.dto;

import lombok.*;

import javax.validation.constraints.NotNull;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class CloneSemesterDto {

    @NotNull
    private UUID semesterIdToClone;

    @NotNull
    private CreateUpdateSemesterDto newSemesterData;

}
