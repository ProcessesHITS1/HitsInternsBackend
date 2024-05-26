package ru.hits.thirdcourseservice.dto;

import lombok.*;
import org.hibernate.annotations.Type;
import ru.hits.thirdcourseservice.entity.DiaryEntity;
import ru.hits.thirdcourseservice.entity.DiaryFeedbackEntity;
import ru.hits.thirdcourseservice.enumeration.AcceptanceStatus;

import javax.persistence.*;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class DiaryFeedbackDto {

    private UUID id;

    private String comments;

    private AcceptanceStatus acceptanceStatus;

    public DiaryFeedbackDto(DiaryFeedbackEntity diaryFeedback) {
        this.id = diaryFeedback.getId();
        this.comments = diaryFeedback.getComments();
        this.acceptanceStatus = diaryFeedback.getAcceptanceStatus();
    }

}
