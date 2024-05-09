create table public.student_group
(
    id                       uuid not null
        primary key,
    number                   integer unique
);

alter table public.student_group
    owner to postgres;

alter table public._user
    add column group_id UUID;

alter table public._user
    add constraint fk_group_id
        foreign key (group_id) references public.student_group(id);