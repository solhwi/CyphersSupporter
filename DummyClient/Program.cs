namespace DummyClient
{
    internal class Program
    {
        private static string serverBaseUrl = "http://localhost:8002";
        private static HttpClient client = new HttpClient();

        private static List<string> testCaseList = new List<string>()
        {
            "/Cy?p1=그님티&p2=냉정침착빵훈맨",
            "/Cy?p1=그님티&p2=믽지",
            "/Cy?p1=그님티&p2=뭅",
            "/Cy?p1=그님티&p2=소리쿤",
            "/Cy?p1=그님티&p2=장뚱어",
            "/Cy?p1=그님티&p2=시흥공기달다",
            "/Cy?p1=그님티&p2=루스77",
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
