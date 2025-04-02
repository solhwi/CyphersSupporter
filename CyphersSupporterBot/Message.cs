using Newtonsoft.Json;
using System.Text;

namespace CyphersSupporterBot
{
    internal abstract class Message
    {
        internal abstract string ReadMessage();

        internal virtual async Task MakeMessage(Command command)
        {
            await Task.Yield();
        }
    }

    internal class TierMessage : Message
    {
        public string userName = string.Empty;

        public string tierName = string.Empty;
        public int currentRatingPoint = 0;
        public int bestRatingPoint = 0;
        public int victoryCount = 0;
        public int defeatCount = 0;
        public int stopCount = 0;
        public float winningRate = 0.0f;

        internal override async Task MakeMessage(Command command)
        {
            if (command is NameCommand nameCommand == false)
                return;

            string jsonData = await CyphersAPI.RequestData(URLType.GetPlayerData, nameCommand.name);
            if (jsonData == null || jsonData == string.Empty)
                return;

            var playerData = JsonConvert.DeserializeObject<CyphersAPIPlayerData>(jsonData);
            if (playerData == null)
                return;

            if (playerData.rows == null || playerData.rows.Length < 1)
                return;

            var data = playerData.rows[0];
            if (data == null)
                return;

            jsonData = await CyphersAPI.RequestData(URLType.GetPlayerDetailData, data.playerId);
            if (jsonData == null || jsonData == string.Empty)
                return;

            var playerDetailData = JsonConvert.DeserializeObject<CyphersAPIPlayerDetailData>(jsonData);
            if (playerDetailData == null)
                return;

            userName = playerDetailData.nickname;
            tierName = playerDetailData.tierName != null ? playerDetailData.tierName : "배치고사 중";
            currentRatingPoint = playerDetailData.ratingPoint != null ? playerDetailData.ratingPoint.Value : 0;
            bestRatingPoint = playerDetailData.maxRatingPoint != null ? playerDetailData.maxRatingPoint.Value : 0;

            if (playerDetailData.records == null || playerDetailData.records.Length < 1)
                return;

            victoryCount = playerDetailData.records[0].winCount;
            defeatCount = playerDetailData.records[0].loseCount;
            stopCount = playerDetailData.records[0].stopCount;

            float playCount = victoryCount + defeatCount;
            winningRate = 100 * victoryCount / playCount;
        }

        internal override string ReadMessage()
        {
            return $"{userName}의 전적 \n" +
                   $">> {tierName} <<\n" +
                   $"현재 RP: {currentRatingPoint}\n" +
                   $"최고 RP: {bestRatingPoint}\n" +
                   $"{victoryCount}승 {defeatCount}패 {stopCount}중단 ({winningRate.ToString("F1")}%)";
        }
    }

    internal class BattleHistoryMessage : Message
    {
        private class BattlePlayerInfo
        {
            public readonly string matchType = string.Empty;
            public readonly bool isRating = false;
            public readonly string characterName = string.Empty;
            public readonly string resultType = string.Empty;
            public readonly int killPoint = 0;
            public readonly int deathPoint = 0;
            public readonly int assigtPoint = 0;

            public BattlePlayerInfo(CyphersAPIMatchingHistoryData.Playinfo playInfo, string gameTypeId)
            {
                this.matchType = gameTypeId == "rating" ? "공식" : "일반";
                this.isRating = gameTypeId == "rating";
                this.characterName = playInfo.characterName;
                this.resultType = playInfo.result == "win" ? "승" : "패";
                this.killPoint = playInfo.killCount;
                this.deathPoint = playInfo.deathCount;
                this.assigtPoint = playInfo.assistCount;
            }
        }

        public string userName = string.Empty;
        private const int battleHistoryCount = 10;
        private List<BattlePlayerInfo> playerInfoList = new List<BattlePlayerInfo>();

        internal override async Task MakeMessage(Command command)
        {
            if (command is RatingAndNameCommand rnCommand == false)
                return;

            string jsonData = await CyphersAPI.RequestData(URLType.GetPlayerData, rnCommand.name);
            if (jsonData == null || jsonData == string.Empty)
                return;

            var playerData = JsonConvert.DeserializeObject<CyphersAPIPlayerData>(jsonData);
            if (playerData == null)
                return;

            if (playerData.rows == null || playerData.rows.Length < 1)
                return;

            var data = playerData.rows[0];
            if (data == null)
                return;

            DateTime endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays(0);
            DateTime startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays(-89);

            jsonData = await CyphersAPI.RequestData(URLType.GetPlayerMatchingHistory, data.playerId, rnCommand.GetRatingString(), startTime.ToString("yyyyMMddTHHmm"), endTime.ToString("yyyyMMddTHHmm"), battleHistoryCount.ToString());
            if (jsonData == null || jsonData == string.Empty)
                return;

            var matchingHistoryData = JsonConvert.DeserializeObject<CyphersAPIMatchingHistoryData>(jsonData);
            if (matchingHistoryData == null)
                return;

            if (matchingHistoryData.matches.rows == null || matchingHistoryData.matches.rows.Length < 1)
                return;

            var histories = matchingHistoryData.matches.rows.Take(battleHistoryCount);
            if (histories == null)
                return;

            userName = rnCommand.name;
            foreach (var history in histories)
            {
                var info = new BattlePlayerInfo(history.playInfo, rnCommand.GetRatingString());
                playerInfoList.Add(info);
            }
        }

        internal override string ReadMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($">> {userName}의 최근 전적 <<\n");

            foreach (var player in playerInfoList)
            {
                if (player.isRating)
                {
                    sb.Append($"{player.matchType} | {player.characterName} | {player.resultType} | {player.killPoint}킬 {player.assigtPoint}어시 {player.deathPoint}데스\n");
                }
                else
                {
                    sb.Append($"{player.matchType} | {player.characterName} | (일반전 세부 전적 미제공)\n");
                }
            }

            return sb.ToString();
        }
    }

    internal class CharacterHistoryMessage : Message
    {
        private class BattleInfo
        {
            public readonly string characterName = string.Empty;
            public readonly int playCount = 0;

            public BattleInfo(string characterName, int playCount)
            {
                this.characterName = characterName;
                this.playCount = playCount;
            }
        }

        public string userName = string.Empty;
        private const int characterCount = 5;
        private Dictionary<string, int> characterInfoDictionary = new Dictionary<string, int>();

        internal override async Task MakeMessage(Command command)
        {
            if (command is RatingAndNameCommand rnCommand == false)
                return;

            string jsonData = await CyphersAPI.RequestData(URLType.GetPlayerData, rnCommand.name);
            if (jsonData == null || jsonData == string.Empty)
                return;

            var playerData = JsonConvert.DeserializeObject<CyphersAPIPlayerData>(jsonData);
            if (playerData == null)
                return;

            if (playerData.rows == null || playerData.rows.Length < 1)
                return;

            var data = playerData.rows[0];
            if (data == null)
                return;

            userName = rnCommand.name;

            // 최대 300일로 보정
            // 3일에 100판을 한다고 가정
            for(int i = 0; i < 80; i++)
            {
                int offsetDay = i * -3;

                DateTime endTime = DateTime.Now.AddDays(offsetDay);
                DateTime startTime = DateTime.Now.AddDays(offsetDay - 3);

                jsonData = await CyphersAPI.RequestData(URLType.GetPlayerMatchingHistory, data.playerId, rnCommand.GetRatingString(), startTime.ToString("yyyyMMddTHHmm"), endTime.ToString("yyyyMMddTHHmm"), 1000.ToString());
                if (jsonData == null || jsonData == string.Empty)
                    continue;

                var matchingHistoryData = JsonConvert.DeserializeObject<CyphersAPIMatchingHistoryData>(jsonData);
                if (matchingHistoryData == null)
                    continue;

                if (matchingHistoryData.matches.rows == null || matchingHistoryData.matches.rows.Length < 1)
                    continue;

                var histories = matchingHistoryData.matches.rows;
                if (histories == null)
                    continue;

                foreach (var history in histories)
                {
                    string id = history.playInfo.characterId;
                    if (characterInfoDictionary.ContainsKey(id))
                    {
                        characterInfoDictionary[id]++;
                    }
                    else
                    {
                        characterInfoDictionary[id] = 1;
                    }
                }
            }
        }


        internal override string ReadMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($">> {userName}의 캐릭터 통계 <<\n");

            for(int i = 0; i < characterCount; i++)
            {
                if (characterInfoDictionary.Count == 0)
                    break;

                var pair = characterInfoDictionary.MaxBy(pair => pair.Value);

                if (CyphersAPI.characterNameDictionary.TryGetValue(pair.Key, out var characterName))
                {
                    sb.Append($">> {i + 1}. {characterName} {pair.Value}판 <<\n");
                }

                characterInfoDictionary.Remove(pair.Key);
            }

            return sb.ToString();
        }
    }

    internal class PartyMessage : Message
    {
        internal override string ReadMessage()
        {
            throw new NotImplementedException();
        }

        internal override async Task MakeMessage(Command command)
        { 
        
        }
    }

    internal class RandomCharacterMessage : Message
    {
        internal override string ReadMessage()
        {
            throw new NotImplementedException();
        }

        internal override async Task MakeMessage(Command command)
        {

        }
    }

    internal class RandomCharacterByPositionMessage : Message
    {
        internal override string ReadMessage()
        {
            throw new NotImplementedException();
        }

        internal override async Task MakeMessage(Command command)
        {

        }
    }

    internal class InformationMessage : Message
    {
        internal override string ReadMessage()
        {
            throw new NotImplementedException();
        }

        internal override async Task MakeMessage(Command command)
        {

        }
    }

    internal class CharacterBGMMessage : Message
    {
        internal override string ReadMessage()
        {
            throw new NotImplementedException();
        }

        internal override async Task MakeMessage(Command command)
        {

        }
    }

    internal class JazzBGMMessage : Message
    {
        internal override string ReadMessage()
        {
            throw new NotImplementedException();
        }

        internal override async Task MakeMessage(Command command)
        {

        }
    }
}
