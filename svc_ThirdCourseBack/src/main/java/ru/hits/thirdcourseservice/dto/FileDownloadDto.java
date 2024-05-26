package ru.hits.thirdcourseservice.dto;

import lombok.*;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class FileDownloadDto {

    /**
     * Байтовый массив содержащий содержимое файла.
     */
    private byte[] in;

    /**
     * Имя файла.
     */
    private String filename;

}
