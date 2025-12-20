CREATE OR REPLACE FUNCTION "Reservations".create_reservation(
  p_event_id    INTEGER,
  p_full_name   VARCHAR(1024),
  p_num_tickets INTEGER
)
RETURNS INTEGER
LANGUAGE plpgsql
AS $$
DECLARE
  v_id INTEGER;
BEGIN
  INSERT INTO "Reservations"."Reservation" (
    "EventId",
    "FullName",
    "NumTickets"
  )
  VALUES (
    p_event_id,
    p_full_name,
    p_num_tickets
  )
  RETURNING "Id" INTO v_id;

  RETURN v_id;
END;
$$;

CREATE OR REPLACE FUNCTION "Reservations".get_reservation_by_id(
  p_id INTEGER
)
RETURNS TABLE (
  "Id" INTEGER,
  "EventId" INTEGER,
  "FullName" VARCHAR,
  "NumTickets" INTEGER,
  "CreatedOn" TIMESTAMPTZ,
  "IsDeleted" BOOLEAN
)
LANGUAGE plpgsql
AS $$
BEGIN
  RETURN QUERY
  SELECT *
  FROM "Reservations"."Reservation"
  WHERE "Id" = p_id
    AND "IsDeleted" = FALSE;
END;
$$;

CREATE OR REPLACE FUNCTION "Reservations".list_reservations()
RETURNS TABLE (
  "ReservationId" INTEGER,
  "FullName" VARCHAR,
  "NumTickets" INTEGER,
  "CreatedOn" TIMESTAMPTZ,
  "EventId" INTEGER,
  "EventName" VARCHAR,
  "StartsAt" TIMESTAMPTZ
)
LANGUAGE plpgsql
AS $$
BEGIN
  RETURN QUERY
  SELECT
    r."Id",
    r."FullName",
    r."NumTickets",
    r."CreatedOn",
    e."Id",
    e."Name",
    e."StartsAt"
  FROM "Reservations"."Reservation" r
  JOIN "Events"."Event" e
    ON e."Id" = r."EventId"
  WHERE r."IsDeleted" = FALSE
    AND e."IsDeleted" = FALSE
  ORDER BY r."CreatedOn" DESC;
END;
$$;

CREATE OR REPLACE FUNCTION "Reservations".delete_reservation(
  p_id INTEGER
)
RETURNS BOOLEAN
LANGUAGE plpgsql
AS $$
BEGIN
  UPDATE "Reservations"."Reservation"
  SET "IsDeleted" = TRUE
  WHERE "Id" = p_id
    AND "IsDeleted" = FALSE;

  RETURN FOUND;
END;
$$;
