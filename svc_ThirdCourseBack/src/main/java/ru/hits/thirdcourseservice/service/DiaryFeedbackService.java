package ru.hits.thirdcourseservice.service;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import ru.hits.thirdcourseservice.dto.AddDiaryDto;
import ru.hits.thirdcourseservice.dto.AddDiaryFeedbackDto;
import ru.hits.thirdcourseservice.entity.DiaryEntity;
import ru.hits.thirdcourseservice.entity.DiaryFeedbackEntity;
import ru.hits.thirdcourseservice.entity.StudentInSemesterEntity;
import ru.hits.thirdcourseservice.exception.NotFoundException;
import ru.hits.thirdcourseservice.repository.DiaryFeedbackRepository;
import ru.hits.thirdcourseservice.repository.DiaryRepository;

import java.time.LocalDateTime;

@Slf4j
@Service
@RequiredArgsConstructor
public class DiaryFeedbackService {

    private final DiaryFeedbackRepository diaryFeedbackRepository;
    private final DiaryRepository diaryRepository;

    @Transactional
    public void addDiaryFeedback(AddDiaryFeedbackDto addDiaryFeedbackDto) {
        DiaryEntity diary = diaryRepository.findById(addDiaryFeedbackDto.getDiaryId())
                .orElseThrow(() -> new NotFoundException("Дневник с ID " + addDiaryFeedbackDto.getDiaryId() + " не найден"));

        if (diaryFeedbackRepository.findByDiary(diary).isPresent()) {
            diaryFeedbackRepository.delete(diaryFeedbackRepository.findByDiary(diary).get());

            DiaryFeedbackEntity diaryFeedback = DiaryFeedbackEntity.builder()
                    .diary(diary)
                    .comments(addDiaryFeedbackDto.getComments())
                    .acceptanceStatus(addDiaryFeedbackDto.getAcceptanceStatus())
                    .build();

            diaryFeedbackRepository.save(diaryFeedback);
        } else {
            DiaryFeedbackEntity diaryFeedback = DiaryFeedbackEntity.builder()
                    .diary(diary)
                    .comments(addDiaryFeedbackDto.getComments())
                    .acceptanceStatus(addDiaryFeedbackDto.getAcceptanceStatus())
                    .build();

            diaryFeedbackRepository.save(diaryFeedback);
        }
    }

}
