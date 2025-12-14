DROP TABLE IF EXISTS "Reservations"."Reservation";
DROP SEQUENCE IF EXISTS "Reservations"."Reservation_Id_seq";

CREATE SEQUENCE "Reservations"."Reservation_Id_seq"
    INCREMENT 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1;

CREATE TABLE "Reservations"."Reservation" (
    "Id" integer DEFAULT nextval('"Reservations"."Reservation_Id_seq"') NOT NULL,
    "EventId" integer NOT NULL,
    "FullName" character varying(1024) NOT NULL,
    "NumTickets" integer NOT NULL,
    "CreatedOn" timestamptz DEFAULT now() NOT NULL,
    "IsDeleted" boolean DEFAULT false NOT NULL,
    CONSTRAINT "Reservation_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "Reservation_Event_fkey"
        FOREIGN KEY ("EventId")
        REFERENCES "Events"."Event" ("Id")
);