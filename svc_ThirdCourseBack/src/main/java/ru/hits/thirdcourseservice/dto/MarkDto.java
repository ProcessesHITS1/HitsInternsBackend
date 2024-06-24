package ru.hits.thirdcourseservice.dto;

import lombok.*;
import org.hibernate.annotations.GenericGenerator;
import ru.hits.thirdcourseservice.entity.MarkRequirementEntity;
import ru.hits.thirdcourseservice.entity.StudentInSemesterEntity;

import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
@Builder
public class MarkDto {
    private UUID id;
    private Integer value;
    private StudentInSemesterDto student;
    private MarkRequirementDto markRequirement;
}
