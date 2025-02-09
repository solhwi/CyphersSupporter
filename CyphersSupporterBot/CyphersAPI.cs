using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyphersSupporterBot
{
    public enum APIType
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
        private static Dictionary<string, string> characterIdDictionary = new Dictionary<string, string>();
    
        public static async Task PreLoad()
        {
            var characterData = await RequestData<CyphersAPICharacterData>(APIType.GetAllCharacterData);
			if (characterData == null)
                return;

            if (characterData.rows == null)
                return;

            characterIdDictionary.Clear();
            foreach (var row in characterData.rows)
            {
                characterIdDictionary.Add(row.characterName, row.characterId);
            }
        }

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

        public static async Task<T> RequestData<T>(APIType urlType, params string[] parameters) where T : CyphersAPIData
		{
			string jsonData = await RequestData(urlType, parameters);
			if (jsonData == null || jsonData == string.Empty)
				return null;

			return JsonConvert.DeserializeObject<T>(jsonData);
		}


		public static async Task<string> RequestData(APIType urlType, params string[] parameters)
        {
            var command = MakeCommand(urlType, parameters);
            if (command == null)
                return string.Empty;

            return await command.Execute();
        }

        private static CyphersAPICommand MakeCommand(APIType urlType, IEnumerable<string> commandParameters)
        {
            switch (urlType)
            {
                case APIType.GetPlayerData:
                    return new GetPlayerDataCommand(commandParameters);

                case APIType.GetPlayerDetailData:
                    return new GetPlayerDetailDataCommand(commandParameters);

                case APIType.GetPlayerMatchingHistory:
                    return new GetPlayerMatchingHistoryCommand(commandParameters);

                case APIType.GetPlayerMatchData:
                    return new GetPlayerMatchDataCommand(commandParameters);

                case APIType.GetPlayerRankingData:
                    return new GetPlayerRankingDataCommand(commandParameters);

                case APIType.GetCharacterRanking:
                    return new GetCharacterRankingDataCommand(commandParameters);

                case APIType.GetAllCharacterData:
                    return new GetAllCharacterDataCommand(commandParameters);
            }

            return null;
        }
    }
}
