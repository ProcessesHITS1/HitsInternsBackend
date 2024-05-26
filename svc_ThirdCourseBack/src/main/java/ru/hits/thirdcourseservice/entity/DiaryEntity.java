package ru.hits.thirdcourseservice.entity;

import lombok.*;
import org.hibernate.annotations.GenericGenerator;

import javax.persistence.*;
import java.time.LocalDateTime;
import java.util.UUID;

@Entity
@NoArgsConstructor
@AllArgsConstructor
@Getter
@Builder
@Setter
@Table(name = "diary")
public class DiaryEntity {
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

    private UUID documentId;

    private LocalDateTime attachedAt;

    @OneToOne(mappedBy = "diary")
    private DiaryFeedbackEntity diaryFeedback;

    @OneToOne(mappedBy = "diary")
    private StudentInSemesterEntity studentInSemesterEntity;

}
