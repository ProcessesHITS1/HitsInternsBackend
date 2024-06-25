package ru.hits.thirdcourseservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import ru.hits.thirdcourseservice.entity.SemesterEntity;
import ru.hits.thirdcourseservice.entity.StudentInSemesterEntity;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

@Repository
public interface StudentInSemesterRepository extends JpaRepository<StudentInSemesterEntity, UUID>  {
    List<StudentInSemesterEntity> findAllBySemester(SemesterEntity semester);

    List<StudentInSemesterEntity> findAllByStudentId(UUID studentId);

    Optional<StudentInSemesterEntity> findByStudentId(UUID studentId);

    Optional<StudentInSemesterEntity> findByIdAndSemester(UUID id, SemesterEntity semester);
}
