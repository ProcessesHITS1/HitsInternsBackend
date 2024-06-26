package ru.hits.thirdcourseservice.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import lombok.SneakyThrows;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;
import ru.hits.thirdcourseservice.dto.FileDownloadDto;
import ru.hits.thirdcourseservice.service.FileService;

import java.util.UUID;

@RestController
@RequestMapping("/api/files")
@RequiredArgsConstructor
@Slf4j
@Tag(name = "Работа с файлами.")
public class FileController {

    /**
     * Контроллер, обрабатывающий запросы, связанные с файлами.
     */
    private final FileService fileService;

    /**
     * Метод для загрузки файла на сервер.
     *
     * @param file загружаемый файл.
     * @return идентификатор загруженного файла.
     */
    @SneakyThrows
    @Operation(summary = "Загрузка файла.")
    @PostMapping(value = "/upload", consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    public String upload(@RequestParam("file") MultipartFile file) {
        log.info("Запрос на загрузку файла: {}", file.getOriginalFilename());
        return fileService.upload(file);
    }

    /**
     * Метод для получения загруженного файла.
     *
     * @param id идентификатор файла.
     * @return файл.
     */
    @Operation(summary = "Получение файла по id.")
    @GetMapping(value = "/download/{id}", produces = MediaType.APPLICATION_OCTET_STREAM_VALUE)
    public ResponseEntity<byte[]> download(@PathVariable("id") UUID id) {
        log.info("Запрос на скачивание файла с ID: {}", id);
        FileDownloadDto fileDownloadDto = fileService.download(id);
        log.info("Файл успешно загружен: {}", fileDownloadDto.getFilename());
        return ResponseEntity.ok()
                .header("Content-Type", MediaType.APPLICATION_OCTET_STREAM_VALUE)
                .header("Access-Control-Expose-Headers", "Content-Disposition")
                .header("Content-Disposition", "attachment; filename=\"" + fileDownloadDto.getFilename() + "\"")
                .body(fileDownloadDto.getIn());
    }

}
