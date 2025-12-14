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