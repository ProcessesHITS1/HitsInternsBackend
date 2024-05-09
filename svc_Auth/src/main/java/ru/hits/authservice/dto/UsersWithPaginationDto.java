package ru.hits.authservice.dto;

import lombok.*;

import java.util.List;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class UsersWithPaginationDto {

    private PageInfoDto pageInfo;

    private List<UserInfoDto> data;

}
