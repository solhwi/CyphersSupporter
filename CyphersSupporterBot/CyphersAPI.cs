﻿using Newtonsoft.Json;
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
        public static Dictionary<string, string> characterIdDictionary = new Dictionary<string, string>();
        public static Dictionary<string, string> characterNameDictionary = new Dictionary<string, string>();

        public static async Task PreLoad()
        {
            var characterData = await RequestData<CyphersAPICharacterData>(URLType.GetAllCharacterData);
			if (characterData == null)
                return;

            if (characterData.rows == null)
                return;

            characterIdDictionary.Clear();
            characterNameDictionary.Clear();
            foreach (var row in characterData.rows)
            {
                characterIdDictionary.Add(row.characterName, row.characterId);
                characterNameDictionary.Add(row.characterId, row.characterName);
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

        public static async Task<T> RequestData<T>(URLType urlType, params string[] parameters) where T : CyphersAPIData
		{
			string jsonData = await RequestData(urlType, parameters);
			if (jsonData == null || jsonData == string.Empty)
				return null;

			return JsonConvert.DeserializeObject<T>(jsonData);
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
