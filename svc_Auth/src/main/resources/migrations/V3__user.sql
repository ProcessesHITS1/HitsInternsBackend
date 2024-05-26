create table public._user
(
    id                       uuid not null
        primary key,
    email                    varchar(255),
    first_name               varchar(255),
    is_admin                 boolean,
    is_school_representative boolean,
    is_student               boolean,
    last_name                varchar(255),
    password                 varchar(255),
    patronymic               varchar(255),
    phone                    varchar(255),
    sex                      integer
);

alter table public._user
    owner to postgres;

insert into public._user (id, email, first_name, is_admin, is_school_representative, is_student, last_name, password, patronymic, phone, sex)
values ('c29ae872-7c11-4ce7-8273-49d103fc1b82', 'admin@gmail.com', 'Admin', true, true, false, 'Admin', '$2a$10$Ep1M5kTespdRqih7oeTuaeBxHuY0zGgAqCZsJIeF8U68vCXVpVvAC', 'Admin', '+79999999999', 0);