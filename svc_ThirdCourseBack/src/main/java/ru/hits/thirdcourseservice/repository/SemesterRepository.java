package ru.hits.thirdcourseservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import ru.hits.thirdcourseservice.entity.FileMetadataEntity;
import ru.hits.thirdcourseservice.entity.SemesterEntity;

import java.util.UUID;

@Repository
public interface SemesterRepository extends JpaRepository<SemesterEntity, UUID>  {
}
