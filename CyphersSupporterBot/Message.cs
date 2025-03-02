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

            var playerData = await CyphersAPI.RequestData<CyphersAPIPlayerData>(APIType.GetPlayerData, nameCommand.name);
            if (playerData == null)
                return;

            if (playerData.rows == null || playerData.rows.Length < 1)
                return;

            var data = playerData.rows[0];
            if (data == null)
                return;

            var playerDetailData = await CyphersAPI.RequestData<CyphersAPIPlayerDetailData>(APIType.GetPlayerDetailData, data.playerId);
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

            jsonData = await CyphersAPI.RequestData(URLType.GetPlayerMatchingHistory, data.playerId, rnCommand.GetRatingString(), battleHistoryCount.ToString());
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
        

        internal override string ReadMessage()
        {
            StringBuilder sb = new StringBuilder();

            return sb.ToString();
        }
    }
}
