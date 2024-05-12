create table public.company
(
    id         uuid not null
        primary key,
    curator_id uuid,
    name       varchar(255)
);

alter table public.company
    owner to postgres;

create table public.company_contact
(
    id         uuid not null
        primary key,
    company_id uuid,
    contact    varchar(255)
);

alter table public.company_contact
    owner to postgres;

create table public.company_season
(
    id         uuid not null
        primary key,
    company_id uuid,
    season_id  uuid
);

alter table public.company_season
    owner to postgres;

