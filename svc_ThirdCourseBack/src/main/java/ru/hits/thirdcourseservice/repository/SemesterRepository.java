package ru.hits.thirdcourseservice.repository;

import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import ru.hits.thirdcourseservice.entity.SemesterEntity;

import java.util.Optional;
import java.util.UUID;

@Repository
public interface SemesterRepository extends JpaRepository<SemesterEntity, UUID>  {

    Optional<SemesterEntity> findByYearAndSemester(Integer year, Integer semester);

    Page<SemesterEntity> findAllBySeasonId(UUID seasonId, Pageable pageable);

}
