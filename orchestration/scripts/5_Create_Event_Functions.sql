CREATE OR REPLACE FUNCTION "Events".create_event(
  p_name        VARCHAR(1024),
  p_description TEXT,
  p_starts_at   TIMESTAMPTZ,
  p_img_url     VARCHAR(1024)
)
RETURNS INTEGER
LANGUAGE plpgsql
AS $$
DECLARE
  v_id INTEGER;
BEGIN
  INSERT INTO "Events"."Event" (
    "Name",
    "Description",
    "StartsAt",
    "ImgUrl"
  )
  VALUES (
    p_name,
    p_description,
    p_starts_at,
    p_img_url
  )
  RETURNING "Id" INTO v_id;

  RETURN v_id;
END;
$$;

CREATE OR REPLACE FUNCTION "Events".get_event_by_id(
    p_id INTEGER
)
RETURNS TABLE (
    "Id" INTEGER,
    "Name" VARCHAR,
    "Description" TEXT,
    "StartsAt" TIMESTAMPTZ,
    "CreatedOn" TIMESTAMPTZ,
    "ImgUrl" VARCHAR,
    "IsDeleted" BOOLEAN
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT e."Id",
           e."Name",
           e."Description",
           e."StartsAt",
           e."CreatedOn",
           e."ImgUrl",
           e."IsDeleted"
    FROM "Events"."Event" AS e
    WHERE e."Id" = p_id
      AND e."IsDeleted" = FALSE;
END;
$$;

CREATE OR REPLACE FUNCTION "Events".list_events()
RETURNS TABLE (
    "Id" INTEGER,
    "Name" VARCHAR,
    "Description" TEXT,
    "StartsAt" TIMESTAMPTZ,
    "CreatedOn" TIMESTAMPTZ,
    "ImgUrl" VARCHAR,
    "IsDeleted" BOOLEAN
)
LANGUAGE sql
AS $$
    SELECT
        e."Id",
        e."Name",
        e."Description",
        e."StartsAt",
        e."CreatedOn",
        e."ImgUrl",
        e."IsDeleted"
    FROM "Events"."Event" e
    WHERE e."IsDeleted" = FALSE
    ORDER BY e."StartsAt";
$$;

CREATE OR REPLACE FUNCTION "Events".delete_event(
  p_id INTEGER
)
RETURNS BOOLEAN
LANGUAGE plpgsql
AS $$
BEGIN
  UPDATE "Events"."Event"
  SET "IsDeleted" = TRUE
  WHERE "Id" = p_id
    AND "IsDeleted" = FALSE;

  RETURN FOUND;
END;
$$;