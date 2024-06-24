package ru.hits.thirdcourseservice.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import ru.hits.thirdcourseservice.dto.CreateMarkRequirementDto;
import ru.hits.thirdcourseservice.dto.MarkRequirementDto;
import ru.hits.thirdcourseservice.service.MarkRequirementService;

import javax.validation.Valid;
import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/mark-requirements")
@RequiredArgsConstructor
@Slf4j
@Tag(name = "Требования к оценке.")
public class MarkRequirementController {

    private final MarkRequirementService markRequirementService;

    @Operation(
            summary = "Создать требование к оценке.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PostMapping
    public ResponseEntity<Void> createMarkRequirement(@RequestBody @Valid CreateMarkRequirementDto createMarkRequirementDto) {
        markRequirementService.createMarkRequirement(createMarkRequirementDto);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Получить требования к оценке.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping
    public ResponseEntity<List<MarkRequirementDto>> getMarkRequirements() {
        return new ResponseEntity<>(markRequirementService.getMarkRequirements(), HttpStatus.OK);
    }

    @Operation(
            summary = "Получить требования к оценкам в конкретном семестре.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping("/semester/{semesterId}")
    public ResponseEntity<List<MarkRequirementDto>> getMarkRequirements(@PathVariable UUID semesterId) {
        List<MarkRequirementDto> markRequirements =  markRequirementService.getMarkRequirementsForSemester(semesterId);
        return new ResponseEntity<>(markRequirements, HttpStatus.OK);
    }

}
