package ru.hits.thirdcourseservice.service;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import ru.hits.thirdcourseservice.dto.*;
import ru.hits.thirdcourseservice.entity.SemesterEntity;
import ru.hits.thirdcourseservice.entity.StudentInSemesterEntity;
import ru.hits.thirdcourseservice.exception.NotFoundException;
import ru.hits.thirdcourseservice.helpingservices.CheckPaginationInfoService;
import ru.hits.thirdcourseservice.repository.SemesterRepository;
import ru.hits.thirdcourseservice.repository.StudentInSemesterRepository;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@Slf4j
@Service
@RequiredArgsConstructor
public class StudentInSemesterService {

    private final StudentInSemesterRepository studentInSemesterRepository;
    private final SemesterRepository semesterRepository;
    private final CheckPaginationInfoService checkPaginationInfoService;


    @Transactional
    public void addStudentsToSemester(AddStudentsToSemesterDto addStudentsToSemesterDto) {
        for (AddStudentToSemesterDto studentInSemesterDto : addStudentsToSemesterDto.getStudentsInSemester()) {
            SemesterEntity semester = semesterRepository.findById(studentInSemesterDto.getSemesterId())
                    .orElseThrow(() -> new NotFoundException("Семестр с ID " + studentInSemesterDto.getSemesterId() + " не найден"));

            StudentInSemesterEntity studentInSemester = StudentInSemesterEntity.builder()
                    .studentId(studentInSemesterDto.getStudentId())
                    .companyId(studentInSemesterDto.getCompanyId())
                    .semester(semester)
                    .diary(null)
                    .internshipPassed(studentInSemesterDto.isInternshipPassed())
                    .build();

            studentInSemesterRepository.save(studentInSemester);
        }
    }

    public StudentsInSemesterWithPaginationDto getAllStudentsInSemester(int page, int size) {
        checkPaginationInfoService.checkPagination(page, size);
        Pageable pageable = PageRequest.of(page - 1, size);
        Page<StudentInSemesterEntity> studentsInSemesterPage = studentInSemesterRepository.findAll(pageable);
        PageInfoDto pageInfoDto = new PageInfoDto(
                (int) studentsInSemesterPage.getTotalElements(),
                page,
                Math.min(size, studentsInSemesterPage.getContent().size())
        );
        List<StudentInSemesterDto> studentsInSemesterDtos = new ArrayList<>();
        for (StudentInSemesterEntity studentInSemester : studentsInSemesterPage.getContent()) {
            StudentInSemesterDto studentInSemesterDto = StudentInSemesterDto.builder()
                    .id(studentInSemester.getId())
                    .studentId(studentInSemester.getStudentId())
                    .companyId(studentInSemester.getCompanyId())
                    .semesterId(studentInSemester.getSemester().getId())
                    .diaryId(studentInSemester.getDiary() != null ? studentInSemester.getDiary().getId() : null)
                    .internshipPassed(studentInSemester.isInternshipPassed())
                    .build();

            studentsInSemesterDtos.add(studentInSemesterDto);
        }

        return new StudentsInSemesterWithPaginationDto(pageInfoDto, studentsInSemesterDtos);
    }

    public StudentInSemesterDto getStudentInSemester(UUID studentInSemesterId) {
        StudentInSemesterEntity studentInSemester = studentInSemesterRepository.findById(studentInSemesterId)
                .orElseThrow(() -> new NotFoundException("Студент в семестре с ID " + studentInSemesterId + " не найден"));

        return StudentInSemesterDto.builder()
                .id(studentInSemester.getId())
                .studentId(studentInSemester.getStudentId())
                .companyId(studentInSemester.getCompanyId())
                .semesterId(studentInSemester.getSemester().getId())
                .diaryId(studentInSemester.getDiary() != null ? studentInSemester.getDiary().getId() : null)
                .internshipPassed(studentInSemester.isInternshipPassed())
                .build();
    }

}
