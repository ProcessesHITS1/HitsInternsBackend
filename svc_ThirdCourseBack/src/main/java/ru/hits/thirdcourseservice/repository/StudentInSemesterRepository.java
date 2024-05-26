package ru.hits.thirdcourseservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import ru.hits.thirdcourseservice.entity.StudentInSemesterEntity;

import java.util.UUID;

@Repository
public interface StudentInSemesterRepository extends JpaRepository<StudentInSemesterEntity, UUID>  {
}
