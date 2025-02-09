using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyphersSupporterBot
{
    public enum URLType
    {
        None = -1,
        GetPlayerData = 0,
        GetPlayerDetailData = 1,
        GetPlayerMatchingHistory,
        GetPlayerMatchData,
        GetPlayerRankingData,
        GetCharacterRanking,
        GetAllCharacterData,
    }

    internal static class CyphersAPI
    {
        private static HttpClient client = new HttpClient();

        public static async Task<string> Get(string url)
        {
            try
            {
                return await client.GetStringAsync(url);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);

                return string.Empty;
            }
        }

        public static async Task<string> RequestData(URLType urlType, params string[] parameters)
        {
            var command = MakeCommand(urlType, parameters);
            if (command == null)
                return string.Empty;

            return await command.Execute();
        }

        private static CyphersAPICommand MakeCommand(URLType urlType, IEnumerable<string> commandParameters)
        {
            switch (urlType)
            {
                case URLType.GetPlayerData:
                    return new GetPlayerDataCommand(commandParameters);

                case URLType.GetPlayerDetailData:
                    return new GetPlayerDetailDataCommand(commandParameters);

                case URLType.GetPlayerMatchingHistory:
                    return new GetPlayerMatchingHistoryCommand(commandParameters);

                case URLType.GetPlayerMatchData:
                    return new GetPlayerMatchDataCommand(commandParameters);

                case URLType.GetPlayerRankingData:
                    return new GetPlayerRankingDataCommand(commandParameters);

                case URLType.GetCharacterRanking:
                    return new GetCharacterRankingDataCommand(commandParameters);

                case URLType.GetAllCharacterData:
                    return new GetAllCharacterDataCommand(commandParameters);
            }

            return null;
        }
    }
}
