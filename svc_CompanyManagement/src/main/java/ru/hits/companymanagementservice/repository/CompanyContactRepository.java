package ru.hits.companymanagementservice.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import ru.hits.companymanagementservice.entity.CompanyContactEntity;
import ru.hits.companymanagementservice.entity.CompanyEntity;

import java.util.List;
import java.util.UUID;

@Repository
public interface CompanyContactRepository extends JpaRepository<CompanyContactEntity, UUID> {

    void deleteAllByCompanyId(UUID companyId);

    List<CompanyContactEntity> findAllByCompanyId(UUID companyId);

}
