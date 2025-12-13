CREATE SCHEMA IF NOT EXISTS "Reservations";
CREATE SCHEMA IF NOT EXISTS "Events";

DROP TABLE IF EXISTS "Events"."Event";
DROP SEQUENCE IF EXISTS "Events"."Event_Id_seq";

CREATE SEQUENCE "Events"."Event_Id_seq"
    INCREMENT 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1;

CREATE TABLE "Events"."Event" (
    "Id" integer DEFAULT nextval('"Events"."Event_Id_seq"') NOT NULL,
    "Name" character varying(1024) NOT NULL,
    "Description" text,
    "StartsAt" timestamptz NOT NULL,
    "CreatedOn" timestamptz DEFAULT now() NOT NULL,
    "ImgUrl" character varying(1024),
    "IsDeleted" boolean DEFAULT false NOT NULL,
    CONSTRAINT "Event_pkey" PRIMARY KEY ("Id")
);

INSERT INTO "Events"."Event"
("Id", "Name", "Description", "StartsAt", "CreatedOn", "ImgUrl", "IsDeleted")
VALUES
(
  1,
  'Casino Royale in Concert',
  'Celebrating its 20th anniversary, Casino Royale is back on the big screen, accompanied by a full symphony orchestra.

Experience David Arnold''s thrilling score performed live by the Royal Philharmonic Concert Orchestra conducted by Anthony Gabriele.

Directed by Martin Campbell, Casino Royale is the 21st Bond film in the series, and the first starring Daniel Craig, with an international cast that includes Eva Green, Mads Mikkelsen, Jeffrey Wright and Dame Judi Dench.',
  '2026-12-30 13:00:00+00',
  '2025-12-13 19:30:31.909615+00',
  'https://d117kfg112vbe4.cloudfront.net/public/Royal-Albert-Hall-DAMS/Promoter-Imagery/2026/12-December/Casino-Royale-in-Concert/39939_csnor_stl_3_h-v3.jpg?type=image&id=6269&token=6d3688e5&mode=fill&width=1410&height=744&format=webp',
  false
);

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
