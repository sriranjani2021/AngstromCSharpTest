using System.Net;
using System.Text;

namespace Application
{
    public class MockResponse : HttpMessageHandler

    {

        private readonly string json;
        public MockResponse(string json)
        {
            this.json = json;
        }


        sealed protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json, Encoding.UTF8, "application/json"),
            };

            return Task.FromResult(response);
        }
    }
}
