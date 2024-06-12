package ru.hits.thirdcourseservice.service;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import ru.hits.thirdcourseservice.dto.CreateMarkRequirementDto;
import ru.hits.thirdcourseservice.dto.MarkRequirementDto;
import ru.hits.thirdcourseservice.entity.MarkRequirementEntity;
import ru.hits.thirdcourseservice.entity.SemesterEntity;
import ru.hits.thirdcourseservice.exception.NotFoundException;
import ru.hits.thirdcourseservice.repository.MarkRequirementRepository;
import ru.hits.thirdcourseservice.repository.SemesterRepository;

import java.util.ArrayList;
import java.util.List;

@Slf4j
@Service
@RequiredArgsConstructor
public class MarkRequirementService {

    private final MarkRequirementRepository markRequirementRepository;
    private final SemesterRepository semesterRepository;

    @Transactional
    public void createMarkRequirement(CreateMarkRequirementDto createMarkRequirementDto) {
        SemesterEntity semester = semesterRepository.findById(createMarkRequirementDto.getSemesterId())
                .orElseThrow(() -> new NotFoundException("Семестр с ID " + createMarkRequirementDto.getSemesterId() + " не найден"));

        MarkRequirementEntity markRequirement = MarkRequirementEntity.builder()
                .description(createMarkRequirementDto.getDescription())
                .semester(semester)
                .build();
        markRequirementRepository.save(markRequirement);
    }

    public List<MarkRequirementDto> getMarkRequirements() {
        List<MarkRequirementEntity> markRequirementEntities = markRequirementRepository.findAll();

        List<MarkRequirementDto> result = new ArrayList<>();
        for (MarkRequirementEntity markRequirement : markRequirementEntities) {
            result.add(new MarkRequirementDto(
                    markRequirement.getId(),
                    markRequirement.getDescription(),
                    markRequirement.getSemester() != null ? markRequirement.getSemester().getId() : null));
        }

        return result;
    }

}
