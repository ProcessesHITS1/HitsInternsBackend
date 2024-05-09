package ru.hits.authservice.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import ru.hits.authservice.dto.CreateStudentGroupDto;
import ru.hits.authservice.dto.CreateUserDto;
import ru.hits.authservice.dto.StudentGroupInfoDto;
import ru.hits.authservice.service.StudentGroupService;

import javax.validation.Valid;
import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/student-groups")
@RequiredArgsConstructor
@Slf4j
@Tag(name = "Группы.")
public class StudentGroupController {

    private final StudentGroupService studentGroupService;

    @Operation(
            summary = "Создать группу.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PostMapping
    public ResponseEntity<Void> createStudentGroup(@RequestBody @Valid CreateStudentGroupDto createStudentGroupDto) {
        studentGroupService.createStudentGroup(createStudentGroupDto.getNumber());
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Получить список всех групп.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping
    public ResponseEntity<List<StudentGroupInfoDto>> getStudentGroups() {
        List<StudentGroupInfoDto> studentGroups = studentGroupService.getStudentGroups();
        return new ResponseEntity<>(studentGroups, HttpStatus.OK);
    }

}
