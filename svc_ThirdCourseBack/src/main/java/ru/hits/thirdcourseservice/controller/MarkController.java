package ru.hits.thirdcourseservice.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import ru.hits.thirdcourseservice.dto.AssignMarkDto;
import ru.hits.thirdcourseservice.dto.MarkDto;
import ru.hits.thirdcourseservice.dto.UpdateMarkDto;
import ru.hits.thirdcourseservice.service.MarkService;

import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/marks")
@RequiredArgsConstructor
@Slf4j
@Tag(name = "Оценки.")
public class MarkController {

    private final MarkService markService;

    @Operation(
            summary = "Назначить оценку.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PostMapping("/student-in-semester/{studentInSemesterId}")
    public ResponseEntity<Void> assignMark(@RequestBody AssignMarkDto assignMarkDto, @PathVariable UUID studentInSemesterId) {
        markService.assignMark(assignMarkDto, studentInSemesterId);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Обновить оценку.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PutMapping("/{id}")
    public ResponseEntity<Void> updateMark(@RequestBody UpdateMarkDto updateMarkDto, @PathVariable UUID id) {
        markService.updateMark(updateMarkDto, id);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Получить оценки студента в семестре.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping("/student-in-semester/{studentInSemesterId}")
    public ResponseEntity<List<MarkDto>> getMarksByStudent(@PathVariable UUID studentInSemesterId) {
        List<MarkDto> marks = markService.getMarksByStudentInSemesterId(studentInSemesterId);
        return ResponseEntity.ok(marks);
    }

    @Operation(
            summary = "Получить собственные оценки за семестр.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping("/semester/{semesterId}/my")
    public ResponseEntity<List<MarkDto>> getMyMarksForSemester(@PathVariable UUID semesterId) {
        List<MarkDto> myMarks = markService.getMyMarksForSemester(semesterId);
        return new ResponseEntity<>(myMarks, HttpStatus.OK);
    }

}
