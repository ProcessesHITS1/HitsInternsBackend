package ru.hits.authservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import ru.hits.authservice.entity.StudentGroupEntity;
import ru.hits.authservice.entity.UserEntity;

import java.util.Optional;
import java.util.UUID;

@Repository
public interface StudentGroupRepository extends JpaRepository<StudentGroupEntity, UUID> {
}
