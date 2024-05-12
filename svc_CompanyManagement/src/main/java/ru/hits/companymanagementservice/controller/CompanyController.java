package ru.hits.companymanagementservice.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import ru.hits.companymanagementservice.dto.CompaniesWithPaginationDto;
import ru.hits.companymanagementservice.dto.CompanyDto;
import ru.hits.companymanagementservice.dto.CreateUpdateCompanyDto;
import ru.hits.companymanagementservice.service.CompanyService;

import javax.validation.Valid;
import java.util.UUID;

@RestController
@RequestMapping("/api/companies")
@RequiredArgsConstructor
@Slf4j
@Tag(name = "Компании.")
public class CompanyController {

    private final CompanyService companyService;

    @Operation(
            summary = "Создать компанию.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PostMapping
    public ResponseEntity<Void> createCompany(@RequestBody @Valid CreateUpdateCompanyDto createUpdateCompanyDto) {
        companyService.createCompany(createUpdateCompanyDto);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Обновить данные компании.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PutMapping("/{id}")
    public ResponseEntity<Void> updateCompany(@PathVariable UUID id, @RequestBody @Valid CreateUpdateCompanyDto createUpdateCompanyDto) {
        companyService.updateCompany(id, createUpdateCompanyDto);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Удалить компанию.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteCompany(@PathVariable UUID id) {
        companyService.deleteCompany(id);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Получить информацию о компании по ID.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping("/{id}")
    public ResponseEntity<CompanyDto> getCompany(@PathVariable UUID id) {
        CompanyDto companyDto = companyService.getCompany(id);
        return new ResponseEntity<>(companyDto, HttpStatus.OK);
    }

    @Operation(
            summary = "Получить список всех компаний с пагинацией.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping
    public ResponseEntity<CompaniesWithPaginationDto> getAllCompanies(
            @RequestParam(defaultValue = "1") int page,
            @RequestParam(defaultValue = "10") int size
    ) {
        CompaniesWithPaginationDto companiesWithPaginationDto = companyService.getAllCompanies(page, size);
        return new ResponseEntity<>(companiesWithPaginationDto, HttpStatus.OK);
    }

}
