CREATE INDEX idx_event_isdeleted
ON "Events"."Event" ("IsDeleted");

CREATE INDEX idx_reservation_eventid
ON "Reservations"."Reservation" ("EventId");

CREATE INDEX idx_reservation_isdeleted
ON "Reservations"."Reservation" ("IsDeleted");