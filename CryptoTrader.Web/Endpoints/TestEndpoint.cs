using FastEndpoints;

namespace CryptoTrader.Web.Endpoints
{
    public class TestRequest
    {
    }
    public class TestResponse
    {

    }

    public class TestEndpoint : Endpoint<TestRequest, TestResponse>
    {
        public override void Configure()
        {
            Get("/api/test");
        }

        public override async Task HandleAsync(TestRequest req, CancellationToken ct)
        {
            await SendAsync(new TestResponse());
        }
    }
}
