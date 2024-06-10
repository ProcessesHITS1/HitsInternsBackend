package ru.hits.thirdcourseservice.service;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import ru.hits.thirdcourseservice.dto.CreateMarkRequirementDto;
import ru.hits.thirdcourseservice.dto.MarkRequirementDto;
import ru.hits.thirdcourseservice.entity.MarkRequirementEntity;
import ru.hits.thirdcourseservice.repository.MarkRequirementRepository;

import java.util.ArrayList;
import java.util.List;

@Slf4j
@Service
@RequiredArgsConstructor
public class MarkRequirementService {

    private final MarkRequirementRepository markRequirementRepository;

    @Transactional
    public void createMarkRequirement(CreateMarkRequirementDto createMarkRequirementDto) {
        MarkRequirementEntity markRequirement = MarkRequirementEntity.builder()
                .description(createMarkRequirementDto.getDescription())
                .build();
        markRequirementRepository.save(markRequirement);
    }

    public List<MarkRequirementDto> getMarkRequirements() {
        List<MarkRequirementEntity> markRequirementEntities = markRequirementRepository.findAll();

        List<MarkRequirementDto> result = new ArrayList<>();
        for (MarkRequirementEntity markRequirement : markRequirementEntities) {
            result.add(new MarkRequirementDto(markRequirement.getId(), markRequirement.getDescription()));
        }

        return result;
    }

}
