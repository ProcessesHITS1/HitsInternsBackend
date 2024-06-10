CREATE TABLE public.mark_requirement (
                                         id UUID PRIMARY KEY,
                                         description VARCHAR(255)
);

alter table public.mark_requirement
    owner to postgres;

CREATE TABLE public.mark (
                             id UUID PRIMARY KEY,
                             value INT,
                             student_id UUID REFERENCES public.student_in_semester,
                             mark_requirement_id UUID REFERENCES public.mark_requirement
);

alter table public.mark
    owner to postgres;