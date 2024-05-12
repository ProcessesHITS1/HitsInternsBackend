package ru.hits.companymanagementservice.dto;

import lombok.*;
import ru.hits.companymanagementservice.enumeration.Sex;

import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class CreateUserDto {

    private String firstName;

    private String lastName;

    private String patronymic;

    private String email;

    private String phone;

    private String password;

    private Sex sex;

    private UUID groupId;

    private Boolean isStudent;

    private Boolean isSchoolRepresentative;

    private Boolean isAdmin;

}
