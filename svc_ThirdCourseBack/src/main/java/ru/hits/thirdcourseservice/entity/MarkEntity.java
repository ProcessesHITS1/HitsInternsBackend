package ru.hits.thirdcourseservice.entity;

import lombok.*;
import org.hibernate.annotations.GenericGenerator;

import javax.persistence.*;
import java.util.UUID;

@Entity
@NoArgsConstructor
@AllArgsConstructor
@Getter
@Builder
@Setter
@Table(name = "mark")
public class MarkEntity {

    @Id
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(
            name = "UUID",
            strategy = "org.hibernate.id.UUIDGenerator"
    )
    private UUID id;

    private Integer value;

    @ManyToOne
    @JoinColumn(name = "student_id", nullable = false)
    private StudentInSemesterEntity student;

    @ManyToOne
    @JoinColumn(name = "mark_requirement_id", nullable = false)
    private MarkRequirementEntity markRequirement;

}
