package ru.hits.thirdcourseservice.dto;

import lombok.*;
import ru.hits.thirdcourseservice.entity.DiaryEntity;
import ru.hits.thirdcourseservice.entity.SemesterEntity;

import javax.persistence.JoinColumn;
import javax.persistence.ManyToOne;
import javax.persistence.OneToOne;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Builder
@Getter
@Setter
public class StudentInSemesterDto {

    private UUID id;

    private UUID studentId;

    private UUID companyId;

    private UUID semesterId;

    private UUID diaryId;

    private Boolean internshipPassed;

}
