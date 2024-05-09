package ru.hits.authservice.entity;

import lombok.*;
import org.hibernate.annotations.GenericGenerator;
import ru.hits.authservice.enumeration.Sex;

import javax.persistence.*;
import java.util.UUID;

@Entity
@NoArgsConstructor
@AllArgsConstructor
@Getter
@Setter
@Builder
@Table(name = "_user")
public class UserEntity {

    @Id
    @GeneratedValue(generator = "UUID")
    @GenericGenerator(
            name = "UUID",
            strategy = "org.hibernate.id.UUIDGenerator"
    )
    private UUID id;

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
