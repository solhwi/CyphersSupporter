using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace CyphersSupporterBot
{
    internal abstract class CyphersAPICommand
    {
        protected abstract string GetAdditionalURL { get; }

        protected virtual string GetURL
        {
            get
            {
                if (commandParameters == null)
                    return string.Empty;

                string additionalURL = GetAdditionalURL;

                if (commandParameters.Count == 1)
                {
                    additionalURL = string.Format(GetAdditionalURL, commandParameters[0]);
                }
                else if (commandParameters.Count == 2)
                {
                    additionalURL = string.Format(GetAdditionalURL, commandParameters[0], commandParameters[1]);
                }
                else if (commandParameters.Count == 3)
                {
                    additionalURL = string.Format(GetAdditionalURL, commandParameters[0], commandParameters[1], commandParameters[2]);
                }
                else if (commandParameters.Count == 4)
                {
                    additionalURL = string.Format(GetAdditionalURL, commandParameters[0], commandParameters[1], commandParameters[2], commandParameters[3]);
                }

                return CyphersAPIServerBaseURL + additionalURL;
            }
        }

        protected const string CyphersAPIServerBaseURL = "https://api.neople.co.kr/cy/";
        protected const string APIKey = "gCD6zHBmCvFW2vN5Tsxy5mWHgyj28zGH";

        protected List<string> commandParameters = new List<string>();

        public CyphersAPICommand(IEnumerable<string> commandParameters)
        {
            this.commandParameters = commandParameters.ToList();
        }

        public async Task<string> Execute()
        {
            return await CyphersAPI.Get(GetURL);
        }
    }

    /// <summary>
    /// 0. 유저 닉네임
    /// </summary>
    internal class GetPlayerDataCommand : CyphersAPICommand
    {
        protected override string GetAdditionalURL => "players?nickname={0}&wordType=match&apikey=" + APIKey;

        public GetPlayerDataCommand(IEnumerable<string> commandParameters) : base(commandParameters) 
        { 
        
        }
    }

    /// <summary>
    /// 0. PlayerId
    /// </summary>
    internal class GetPlayerDetailDataCommand : CyphersAPICommand
    {
        protected override string GetAdditionalURL => "players/{0}?apikey=" + APIKey;

        public GetPlayerDetailDataCommand(IEnumerable<string> commandParameters) : base(commandParameters)
        {

        }
    }

    /// <summary>
    /// 0. playerId
    /// 1. 시작일 (20231212 등으로 입력)
    /// 2. 종료일
    /// </summary>
    internal class GetPlayerMatchingHistoryCommand : CyphersAPICommand
    {
        public GetPlayerMatchingHistoryCommand(IEnumerable<string> commandParameters) : base(commandParameters)
        {
        }

        protected override string GetAdditionalURL
        {
            get
            {
                return "players/{0}/matches?gameTypeId=rating&startDate={1}&endDate={2}&limit=<limit>&next=<next>&apikey=";
            }
        }
    }

    /// <summary>
    /// 0. playerId
    /// </summary>
    internal class GetPlayerMatchDataCommand : CyphersAPICommand
    {
        public GetPlayerMatchDataCommand(IEnumerable<string> commandParameters) : base(commandParameters)
        {
        }

        protected override string GetAdditionalURL
        {
            get
            {
                return "matches/{0}?&apikey=";
            }
        }
    }

    /// <summary>
    /// 0. playerId
    /// 1. 몇 위부터 검색할 지
    /// 2. 몇 개를 검색할 지
    /// </summary>
    internal class GetPlayerRankingDataCommand : CyphersAPICommand
    {
        public GetPlayerRankingDataCommand(IEnumerable<string> commandParameters) : base(commandParameters)
        {
        }

        protected override string GetAdditionalURL
        {
            get
            {
                return "ranking/ratingpoint?playerId={0}&offset={1}&limit={2}&apikey=";
            }
        }
    }

    public enum CharacterRankingType
    {
        winCount = 0,
        winRate = 1,
        killCount = 2,
        assistCount = 3,
        exp = 4,
    }

    /// <summary>
    /// 0. characterId, 어떤 캐릭터를 검색할 지
    /// 1. 승리수: winCount, 승률: winRate, 킬: killCount, 도움: assistCount, 경험치:exp
    /// 2. playerId
    /// </summary>
    internal class GetCharacterRankingDataCommand : CyphersAPICommand
    {
        public GetCharacterRankingDataCommand(IEnumerable<string> commandParameters) : base(commandParameters)
        {
        }

        protected override string GetAdditionalURL
        {
            get
            {
                return "ranking/characters/{0}/{1}?playerId={2}&offset=<offset>&limit=<limit>&apikey=";
            }
        }
    }

    internal class GetAllCharacterDataCommand : CyphersAPICommand
    {
        public GetAllCharacterDataCommand(IEnumerable<string> commandParameters) : base(commandParameters)
        {
        }

        protected override string GetAdditionalURL
        {
            get
            {
                return "characters?apikey=";
            }
        }
    }
}
