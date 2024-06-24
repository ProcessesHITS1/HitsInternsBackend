package ru.hits.thirdcourseservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import ru.hits.thirdcourseservice.entity.MarkRequirementEntity;
import ru.hits.thirdcourseservice.entity.SemesterEntity;

import java.util.List;
import java.util.UUID;

public interface MarkRequirementRepository extends JpaRepository<MarkRequirementEntity, UUID> {

    List<MarkRequirementEntity> findAllBySemester(SemesterEntity semester);

}
