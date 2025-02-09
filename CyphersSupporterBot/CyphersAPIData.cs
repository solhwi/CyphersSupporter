using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyphersSupporterBot
{
    internal abstract class CyphersAPIData
    {

    }

    internal class CyphersAPIPlayerData : CyphersAPIData
    {
        public class Row
        {
            public string playerId { get; set; }
            public string nickname { get; set; }
            public Represent represent { get; set; }
            public int grade { get; set; }
        }

        public class Represent
        {
            public string characterId { get; set; }
            public string characterName { get; set; }
        }

        public Row[] rows { get; set; }
    }

    public class CyphersAPIPlayerDetailData
    {
        public class Represent
        {
            public string characterId { get; set; }
            public string characterName { get; set; }
        }

        public class Record
        {
            public string gameTypeId { get; set; }
            public int winCount { get; set; }
            public int loseCount { get; set; }
            public int stopCount { get; set; }
            public int playCount { get; set; }
        }

        public string playerId { get; set; }
        public string nickname { get; set; }
        public int grade { get; set; }
        public bool tierTest { get; set; }
        public Represent represent { get; set; }
        public string clanName { get; set; }
        public int? ratingPoint { get; set; }
        public int? maxRatingPoint { get; set; }
        public string tierName { get; set; }
        public Record[] records { get; set; }
    }


}
