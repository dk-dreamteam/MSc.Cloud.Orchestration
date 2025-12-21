namespace MSc.Cloud.Orchestration.Common;

public class NamesValues
{
    public class EnvironmentVariables
    {
        public static readonly string PostgresConnectionString = "POSTGRES_CONN_STR";
        public static readonly string SupabaseSendEmailFunctionUrl = "SUPABASE_SEND_EMAIL_FUNCTION_URL";
    }

    public class Queries
    {
        public class Reservations
        {
            internal const string CreateReservation = """
                SELECT "Reservations".create_reservation(
                    @EventId,
                    @FullName,
                    @NumTickets,
                    @EmailAddress
                );
                """;

            internal const string GetReservationById = """
                SELECT *
                FROM "Reservations".get_reservation_by_id(@Id);
                """;

            internal const string GetReservations = """
                SELECT *
                FROM "Reservations".list_reservations();
                """;

            internal const string DeleteReservation = """
                SELECT "Reservations".delete_reservation(@Id);
                """;
        }

        public class Events
        {
            internal const string CreateEvent = """
                SELECT "Events".create_event(
                    @Name,
                    @Description,
                    @StartsAt,
                    @ImgUrl
                );
                """;

            internal const string GetEventById = """
                SELECT *
                FROM "Events".get_event_by_id(@Id);
                """;

            internal const string GetEvents = """
                SELECT *
                FROM "Events".list_events();
                """;

            internal const string DeleteEvent = """
                SELECT "Events".delete_event(@Id);
                """;
        }
    }
}
