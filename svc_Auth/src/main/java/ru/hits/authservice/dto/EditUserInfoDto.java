package ru.hits.authservice.dto;

import lombok.*;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class EditUserInfoDto {

    private String firstName;

    private String lastName;

    private String patronymic;

    private String email;

    private String phone;

}
