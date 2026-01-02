-- CREATE TABLE IF NOT EXISTS public.workitems
-- (
    -- id BIGSERIAL PRIMARY KEY,

    -- title VARCHAR(200) NOT NULL,

    -- description VARCHAR(1000),

    -- is_completed BOOLEAN NOT NULL DEFAULT FALSE,

    -- created_at TIMESTAMP WITHOUT TIME ZONE,

    -- updated_at TIMESTAMP WITHOUT TIME ZONE
        -- NOT NULL DEFAULT CURRENT_TIMESTAMP
-- );

-- ALTER TABLE public.workitems
-- ADD COLUMN user_id INT;

-- ALTER TABLE public.workitems
-- ADD CONSTRAINT fk_workitems_user
-- FOREIGN KEY (user_id)
-- REFERENCES public.users(user_id)
-- ON DELETE CASCADE;


-- CREATE INDEX idx_workitems_userid
-- ON public.workitems(user_id);
