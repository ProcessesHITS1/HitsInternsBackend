package ru.hits.companymanagementservice.service;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import ru.hits.companymanagementservice.dto.CompaniesWithPaginationDto;
import ru.hits.companymanagementservice.dto.CompanyDto;
import ru.hits.companymanagementservice.dto.CreateUpdateCompanyDto;
import ru.hits.companymanagementservice.dto.PageInfoDto;
import ru.hits.companymanagementservice.entity.CompanyContactEntity;
import ru.hits.companymanagementservice.entity.CompanyEntity;
import ru.hits.companymanagementservice.exception.NotFoundException;
import ru.hits.companymanagementservice.helpingservices.CheckPaginationInfoService;
import ru.hits.companymanagementservice.repository.CompanyContactRepository;
import ru.hits.companymanagementservice.repository.CompanyRepository;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
@Slf4j
public class CompanyService {

    private final CompanyRepository companyRepository;

    private final CompanyContactRepository companyContactRepository;

    private final CheckPaginationInfoService checkPaginationInfoService;

    @Transactional
    public void createCompany(CreateUpdateCompanyDto createUpdateCompanyDto) {
        CompanyEntity company = CompanyEntity.builder()
                .name(createUpdateCompanyDto.getName())
                .curatorId(createUpdateCompanyDto.getCuratorId())
                .build();
        company = companyRepository.save(company);

        List<CompanyContactEntity> contacts = new ArrayList<>();
        for (String contact : createUpdateCompanyDto.getContacts()) {
            CompanyContactEntity contactEntity = CompanyContactEntity.builder()
                    .companyId(company.getId())
                    .contact(contact)
                    .build();
            contacts.add(contactEntity);
        }
        companyContactRepository.saveAll(contacts);
    }

    @Transactional
    public void updateCompany(UUID companyId, CreateUpdateCompanyDto createUpdateCompanyDto) {
        CompanyEntity companyEntity = companyRepository.findById(companyId)
                .orElseThrow(() -> new NotFoundException("Компания с ID " + companyId + " не найдена"));

        companyEntity.setName(createUpdateCompanyDto.getName());
        companyEntity.setCuratorId(createUpdateCompanyDto.getCuratorId());
        companyContactRepository.deleteAllByCompanyId(companyId);

        List<CompanyContactEntity> contacts = new ArrayList<>();
        for (String contact : createUpdateCompanyDto.getContacts()) {
            CompanyContactEntity contactEntity = CompanyContactEntity.builder()
                    .companyId(companyId)
                    .contact(contact)
                    .build();
            contacts.add(contactEntity);
        }
        companyContactRepository.saveAll(contacts);

        companyRepository.save(companyEntity);
    }

    @Transactional
    public void deleteCompany(UUID companyId) {
        CompanyEntity companyEntity = companyRepository.findById(companyId)
                .orElseThrow(() -> new NotFoundException("Компания с ID " + companyId + " не найдена"));

        companyRepository.delete(companyEntity);
        companyContactRepository.deleteAllByCompanyId(companyId);
    }

    public CompanyDto getCompany(UUID companyId) {
        CompanyEntity companyEntity = companyRepository.findById(companyId)
                .orElseThrow(() -> new NotFoundException("Компания с ID " + companyId + " не найдена"));

        List<String> contacts = companyContactRepository.findAllByCompanyId(companyId)
                .stream()
                .map(CompanyContactEntity::getContact)
                .collect(Collectors.toList());

        return CompanyDto.builder()
                .id(companyId)
                .name(companyEntity.getName())
                .curatorId(companyEntity.getCuratorId())
                .contacts(contacts)
                .build();
    }

    public CompaniesWithPaginationDto getAllCompanies(int page, int size) {
        checkPaginationInfoService.checkPagination(page, size);
        Pageable pageable = PageRequest.of(page - 1, size);
        Page<CompanyEntity> companiesPage = companyRepository.findAll(pageable);
        PageInfoDto pageInfoDto = new PageInfoDto(
                (int) companiesPage.getTotalElements(),
                page,
                Math.min(size, companiesPage.getContent().size())
        );
        List<CompanyDto> companyDtos = new ArrayList<>();
        for (CompanyEntity company : companiesPage.getContent()) {
            List<String> contacts = companyContactRepository.findAllByCompanyId(company.getId())
                    .stream()
                    .map(CompanyContactEntity::getContact)
                    .collect(Collectors.toList());

            CompanyDto companyDto = CompanyDto.builder()
                    .id(company.getId())
                    .name(company.getName())
                    .curatorId(company.getCuratorId())
                    .contacts(contacts)
                    .build();

            companyDtos.add(companyDto);
        }

        return new CompaniesWithPaginationDto(pageInfoDto, companyDtos);
    }

}
