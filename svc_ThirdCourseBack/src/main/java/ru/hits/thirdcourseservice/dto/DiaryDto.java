package ru.hits.thirdcourseservice.dto;

import com.fasterxml.jackson.annotation.JsonFormat;
import lombok.*;
import ru.hits.thirdcourseservice.entity.DiaryFeedbackEntity;

import javax.persistence.OneToOne;
import java.time.LocalDateTime;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Builder
@Getter
@Setter
public class DiaryDto {

    private UUID id;

    private UUID documentId;

    @JsonFormat(pattern = "yyyy-MM-dd HH:mm:ss")
    private LocalDateTime attachedAt;

    private DiaryFeedbackDto diaryFeedback;

}
