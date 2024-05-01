package ru.hits.authservice.dto;

import lombok.*;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class ChangePasswordDto {

    private String password;

}
