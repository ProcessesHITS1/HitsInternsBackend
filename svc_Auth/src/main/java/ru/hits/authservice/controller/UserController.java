package ru.hits.authservice.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import ru.hits.authservice.dto.*;
import ru.hits.authservice.service.UserService;

import javax.validation.Valid;
import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/users")
@RequiredArgsConstructor
@Slf4j
@Tag(name = "Пользователи.")
public class UserController {

    private final UserService userService;

    @Operation(
            summary = "Вход в систему."
    )
    @PostMapping("/sign-in")
    public ResponseEntity<AccessTokenDto> signIn(@RequestBody @Valid UserSignInDto userSignInDto) {
        return new ResponseEntity<>(userService.signIn(userSignInDto), HttpStatus.OK);
    }

    @Operation(
            summary = "Получить пользователей.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping
    public ResponseEntity<UsersWithPaginationDto> getAllUsers(
            @RequestParam(defaultValue = "1") int page,
            @RequestParam(defaultValue = "10") int size) {
        UsersWithPaginationDto usersWithPaginationDto = userService.getAllUsers(page, size);
        return new ResponseEntity<>(usersWithPaginationDto, HttpStatus.OK);
    }

    @Operation(
            summary = "Создать пользователя.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PostMapping("/create")
    public ResponseEntity<Void> createUser(@RequestBody @Valid CreateUserDto createUserDto) {
        userService.createUser(createUserDto);
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @Operation(
            summary = "Получить информацию о конкретном пользователе.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping("/{id}/info")
    public ResponseEntity<UserInfoDto> getUserInfo(@PathVariable UUID id) {
        return new ResponseEntity<>(userService.getUserInfo(id), HttpStatus.OK);
    }

    @Operation(
            summary = "Получить информацию о себе.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @GetMapping("/info")
    public ResponseEntity<UserInfoDto> getAuthenticatedUserInfo() {
        return new ResponseEntity<>(userService.getAuthenticatedUserInfo(), HttpStatus.OK);
    }

    @Operation(
            summary = "Сменить пароль.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PutMapping("/profile/change-password")
    public ResponseEntity<Void> changePassword(@RequestBody @Valid ChangePasswordDto changePasswordDto) {
        userService.changePassword(changePasswordDto);
        return new ResponseEntity<>(HttpStatus.OK);
    }

//    @Operation(
//            summary = "Получить всех пользователей.",
//            security = @SecurityRequirement(name = "bearerAuth")
//    )
//    @GetMapping
//    public ResponseEntity<List<UserInfoDto>> getAllUsers() {
//        return new ResponseEntity<>(userService.getAllUsers(), HttpStatus.OK);
//    }

    @Operation(
            summary = "Редактировать данные пользователя.",
            security = @SecurityRequirement(name = "bearerAuth")
    )
    @PutMapping("/profile")
    public ResponseEntity<UserInfoDto> editUserInfo(@RequestBody @Valid EditUserInfoDto editUserInfoDto) {
        return new ResponseEntity<>(userService.editUserInfo(editUserInfoDto), HttpStatus.OK);
    }


}
