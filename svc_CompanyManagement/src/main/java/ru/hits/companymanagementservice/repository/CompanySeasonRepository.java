package ru.hits.companymanagementservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import ru.hits.companymanagementservice.entity.CompanyContactEntity;
import ru.hits.companymanagementservice.entity.CompanyEntity;
import ru.hits.companymanagementservice.entity.CompanySeasonEntity;

import java.util.List;
import java.util.UUID;

@Repository
public interface CompanySeasonRepository extends JpaRepository<CompanySeasonEntity, UUID> {

    void deleteAllByCompanyId(UUID companyId);

    List<CompanySeasonEntity> findAllByCompanyId(UUID companyId);

}
