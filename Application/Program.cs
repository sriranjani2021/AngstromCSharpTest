using Application;

//using HttpClient client = new();

//var canadaDateResponse = client.GetFromJsonAsync<WorldTimeAPIResponse>("http://worldtimeapi.org/api/timezone/America/Toronto").Result;
//var canadaDateTime = DateTimeOffset.ParseExact(canadaDateResponse.datetime, "yyyy-MM-dd'T'HH:mm:ss.FFFFFFzzz", CultureInfo.InvariantCulture);

//var ukDateTime = DateTime.Now;

//var dateTimeFormatter = "dddd dd MMMM yyyy HH:mm:ss";

//Console.WriteLine($"UK Time: {ukDateTime.ToString(dateTimeFormatter)}");
//Console.WriteLine($"Canada time:  {canadaDateTime.ToString(dateTimeFormatter)}");

//if (ukDateTime > canadaDateTime)
//{
//    Console.WriteLine($"You are {Math.Round(ukDateTime.Subtract(canadaDateTime.DateTime).TotalMinutes, 0)}m ahead of Canada");
//} 
//else
//{
//    Console.WriteLine($"Canada is {Math.Round(canadaDateTime.Subtract(ukDateTime).TotalMinutes, 0)}m ahead of you");
//}

//public partial class Program { }

var httpClient = new HttpClient();
var currentDateTime = new CurrentDateTime(httpClient);

currentDateTime.GetCanadaDateTime();
currentDateTime.GetUkDateTime();
currentDateTime.CalculateDifferenceInTime();
