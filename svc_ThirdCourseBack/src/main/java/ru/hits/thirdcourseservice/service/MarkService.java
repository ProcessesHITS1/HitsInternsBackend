package ru.hits.thirdcourseservice.service;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import ru.hits.thirdcourseservice.dto.AssignMarkDto;
import ru.hits.thirdcourseservice.dto.MarkDto;
import ru.hits.thirdcourseservice.dto.MarkRequirementDto;
import ru.hits.thirdcourseservice.dto.StudentInSemesterDto;
import ru.hits.thirdcourseservice.entity.MarkEntity;
import ru.hits.thirdcourseservice.entity.MarkRequirementEntity;
import ru.hits.thirdcourseservice.entity.StudentInSemesterEntity;
import ru.hits.thirdcourseservice.exception.NotFoundException;
import ru.hits.thirdcourseservice.repository.MarkRepository;
import ru.hits.thirdcourseservice.repository.MarkRequirementRepository;
import ru.hits.thirdcourseservice.repository.StudentInSemesterRepository;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@Slf4j
@Service
@RequiredArgsConstructor
public class MarkService {

    private final MarkRepository markRepository;
    private final StudentInSemesterRepository studentInSemesterRepository;
    private final MarkRequirementRepository markRequirementRepository;

    @Transactional
    public void assignMark(AssignMarkDto assignMarkDto, UUID studentInSemesterId) {
        StudentInSemesterEntity studentInSemester = studentInSemesterRepository.findById(studentInSemesterId)
                .orElseThrow(() -> new NotFoundException("Студент в семестре с ID " + studentInSemesterId + " не найден"));

        MarkRequirementEntity markRequirement = markRequirementRepository.findById(assignMarkDto.getMarkRequirementId())
                .orElseThrow(() -> new NotFoundException("Требование к оценке с ID " + assignMarkDto.getMarkRequirementId() + " не найдено"));

        MarkEntity mark = MarkEntity.builder()
                .student(studentInSemester)
                .markRequirement(markRequirement)
                .value(assignMarkDto.getValue())
                .build();

        markRepository.save(mark);
    }

    public List<MarkDto> getMarksByStudentInSemesterId(UUID studentInSemesterId) {
        StudentInSemesterEntity student = studentInSemesterRepository.findById(studentInSemesterId)
                .orElseThrow(() -> new NotFoundException("Студент в семестре с ID " + studentInSemesterId + " не найден"));
        List<MarkEntity> studentMarks = markRepository.findAllByStudent(student);

        List<MarkDto> result = new ArrayList<>();
        for (MarkEntity mark : studentMarks) {
            result.add(
                    new MarkDto(
                            mark.getId(),
                            mark.getValue(),
                            new StudentInSemesterDto(
                                    student.getId(),
                                    student.getStudentId(),
                                    student.getCompanyId(),
                                    student.getSemester().getId(),
                                    student.getDiary() != null ? student.getDiary().getId() : null,
                                    student.getInternshipPassed()
                            ),
                            new MarkRequirementDto(
                                    mark.getMarkRequirement().getId(),
                                    mark.getMarkRequirement().getDescription(),
                                    mark.getMarkRequirement().getSemester() != null ? mark.getMarkRequirement().getSemester().getId() : null
                            )
                    )
            );
        }

        return result;
    }

}
