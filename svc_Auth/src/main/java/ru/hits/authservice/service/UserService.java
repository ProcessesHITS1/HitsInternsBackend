package ru.hits.authservice.service;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import ru.hits.authservice.dto.*;
import ru.hits.authservice.entity.UserEntity;
import ru.hits.authservice.exception.NotFoundException;
import ru.hits.authservice.exception.UnauthorizedException;
import ru.hits.authservice.repository.UserRepository;
import ru.hits.authservice.security.JWTUtil;
import ru.hits.authservice.security.JwtUserData;

import java.util.*;

@Service
@RequiredArgsConstructor
@Slf4j
public class UserService {

    private final UserRepository userRepository;

    private final BCryptPasswordEncoder bCryptPasswordEncoder;

    private final JWTUtil jwtUtil;

    public AccessTokenDto signIn(UserSignInDto userSignInDto) {
        Optional<UserEntity> user = userRepository.findByEmail(userSignInDto.getEmail());

        if (user.isEmpty() ||
                !bCryptPasswordEncoder.matches(userSignInDto.getPassword(), user.get().getPassword())) {
            String message = "Некорректные данные.";
            log.error(message);
            throw new UnauthorizedException(message);
        }

        List<String> userRoles = new ArrayList<>();

        if (user.get().getIsStudent()) {
            userRoles.add("ROLE_STUDENT");
        }

        if (user.get().getIsSchoolRepresentative()) {
            userRoles.add("ROLE_SCHOOL_REPRESENTATIVE");
        }

        if (user.get().getIsAdmin()) {
            userRoles.add("ROLE_ADMIN");
        }

        AccessTokenDto accessTokenDto = new AccessTokenDto(
                jwtUtil.generateToken(
                        user.get().getId(),
                        user.get().getLastName() + user.get().getFirstName() + user.get().getPatronymic(),
                        user.get().getEmail(),
                        user.get().getPhone(),
                        userRoles
                )
        );

        return accessTokenDto;
    }

    @Transactional
    public void createUser(CreateUserDto createUserDto) {
        UserEntity userEntity = UserEntity.builder()
                .firstName(createUserDto.getFirstName())
                .lastName(createUserDto.getLastName())
                .patronymic(createUserDto.getPatronymic())
                .email(createUserDto.getEmail())
                .phone(createUserDto.getPhone())
                .password(bCryptPasswordEncoder.encode(createUserDto.getPassword()))
                .sex(createUserDto.getSex())
                .isStudent(createUserDto.getIsStudent())
                .isSchoolRepresentative(createUserDto.getIsSchoolRepresentative())
                .isAdmin(createUserDto.getIsAdmin())
                .build();
        userRepository.save(userEntity);
    }

    public UserInfoDto getUserInfo(UUID id) {
        UserEntity user = userRepository.findById(id)
                .orElseThrow(() -> new NotFoundException("Пользователя с ID " + id + " не существует"));

        return new UserInfoDto(user);
    }

    public UserInfoDto getAuthenticatedUserInfo() {
        UUID authenticatedUserId = getAuthenticatedUserId();

        UserEntity user = userRepository.findById(authenticatedUserId)
                .orElseThrow(() -> new NotFoundException("Пользователя с ID " + authenticatedUserId + " не существует"));

        return new UserInfoDto(user);
    }

    public List<UserInfoDto> getAllUsers() {
        List<UserEntity> users = userRepository.findAll();
        List<UserInfoDto> userInfoDtoList = new ArrayList<>();
        for (UserEntity user : users) {
            userInfoDtoList.add(new UserInfoDto(user));
        }
        return userInfoDtoList;
    }

    @Transactional
    public void changePassword(ChangePasswordDto changePasswordDto) {
        UUID authenticatedUserId = getAuthenticatedUserId();

        UserEntity user = userRepository.findById(authenticatedUserId)
                .orElseThrow(() -> new NotFoundException("Пользователя с ID " + authenticatedUserId + " не существует"));

        user.setPassword(bCryptPasswordEncoder.encode(changePasswordDto.getPassword()));
        userRepository.save(user);
    }

    @Transactional
    public UserInfoDto editUserInfo(EditUserInfoDto editUserInfoDto) {
        UUID authenticatedUserId = getAuthenticatedUserId();

        UserEntity user = userRepository.findById(authenticatedUserId)
                .orElseThrow(() -> new NotFoundException("Пользователя с ID " + authenticatedUserId + " не существует"));

        if (editUserInfoDto.getFirstName() != null) {
            user.setFirstName(editUserInfoDto.getFirstName());
        }

        if (editUserInfoDto.getLastName() != null) {
            user.setLastName(editUserInfoDto.getLastName());
        }

        if (editUserInfoDto.getPatronymic() != null) {
            user.setPatronymic(editUserInfoDto.getPatronymic());
        }

        if (editUserInfoDto.getEmail() != null) {
            user.setEmail(editUserInfoDto.getEmail());
        }

        if (editUserInfoDto.getPhone() != null) {
            user.setPhone(editUserInfoDto.getPhone());
        }

        user = userRepository.save(user);

        return new UserInfoDto(user);
    }

    private UUID getAuthenticatedUserId() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        JwtUserData userData = (JwtUserData) authentication.getPrincipal();
        return userData.getId();
    }

}
