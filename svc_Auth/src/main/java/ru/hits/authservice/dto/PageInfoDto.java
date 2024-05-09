package ru.hits.authservice.dto;

import lombok.*;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class PageInfoDto {

    private Integer total;

    private Integer pageNumber;

    private Integer pageSize;

}
