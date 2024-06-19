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
import java.util.stream.Collectors;

@Slf4j
@Service
@RequiredArgsConstructor
public class SemesterService {

    private final SemesterRepository semesterRepository;
    private final StudentInSemesterRepository studentInSemesterRepository;
    private final CheckPaginationInfoService checkPaginationInfoService;

    @Transactional
    public void createSemester(CreateUpdateSemesterDto createUpdateSemesterDto) {
        SemesterEntity semester = SemesterEntity.builder()
                .year(createUpdateSemesterDto.getYear())
                .semester(createUpdateSemesterDto.getSemester())
                .seasonId(createUpdateSemesterDto.getSeasonId())
                .documentsDeadline(createUpdateSemesterDto.getDocumentsDeadline())
                .isClosed(false)
                .build();
        semesterRepository.save(semester);
    }

    @Transactional
    public void updateSemester(UUID semesterId, CreateUpdateSemesterDto createUpdateSemesterDto) {
        SemesterEntity semester = semesterRepository.findById(semesterId)
                .orElseThrow(() -> new NotFoundException("Семестр с ID " + semesterId + " не найден"));

        semester.setYear(createUpdateSemesterDto.getYear());
        semester.setSemester(createUpdateSemesterDto.getSemester());
        semester.setSeasonId(createUpdateSemesterDto.getSeasonId());
        semester.setDocumentsDeadline(createUpdateSemesterDto.getDocumentsDeadline());

        semesterRepository.save(semester);
    }

    @Transactional
    public void cloneSemester(CloneSemesterDto cloneSemesterDto) {
        UUID semesterIdToClone = cloneSemesterDto.getSemesterIdToClone();
        SemesterEntity semesterToClone = semesterRepository.findById(semesterIdToClone)
                .orElseThrow(() -> new NotFoundException("Семестр с ID " + semesterIdToClone + " не найден"));

        CreateUpdateSemesterDto newSemesterData = cloneSemesterDto.getNewSemesterData();

        SemesterEntity newSemester = SemesterEntity.builder()
                .year(newSemesterData.getYear())
                .semester(newSemesterData.getSemester())
                .seasonId(newSemesterData.getSeasonId())
                .documentsDeadline(newSemesterData.getDocumentsDeadline())
                .isClosed(false)
                .build();

        semesterRepository.save(newSemester);

        List<StudentInSemesterEntity> studentsInOldSemester = studentInSemesterRepository.findAllBySemester(semesterToClone);
        for (StudentInSemesterEntity student : studentsInOldSemester) {
            StudentInSemesterEntity newStudentInSemester = StudentInSemesterEntity.builder()
                    .studentId(student.getStudentId())
                    .semester(newSemester)
                    .companyId(student.getCompanyId())
                    .diary(null)
                    .internshipPassed(null)
                    .build();
            studentInSemesterRepository.save(newStudentInSemester);
        }
    }

    @Transactional
    public void closeSemester(UUID semesterId) {
        SemesterEntity semester = semesterRepository.findById(semesterId)
                .orElseThrow(() -> new NotFoundException("Семестр с ID " + semesterId + " не найден"));

        semester.setIsClosed(true);

        semesterRepository.save(semester);
    }

    public SemestersWithPaginationDto getAllSemesters(int page, int size) {
        checkPaginationInfoService.checkPagination(page, size);
        Pageable pageable = PageRequest.of(page - 1, size);
        Page<SemesterEntity> semestersPage = semesterRepository.findAll(pageable);
        PageInfoDto pageInfoDto = new PageInfoDto(
                (int) semestersPage.getTotalElements(),
                page,
                Math.min(size, semestersPage.getContent().size())
        );
        List<SemesterDto> semesterDtos = new ArrayList<>();
        for (SemesterEntity semester : semestersPage.getContent()) {
            SemesterDto semesterDto = SemesterDto.builder()
                    .id(semester.getId())
                    .year(semester.getYear())
                    .semester(semester.getSemester())
                    .seasonId(semester.getSeasonId())
                    .documentsDeadline(semester.getDocumentsDeadline())
                    .isClosed(semester.getIsClosed())
                    .build();

            semesterDtos.add(semesterDto);
        }

        return new SemestersWithPaginationDto(pageInfoDto, semesterDtos);
    }

    public SemesterDto getSemester(UUID semesterId) {
        SemesterEntity semester = semesterRepository.findById(semesterId)
                .orElseThrow(() -> new NotFoundException("Семестр с ID " + semesterId + " не найден"));

        return SemesterDto.builder()
                .id(semester.getId())
                .year(semester.getYear())
                .semester(semester.getSemester())
                .seasonId(semester.getSeasonId())
                .documentsDeadline(semester.getDocumentsDeadline())
                .isClosed(semester.getIsClosed())
                .build();
    }

}
