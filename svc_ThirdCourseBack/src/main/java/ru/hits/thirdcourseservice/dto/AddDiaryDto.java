package ru.hits.thirdcourseservice.dto;

import lombok.*;
import ru.hits.thirdcourseservice.entity.DiaryFeedbackEntity;
import ru.hits.thirdcourseservice.entity.StudentInSemesterEntity;

import javax.persistence.OneToOne;
import java.time.LocalDateTime;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class AddDiaryDto {

    private UUID documentId;

    private UUID studentInSemesterId;

}
