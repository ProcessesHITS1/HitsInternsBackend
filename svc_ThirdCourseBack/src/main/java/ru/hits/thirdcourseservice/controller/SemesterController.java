package ru.hits.thirdcourseservice.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import ru.hits.thirdcourseservice.dto.CloneSemesterDto;
import ru.hits.thirdcourseservice.dto.CreateUpdateSemesterDto;
import ru.hits.thirdcourseservice.dto.SemesterDto;
import ru.hits.thirdcourseservice.dto.SemestersWithPaginationDto;
import ru.hits.thirdcourseservice.service.SemesterService;

import javax.validation.Valid;
import java.util.UUID;

@RestController
@RequestMapping("/api/semesters")
@RequiredArgsConstructor
@Slf4j
@Tag(name = "Семестры.")
public class SemesterController {

    private final SemesterService semesterService;

    @Operation(
            summary = "Создать семестр.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PostMapping
    public ResponseEntity<Void> createSemester(@RequestBody @Valid CreateUpdateSemesterDto createUpdateSemesterDto) {
        semesterService.createSemester(createUpdateSemesterDto);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Обновить данные семестра.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PutMapping("/{id}")
    public ResponseEntity<Void> updateSemester(@PathVariable UUID id, @RequestBody @Valid CreateUpdateSemesterDto createUpdateSemesterDto) {
        semesterService.updateSemester(id, createUpdateSemesterDto);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Закрыть семестр.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PutMapping("/{id}/close")
    public ResponseEntity<Void> closeSemester(@PathVariable UUID id) {
        semesterService.closeSemester(id);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Клонировать семестр.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PostMapping("/clone")
    public ResponseEntity<Void> cloneSemester(@RequestBody @Valid CloneSemesterDto cloneSemesterDto) {
        semesterService.cloneSemester(cloneSemesterDto);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Получить список всех семестров с пагинацией.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping
    public ResponseEntity<SemestersWithPaginationDto> getAllSemesters(
            @RequestParam(defaultValue = "1") int page,
            @RequestParam(defaultValue = "10") int size
    ) {
        SemestersWithPaginationDto semestersWithPaginationDto = semesterService.getAllSemesters(page, size);
        return new ResponseEntity<>(semestersWithPaginationDto, HttpStatus.OK);
    }

    @Operation(
            summary = "Получить информацию о семестре по ID.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping("/{id}")
    public ResponseEntity<SemesterDto> getSemester(@PathVariable UUID id) {
        SemesterDto semesterDto = semesterService.getSemester(id);
        return new ResponseEntity<>(semesterDto, HttpStatus.OK);
    }

}
