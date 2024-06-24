package ru.hits.thirdcourseservice.dto;

import lombok.*;
import ru.hits.thirdcourseservice.entity.DiaryEntity;
import ru.hits.thirdcourseservice.entity.SemesterEntity;
import ru.hits.thirdcourseservice.entity.StudentInSemesterEntity;

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

    public StudentInSemesterDto(StudentInSemesterEntity entity) {
        this.id = entity.getId();
        this.studentId = entity.getStudentId();
        this.companyId = entity.getCompanyId();
        this.semesterId = entity.getSemester() != null ? entity.getSemester().getId() : null;
        this.diaryId = entity.getDiary() != null ? entity.getDiary().getId() : null;
        this.internshipPassed = entity.getInternshipPassed();
    }

}
