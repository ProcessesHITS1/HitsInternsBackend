package ru.hits.authservice.dto;

import lombok.*;

import javax.validation.constraints.NotBlank;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class UserSignInDto {

    @NotBlank(message = "Электронная почта не может быть пустой.")
    private String email;

    @NotBlank(message = "Пароль не может быть пустым.")
    private String password;

}
