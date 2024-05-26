create table public.diary
(
    id                uuid not null
        primary key,
    attached_at       timestamp,
    document_id       uuid,
    acceptance_status varchar(255),
    comments          text
);

alter table public.diary
    owner to postgres;

create table public.semester
(
    id                 uuid    not null
        primary key,
    documents_deadline timestamp,
    season_id          uuid,
    semester           integer not null,
    year               integer
);

alter table public.semester
    owner to postgres;

create table public.student_in_semester
(
    id                uuid    not null
        primary key,
    company_id        uuid,
    internship_passed boolean not null,
    student_id        uuid,
    semester_id       uuid    not null
        constraint fkphemoslbofwujsww1hvt5q4bm
            references public.semester,
    diary_id          uuid
        constraint fkglynle1gt6svmlmpr57fxbht3
            references public.diary
);

alter table public.student_in_semester
    owner to postgres;

create table public.diary_feedback
(
    id                uuid not null
        primary key,
    acceptance_status varchar(255),
    comments          text,
    diary_id          uuid not null
        constraint fk6evnhcyqpk625qhq7ix5iv3m
            references public.diary
);

alter table public.diary_feedback
    owner to postgres;