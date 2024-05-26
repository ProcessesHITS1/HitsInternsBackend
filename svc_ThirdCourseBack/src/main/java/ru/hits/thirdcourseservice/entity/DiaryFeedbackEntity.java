package ru.hits.thirdcourseservice.entity;

import lombok.*;
import org.hibernate.annotations.GenericGenerator;
import org.hibernate.annotations.Type;
import ru.hits.thirdcourseservice.enumeration.AcceptanceStatus;

import javax.persistence.*;
import java.util.UUID;

@Entity
@NoArgsConstructor
@AllArgsConstructor
@Builder
@Getter
@Setter
@Table(name = "diary_feedback")
public class DiaryFeedbackEntity {

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

    @OneToOne
    @JoinColumn(name = "diary_id", nullable = false)
    private DiaryEntity diary;

    @Lob
    @Type(type = "org.hibernate.type.TextType")
    private String comments;

    @Enumerated(EnumType.STRING)
    private AcceptanceStatus acceptanceStatus;

}
