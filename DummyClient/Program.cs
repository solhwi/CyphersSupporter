namespace DummyClient
{
    internal class Program
    {
        private static string serverBaseUrl = "http://localhost:8002";
        private static HttpClient client = new HttpClient();

        private static List<string> testCaseList = new List<string>()
        {
            "/Cy?p1=그님티&p2=뭅",
            "/Cy?p1=공식전적&p2=뭅",
            "/Cy?p1=일반전적&p2=뭅",
            "/Cy?p1=캐릭터&p2=뭅"
        };

        public static async Task Main(string[] args)
        {
            foreach (var testCase in testCaseList)
            {
                var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, serverBaseUrl + testCase));
                string message = await response.Content.ReadAsStringAsync();
                Console.WriteLine(message);
            }
        }
    }
}
