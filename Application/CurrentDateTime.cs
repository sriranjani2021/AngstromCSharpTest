using System.Globalization;
using System.Net.Http.Json;

namespace Application
{
    public class CurrentDateTime
    {
        private readonly HttpClient httpClient;
        private DateTimeOffset formattedCanadaDateTime;
        private DateTimeOffset ukDateTime;

        public CurrentDateTime(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public void GetCanadaDateTime()
        {
            var canadaDateResponse = httpClient.GetFromJsonAsync<WorldTimeAPIResponse>("http://worldtimeapi.org/api/timezone/America/Toronto").Result;
            formattedCanadaDateTime = DateTimeOffset.ParseExact(canadaDateResponse.datetime, "yyyy-MM-dd'T'HH:mm:ss.FFFFFFzzz", CultureInfo.InvariantCulture);

            var canadaCurrentDateTime = FormatDate(formattedCanadaDateTime);
            WriteOutputToConsole(canadaCurrentDateTime, "Canada");
        }

        public void GetUkDateTime()
        {
            ukDateTime = DateTime.Now;
            var ukFormattedDate = FormatDate(ukDateTime);
            WriteOutputToConsole(ukFormattedDate, "UK");

        }

        public static string FormatDate(DateTimeOffset date)
        {
            var dateTimeFormatter = "dddd dd MMMM yyyy HH:mm:ss";
            return date.ToString(dateTimeFormatter);
        }

        public static void WriteOutputToConsole(string output, string country)
        {
            Console.WriteLine($"{country} time- {output}");
        }

        public void CalculateDifferenceInTime()
        {
            if (ukDateTime > formattedCanadaDateTime)
            {
                Console.WriteLine($"You are {Math.Round(ukDateTime.Subtract(formattedCanadaDateTime.DateTime).TotalMinutes, 0)}m ahead of Canada");
            }
            else
            {
                Console.WriteLine($"Canada is {Math.Round(formattedCanadaDateTime.Subtract(ukDateTime).TotalMinutes, 0)}m ahead of you");
            }
        }

    }
}

