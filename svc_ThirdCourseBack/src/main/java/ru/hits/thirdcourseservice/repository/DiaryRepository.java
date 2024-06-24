package ru.hits.thirdcourseservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import ru.hits.thirdcourseservice.entity.DiaryEntity;
import ru.hits.thirdcourseservice.entity.FileMetadataEntity;
import ru.hits.thirdcourseservice.entity.StudentInSemesterEntity;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

@Repository
public interface DiaryRepository extends JpaRepository<DiaryEntity, UUID> {

    Optional<DiaryEntity> findByStudentInSemesterEntity(StudentInSemesterEntity studentInSemester);

}
