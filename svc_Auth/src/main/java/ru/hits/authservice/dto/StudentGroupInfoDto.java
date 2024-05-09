package ru.hits.authservice.dto;

import lombok.*;
import ru.hits.authservice.entity.StudentGroupEntity;
import ru.hits.authservice.entity.UserEntity;

import java.util.ArrayList;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class StudentGroupInfoDto {

    private UUID id;

    private Integer number;

    public StudentGroupInfoDto(StudentGroupEntity group) {
        this.id = group.getId();
        this.number = group.getNumber();
    }

}
