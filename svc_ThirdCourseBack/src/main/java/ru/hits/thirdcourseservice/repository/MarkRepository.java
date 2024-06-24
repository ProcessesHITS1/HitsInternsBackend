package ru.hits.thirdcourseservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import ru.hits.thirdcourseservice.entity.MarkEntity;
import ru.hits.thirdcourseservice.entity.MarkRequirementEntity;
import ru.hits.thirdcourseservice.entity.StudentInSemesterEntity;

import java.util.List;
import java.util.UUID;

public interface MarkRepository extends JpaRepository<MarkEntity, UUID> {

    List<MarkEntity> findAllByStudent(StudentInSemesterEntity student);

    List<MarkEntity> findAllByStudentId(UUID studentId);

}
