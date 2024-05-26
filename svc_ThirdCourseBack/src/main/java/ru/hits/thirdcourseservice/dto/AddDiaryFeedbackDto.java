package ru.hits.thirdcourseservice.dto;

import lombok.*;
import org.hibernate.annotations.Type;
import ru.hits.thirdcourseservice.entity.DiaryEntity;
import ru.hits.thirdcourseservice.enumeration.AcceptanceStatus;

import javax.persistence.*;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class AddDiaryFeedbackDto {

    private UUID diaryId;

    private String comments;

    private AcceptanceStatus acceptanceStatus;

}
