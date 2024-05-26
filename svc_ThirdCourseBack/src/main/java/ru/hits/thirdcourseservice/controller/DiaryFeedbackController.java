package ru.hits.thirdcourseservice.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import ru.hits.thirdcourseservice.dto.AddDiaryDto;
import ru.hits.thirdcourseservice.dto.AddDiaryFeedbackDto;
import ru.hits.thirdcourseservice.service.DiaryFeedbackService;

import javax.validation.Valid;

@RestController
@RequestMapping("/api/diaries-feedback")
@RequiredArgsConstructor
@Slf4j
@Tag(name = "Фидбэк дневников.")
public class DiaryFeedbackController {

    private final DiaryFeedbackService diaryFeedbackService;

    @Operation(
            summary = "Добавить фидбэк к дневнику.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PostMapping
    public ResponseEntity<Void> addDiaryFeedback(@RequestBody @Valid AddDiaryFeedbackDto addDiaryFeedbackDto) {
        diaryFeedbackService.addDiaryFeedback(addDiaryFeedbackDto);
        return new ResponseEntity<>(HttpStatus.OK);
    }

}
