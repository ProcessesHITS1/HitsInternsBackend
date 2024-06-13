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
@Table(name = "mark_requirement")
public class MarkRequirementEntity {

    /**
     * Уникальный идентификатор сущности.
     */
    @Id
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(
            name = "UUID",
            strategy = "org.hibernate.id.UUIDGenerator"
    )
    private UUID id;

    private String description;

    @ManyToOne
    @JoinColumn(name = "semester_id")
    private SemesterEntity semester;

}
