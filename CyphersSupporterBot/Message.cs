using Newtonsoft.Json;

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
            tierName = playerDetailData.tierName != null ? playerDetailData.tierName : "UNRANK";
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

}
