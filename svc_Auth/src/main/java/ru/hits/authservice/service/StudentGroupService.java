package ru.hits.authservice.service;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import ru.hits.authservice.dto.StudentGroupInfoDto;
import ru.hits.authservice.entity.StudentGroupEntity;
import ru.hits.authservice.repository.StudentGroupRepository;

import java.util.List;
import java.util.UUID;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
@Slf4j
public class StudentGroupService {

    private final StudentGroupRepository studentGroupRepository;

    @Transactional
    public void createStudentGroup(Integer number) {
        StudentGroupEntity studentGroup = StudentGroupEntity.builder()
                .number(number)
                .build();
        studentGroupRepository.save(studentGroup);
    }

    public List<StudentGroupInfoDto> getStudentGroups() {
        List<StudentGroupEntity> studentGroups = studentGroupRepository.findAll();
        return studentGroups.stream()
                .map(StudentGroupInfoDto::new)
                .collect(Collectors.toList());
    }

}
