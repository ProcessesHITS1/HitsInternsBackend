ALTER TABLE public.mark_requirement
    ADD COLUMN semester_id UUID,
ADD CONSTRAINT fk_mark_requirement_semester
    FOREIGN KEY (semester_id)
    REFERENCES public.semester (id);