CREATE INDEX idx_event_isdeleted
ON "Event" ("IsDeleted");

CREATE INDEX idx_reservation_eventid
ON "Reservation" ("EventId");

CREATE INDEX idx_reservation_isdeleted
ON "Reservation" ("IsDeleted");