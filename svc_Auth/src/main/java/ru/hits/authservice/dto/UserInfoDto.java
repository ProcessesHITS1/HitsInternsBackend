package ru.hits.authservice.dto;

import lombok.*;
import ru.hits.authservice.entity.StudentGroupEntity;
import ru.hits.authservice.entity.UserEntity;
import ru.hits.authservice.enumeration.Sex;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class UserInfoDto {

    private UUID id;

    private String firstName;

    private String lastName;

    private String patronymic;

    private String email;

    private String phone;

    private Sex sex;

    private StudentGroupInfoDto group;

    private List<String> roles;

    public UserInfoDto(UserEntity user) {
        this.id = user.getId();
        this.firstName = user.getFirstName();
        this.lastName = user.getLastName();
        this.patronymic = user.getPatronymic();
        this.email = user.getEmail();
        this.phone = user.getPhone();
        this.sex = user.getSex();
        this.group = user.getGroup() != null ? new StudentGroupInfoDto(user.getGroup()) : null;
        this.roles = new ArrayList<>();

        if (user.getIsStudent()) {
            this.roles.add("ROLE_STUDENT");
        }

        if (user.getIsSchoolRepresentative()) {
            this.roles.add("ROLE_SCHOOL_REPRESENTATIVE");
        }

        if (user.getIsAdmin()) {
            this.roles.add("ROLE_ADMIN");
        }
    }

}
