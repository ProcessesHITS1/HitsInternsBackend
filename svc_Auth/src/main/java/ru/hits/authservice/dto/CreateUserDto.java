package ru.hits.authservice.dto;

import lombok.*;
import ru.hits.authservice.enumeration.Sex;

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

    private Boolean isStudent;

    private Boolean isSchoolRepresentative;

    private Boolean isAdmin;

}
