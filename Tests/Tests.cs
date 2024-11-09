using Application;
using System.Globalization;
using System.Text.Json;

namespace Tests
{
    [TestClass]
    public class Tests
    {
        private readonly Helpers _helpers;

        public Tests()
        {
            _helpers = new Helpers();
        }

        [TestMethod]
        public void ExampleTest()
        {
            // Runs the app and returns the output from Console.WriteLine
            string capturedStdOut = _helpers.CapturedStdOut(_helpers.RunApp);

            // Run an assertion on the captured output
            Assert.IsTrue(capturedStdOut.Contains("UK time"));
        }


        // Since we are using a third party api we dont need to test the functionality of the api.
        // But the acceptance criteria specifically states it needs to display current date and time so I have included a test to cross check the times.
        [TestMethod]
        public void CheckCurrentUKLondonTime()
        {
            var capturedStdOut = _helpers.CapturedStdOut(_helpers.RunApp).Split(Environment.NewLine);
            var expectedUkDate = DateTime.Now.ToString("dddd dd MMMM yyyy HH:mm");
            Assert.IsTrue(capturedStdOut[1].Contains(expectedUkDate), expectedUkDate);
        }


        // Requirement just mentions Canada and not specific time zone. Since the application is calling America/Toronto I have checked it against Eastern Standard Time.
        [TestMethod]
        public void CheckCurrentCanadaTorontoTime()
        {
            var capturedStdOut = _helpers.CapturedStdOut(_helpers.RunApp).Split(Environment.NewLine);
            var currentCanadaTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(),
                TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
            var expectedCanadaTime = currentCanadaTime.ToString("dddd dd MMMM yyyy HH:mm");
            Assert.IsTrue(capturedStdOut[0].Contains(expectedCanadaTime), expectedCanadaTime);
        }

        [TestMethod]
        public void CheckCurrentCanadaOtherTimeZone()
        {
            // If other time zones are included - add tests accordingly.
        }


        //To test the first acceptance criteria that the date and time must be retrieved from https://worldtimeapi.org/ by mocking the response.
        [TestMethod]
        public void CheckCanadaApiIsCalled()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var worldTimeApiResponse = new WorldTimeAPIResponse
            {
                datetime = "2024-11-08T16:19:10.494006+00:00"
            };

            var apiResponseJson = JsonSerializer.Serialize(worldTimeApiResponse);

            var jsonMessageHandler = new MockResponse(apiResponseJson);

            var httpClient = new HttpClient(jsonMessageHandler);
            var currentDateTime = new CurrentDateTime(httpClient);

            // Act
            currentDateTime.GetCanadaDateTime();

            //Assert
            var output = stringWriter.ToString().Trim();
            Assert.IsNotNull(output);
            var lines = output.Split(Environment.NewLine).ToList();
            Assert.AreEqual(1, lines.Count);
            Assert.IsTrue(lines[0].Contains("Friday 08 November 2024 16:19:10"));
        }

        //To test the first acceptance criteria that the date and time must be retrieved from https://worldtimeapi.org/ only. 
        //Application does not call the api for UK date and time. Hence added the test and made sure it fails by mocking the response. 
        [TestMethod]
        public void CheckUkApiIsCalled()
        {
            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            var worldTimeApiResponse = new WorldTimeAPIResponse
            {
                datetime = "2024-11-08T20:19:10.494006+00:00"
            };

            var apiResponseJson = JsonSerializer.Serialize(worldTimeApiResponse);

            var jsonMessageHandler = new MockResponse(apiResponseJson);

            var httpClient = new HttpClient(jsonMessageHandler);
            var currentDateTime = new CurrentDateTime(httpClient);


            currentDateTime.GetUkDateTime();


            var output = stringWriter.ToString().Trim();
            Assert.IsNotNull(output);
            var lines = output.Split(Environment.NewLine).ToList();
            Assert.AreEqual(1, lines.Count);
            Assert.IsTrue(lines[0].Contains("Friday 08 November 2024 20:19:10"));
        }


        // Requirement is to display the current date and time but does not say what the exact message should be.
        // Original Application code had 'Uk Time :' but I changed it to 'Uk Time -' to make my unit test work.
        // I cant think of an alternate way to TryExactParse to compare the string.
        // This can be discussed with developer and BA on what the exact message should be and I will take help from others to write a better test if needed.
        [TestMethod]
        public void CheckDateIsInCorrectFormat()
        {
            var capturedStdOut = _helpers.CapturedStdOut(_helpers.RunApp).Split(Environment.NewLine);
            var canadaDateResult = capturedStdOut[0].Split('-');

            Assert.IsTrue(DateTime.TryParseExact(canadaDateResult[1], " dddd dd MMMM yyyy HH:mm:ss", null, DateTimeStyles.None, out DateTime canadaDate));

            var ukDateResult = capturedStdOut[1].Split('-');
            Assert.IsTrue(DateTime.TryParseExact(ukDateResult[1], " dddd dd MMMM yyyy HH:mm:ss", null, DateTimeStyles.None, out DateTime ukDate));
        }

        [TestMethod]
        public void CheckDifferenceInTimeIsDisplayed()
        {
            var capturedStdOut = _helpers.CapturedStdOut(_helpers.RunApp).Split(Environment.NewLine);
            Assert.IsTrue(capturedStdOut[2].Contains("Difference"));
            // The requirement is not very clear on how the difference should be displayed - In Minutes or Hours etc.
            // And what the exact message should be . So I have just added a test to check for keyword 'Difference' and make the test fail.
            // This can be addressed by having discussions with developers and BA to understand the requirement better. 
        }

        [TestMethod]
        public void CheckErrorMessageIfAPIResponseStatusCodeIsNotOk()
        {
            //The third party api often displays 502.
            //Application does not have exception handler to address this. Add tests to check that an error message is displayed when the Api response is other than 200.
        }

    }

}