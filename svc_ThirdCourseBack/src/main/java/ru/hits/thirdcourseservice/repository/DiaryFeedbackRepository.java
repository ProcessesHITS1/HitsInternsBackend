package ru.hits.thirdcourseservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import ru.hits.thirdcourseservice.entity.DiaryEntity;
import ru.hits.thirdcourseservice.entity.DiaryFeedbackEntity;

import java.util.UUID;

@Repository
public interface DiaryFeedbackRepository extends JpaRepository<DiaryFeedbackEntity, UUID> {
}
