CREATE TABLE IF NOT EXISTS public.users
(
    user_id BIGSERIAL PRIMARY KEY,

    email VARCHAR(200) NOT NULL,

    password_hash TEXT NOT NULL,

    full_name varchar(250) NULL,	

    created_at TIMESTAMP WITHOUT TIME ZONE,

    updated_at TIMESTAMP WITHOUT TIME ZONE
        NOT NULL DEFAULT CURRENT_TIMESTAMP
);
--select * from users
--truncate table users