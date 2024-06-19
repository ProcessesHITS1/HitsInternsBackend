package ru.hits.thirdcourseservice.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import ru.hits.thirdcourseservice.dto.*;
import ru.hits.thirdcourseservice.service.StudentInSemesterService;

import javax.validation.Valid;
import java.util.UUID;

@RestController
@RequestMapping("/api/students-in-semesters")
@RequiredArgsConstructor
@Slf4j
@Tag(name = "Студенты в семестрах.")
public class StudentInSemesterController {

    private final StudentInSemesterService studentInSemesterService;

    @Operation(
            summary = "Добавить студентов в семестр.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PostMapping
    public ResponseEntity<Void> addStudentsToSemester(@RequestBody @Valid AddStudentsToSemesterDto addStudentsToSemesterDto) {
        studentInSemesterService.addStudentsToSemester(addStudentsToSemesterDto);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Перенести студентов на третий курс.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PostMapping("/transfer-to-third-course")
    public ResponseEntity<Void> transferToThirdCourse(@RequestBody @Valid TransferStudentsToThirdCourseDto transferStudentsToThirdCourseDto) {
        studentInSemesterService.transferToThirdCourse(transferStudentsToThirdCourseDto);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Получить список всех студентов в семестрах с пагинацией.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping
    public ResponseEntity<StudentsInSemesterWithPaginationDto> getAllStudentsInSemester(
            @RequestParam(defaultValue = "1") int page,
            @RequestParam(defaultValue = "10") int size
    ) {
        StudentsInSemesterWithPaginationDto studentsInSemesterWithPaginationDto = studentInSemesterService.getAllStudentsInSemester(page, size);
        return new ResponseEntity<>(studentsInSemesterWithPaginationDto, HttpStatus.OK);
    }

    @Operation(
            summary = "Получить информацию о студенте в семестре по ID.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping("/{id}")
    public ResponseEntity<StudentInSemesterDto> getStudentInSemester(@PathVariable UUID id) {
        StudentInSemesterDto studentInSemesterDto = studentInSemesterService.getStudentInSemester(id);
        return new ResponseEntity<>(studentInSemesterDto, HttpStatus.OK);
    }

    @Operation(
            summary = "Обновить данные студента в семестре.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PutMapping("/{id}")
    public ResponseEntity<Void> updateStudentInSemester(@PathVariable UUID id, @RequestBody @Valid UpdateStudentInSemesterDto updateStudentInSemesterDto) {
        studentInSemesterService.updateStudentInSemester(id, updateStudentInSemesterDto);
        return new ResponseEntity<>(HttpStatus.OK);
    }

}
