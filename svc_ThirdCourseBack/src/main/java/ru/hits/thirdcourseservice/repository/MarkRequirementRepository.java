package ru.hits.thirdcourseservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import ru.hits.thirdcourseservice.entity.MarkRequirementEntity;

import java.util.UUID;

public interface MarkRequirementRepository extends JpaRepository<MarkRequirementEntity, UUID> {
}
