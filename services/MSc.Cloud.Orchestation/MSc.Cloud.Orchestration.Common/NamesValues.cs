namespace MSc.Cloud.Orchestration.Common;

public class NamesValues
{
    public class EnvironmentVariables
    {
        public const string PostgresConnectionString = "POSTGRES_CONN_STR";
    }

    public class Queries
    {
        public class Reservations
        {
            public const string GetReservations = "SELECT * FROM \"Reservations\".\"Reservation\"";
        }

        public class Events
        {
            public const string GetEventsNotDeleted = "SELECT * FROM \"Events\".\"Event\" WHERE \"IsDeleted\" = FALSE";
        }
    }
}
