package ru.hits.authservice.service;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.mail.SimpleMailMessage;
import org.springframework.mail.javamail.JavaMailSender;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import ru.hits.authservice.dto.*;
import ru.hits.authservice.entity.UserEntity;
import ru.hits.authservice.exception.ConflictException;
import ru.hits.authservice.exception.NotFoundException;
import ru.hits.authservice.exception.UnauthorizedException;
import ru.hits.authservice.helpingservices.CheckPaginationInfoService;
import ru.hits.authservice.repository.StudentGroupRepository;
import ru.hits.authservice.repository.UserRepository;
import ru.hits.authservice.security.JWTUtil;
import ru.hits.authservice.security.JwtUserData;

import java.util.*;
import java.util.stream.Collectors;

@Service
@RequiredArgsConstructor
@Slf4j
public class UserService {

    private final UserRepository userRepository;

    private final StudentGroupRepository studentGroupRepository;

    private final BCryptPasswordEncoder bCryptPasswordEncoder;

    private final CheckPaginationInfoService checkPaginationInfoService;

    private final JWTUtil jwtUtil;

    private final JavaMailSender javaMailSender;

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
        if (userRepository.findByEmail(createUserDto.getEmail()).isPresent()) {
            throw new ConflictException("Пользователь с таким email уже существует.");
        }

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
                .group(createUserDto.getGroupId() != null ? studentGroupRepository.findById(createUserDto.getGroupId()).get() : null)
                .build();
        userRepository.save(userEntity);

        SimpleMailMessage message = new SimpleMailMessage();
        message.setFrom("hitsemail681@gmail.com");
        message.setTo(createUserDto.getEmail());
        message.setSubject("HITS Interns");
        message.setText("Добро пожаловать в нашу систему!\n\n"
                + "Ваш логин: " + createUserDto.getEmail() + "\n"
                + "Ваш пароль: " + createUserDto.getPassword() + "\n\n");
        javaMailSender.send(message);
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

    public UsersWithPaginationDto getAllUsers(int page, int size) {
        checkPaginationInfoService.checkPagination(page, size);
        Pageable pageable = PageRequest.of(page - 1, size);
        Page<UserEntity> usersPage = userRepository.findAll(pageable);
        PageInfoDto pageInfoDto = new PageInfoDto(
                (int) usersPage.getTotalElements(),
                page,
                Math.min(size, usersPage.getContent().size())
        );
        List<UserInfoDto> userInfoDtoList = usersPage.getContent().stream()
                .map(UserInfoDto::new)
                .collect(Collectors.toList());
        return new UsersWithPaginationDto(pageInfoDto, userInfoDtoList);
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
