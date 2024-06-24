package ru.hits.thirdcourseservice.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import ru.hits.thirdcourseservice.dto.AddDiaryDto;
import ru.hits.thirdcourseservice.dto.DiaryDto;
import ru.hits.thirdcourseservice.service.DiaryService;

import javax.validation.Valid;
import java.util.UUID;

@RestController
@RequestMapping("/api/diaries")
@RequiredArgsConstructor
@Slf4j
@Tag(name = "Дневники.")
public class DiaryController {

    private final DiaryService diaryService;

    @Operation(
            summary = "Добавить дневник.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PostMapping
    public ResponseEntity<Void> addDiary(@RequestBody @Valid AddDiaryDto addDiaryDto) {
        diaryService.addDiary(addDiaryDto);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Получить информацию о дневнике студента по ID.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping("/{id}")
    public ResponseEntity<DiaryDto> getDiary(@PathVariable UUID id) {
        DiaryDto diaryDto = diaryService.getDiary(id);
        return new ResponseEntity<>(diaryDto, HttpStatus.OK);
    }

    @Operation(
            summary = "Получить информацию о дневнике студента в семестре.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping("/semester/{semesterId}/student/{studentId}")
    public ResponseEntity<DiaryDto> getStudentDiary(@PathVariable UUID semesterId, @PathVariable UUID studentId) {
        DiaryDto studentDiary = diaryService.getStudentDiary(semesterId, studentId);
        return new ResponseEntity<>(studentDiary, HttpStatus.OK);
    }

}
