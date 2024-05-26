package ru.hits.thirdcourseservice.service;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import ru.hits.thirdcourseservice.dto.AddDiaryDto;
import ru.hits.thirdcourseservice.dto.DiaryDto;
import ru.hits.thirdcourseservice.dto.DiaryFeedbackDto;
import ru.hits.thirdcourseservice.dto.SemesterDto;
import ru.hits.thirdcourseservice.entity.DiaryEntity;
import ru.hits.thirdcourseservice.entity.SemesterEntity;
import ru.hits.thirdcourseservice.entity.StudentInSemesterEntity;
import ru.hits.thirdcourseservice.exception.NotFoundException;
import ru.hits.thirdcourseservice.repository.DiaryRepository;
import ru.hits.thirdcourseservice.repository.StudentInSemesterRepository;

import java.time.LocalDateTime;
import java.util.UUID;

@Slf4j
@Service
@RequiredArgsConstructor
public class DiaryService {

    private final DiaryRepository diaryRepository;
    private final StudentInSemesterRepository studentInSemesterRepository;

    public void addDiary(AddDiaryDto addDiaryDto) {
        StudentInSemesterEntity studentInSemester = studentInSemesterRepository.findById(addDiaryDto.getStudentInSemesterId())
                .orElseThrow(() -> new NotFoundException("Студент в семестре с ID " + addDiaryDto.getStudentInSemesterId() + " не найден"));

        DiaryEntity diary = DiaryEntity.builder()
                .documentId(addDiaryDto.getDocumentId())
                .attachedAt(LocalDateTime.now())
                .studentInSemesterEntity(studentInSemester)
                .build();

        diaryRepository.save(diary);
        studentInSemester.setDiary(diary);
        studentInSemesterRepository.save(studentInSemester);
    }

    public DiaryDto getDiary(UUID diaryId) {
        DiaryEntity diary = diaryRepository.findById(diaryId)
                .orElseThrow(() -> new NotFoundException("Дневник с ID " + diaryId + " не найден"));

        return DiaryDto.builder()
                .id(diary.getId())
                .documentId(diary.getDocumentId())
                .attachedAt(diary.getAttachedAt())
                .diaryFeedback(diary.getDiaryFeedback() != null ? new DiaryFeedbackDto(diary.getDiaryFeedback()) : null)
                .build();
    }

}
