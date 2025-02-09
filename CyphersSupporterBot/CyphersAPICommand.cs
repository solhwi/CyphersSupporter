using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyphersSupporterBot
{
    internal abstract class CyphersAPICommand
    {
        protected abstract string GetAdditionalURL { get; }
        protected abstract string GetURL { get; }

        protected const string CyphersAPIServerBaseURL = "https://api.neople.co.kr/cy/";
        protected const string APIKey = "gCD6zHBmCvFW2vN5Tsxy5mWHgyj28zGH";

        protected List<string> commandParameters = new List<string>();

        public CyphersAPICommand(IEnumerable<string> commandParameters)
        {
            this.commandParameters = commandParameters.ToList();
        }

        public async Task<string> Execute()
        {
            return await CyphersAPIManager.Get(GetURL);
        }
    }

    internal class GetPlayerDataCommand : CyphersAPICommand
    {
        protected override string GetAdditionalURL => "players?nickname={0}&wordType={1}&apikey=" + APIKey;

        protected override string GetURL
        {
            get
            {
                if (commandParameters == null)
                    return string.Empty;

                if (commandParameters.Count < 1)
                    return string.Empty;

                string additionalURL = string.Format(GetAdditionalURL, commandParameters[0], "match");
                return CyphersAPIServerBaseURL + additionalURL;
            }
        }

        public GetPlayerDataCommand(IEnumerable<string> commandParameters) : base(commandParameters) 
        { 
        
        }
    }

    internal class GetPlayerDetailDataCommand : CyphersAPICommand
    {
        protected override string GetAdditionalURL => "players/{0}?apikey=" + APIKey;

        protected override string GetURL
        {
            get
            {
                if (commandParameters == null)
                    return string.Empty;

                if (commandParameters.Count < 1)
                    return string.Empty;

                string additionalURL = string.Format(GetAdditionalURL, commandParameters[0]);
                return CyphersAPIServerBaseURL + additionalURL;
            }
        }

        public GetPlayerDetailDataCommand(IEnumerable<string> commandParameters) : base(commandParameters)
        {

        }
    }
}
