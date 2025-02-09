using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyphersSupporterBot
{
    internal class CyphersAPIData
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

	internal class CyphersAPIPlayerDetailData : CyphersAPIData
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

	internal class CyphersAPIMatchingHistoryData : CyphersAPIData
	{
        public string playerId { get; set; }
        public string nickname { get; set; }
        public int grade { get; set; }
        public bool tierTest { get; set; }
        public Represent represent { get; set; }
        public string clanName { get; set; }
        public int ratingPoint { get; set; }
        public int maxRatingPoint { get; set; }
        public string tierName { get; set; }
        public Record[] records { get; set; }
        public Matches matches { get; set; }

        public class Represent
        {
            public string characterId { get; set; }
            public string characterName { get; set; }
        }

        public class Matches
        {
            public Date date { get; set; }
            public string gameTypeId { get; set; }
            public object next { get; set; }
            public Row[] rows { get; set; }
        }

        public class Date
        {
            public string start { get; set; }
            public string end { get; set; }
        }

        public class Row
        {
            public string date { get; set; }
            public string matchId { get; set; }
            public Map map { get; set; }
            public Playinfo playInfo { get; set; }
            public Position position { get; set; }
        }

        public class Map
        {
            public string mapId { get; set; }
            public string name { get; set; }
        }

        public class Playinfo
        {
            public bool random { get; set; }
            public int partyUserCount { get; set; }
            public object partyInfo { get; set; }
            public string playTypeName { get; set; }
            public string characterId { get; set; }
            public string characterName { get; set; }
            public int level { get; set; }
            public int playTime { get; set; }
            public string result { get; set; }
            public int killCount { get; set; }
            public int deathCount { get; set; }
            public int assistCount { get; set; }
            public int attackPoint { get; set; }
            public int damagePoint { get; set; }
            public int battlePoint { get; set; }
            public int sightPoint { get; set; }
            public int towerAttackPoint { get; set; }
            public int backAttackCount { get; set; }
            public int comboCount { get; set; }
            public int spellCount { get; set; }
            public int healAmount { get; set; }
            public int sentinelKillCount { get; set; }
            public int demolisherKillCount { get; set; }
            public int trooperKillCount { get; set; }
            public int guardianKillCount { get; set; }
            public int guardTowerKillCount { get; set; }
            public int getCoin { get; set; }
            public int spendCoin { get; set; }
            public int spendConsumablesCoin { get; set; }
            public int responseTime { get; set; }
            public int minLifeTime { get; set; }
            public int maxLifeTime { get; set; }
            public Multikillcount multiKillCount { get; set; }
        }

        public class Multikillcount
        {
            public int _double { get; set; }
            public int triple { get; set; }
            public int quadruple { get; set; }
            public int genocide { get; set; }
        }

        public class Position
        {
            public string name { get; set; }
            public string explain { get; set; }
            public Attribute[] attribute { get; set; }
        }

        public class Attribute
        {
            public int level { get; set; }
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Record
        {
            public string gameTypeId { get; set; }
            public int winCount { get; set; }
            public int loseCount { get; set; }
            public int stopCount { get; set; }
            public int playCount { get; set; }
        }
    }

	internal class CyphersAPIPlayerMatchData : CyphersAPIData
	{
        public string date { get; set; }
        public string gameTypeId { get; set; }
        public Map map { get; set; }
        public Team[] teams { get; set; }
        public Player[] players { get; set; }

        public class Map
        {
            public string mapId { get; set; }
            public string name { get; set; }
        }

        public class Team
        {
            public string result { get; set; }
            public string[] players { get; set; }
        }

        public class Player
        {
            public string playerId { get; set; }
            public string nickname { get; set; }
            public Map1 map { get; set; }
            public Playinfo playInfo { get; set; }
            public Position position { get; set; }
            public string[] itemPurchase { get; set; }
            public Item[] items { get; set; }
        }

        public class Map1
        {
            public string mapId { get; set; }
            public string name { get; set; }
        }

        public class Playinfo
        {
            public bool random { get; set; }
            public int partyUserCount { get; set; }
            public string partyId { get; set; }
            public string playTypeName { get; set; }
            public string characterId { get; set; }
            public string characterName { get; set; }
            public int level { get; set; }
            public int playTime { get; set; }
            public int killCount { get; set; }
            public int deathCount { get; set; }
            public int assistCount { get; set; }
            public int attackPoint { get; set; }
            public int damagePoint { get; set; }
            public int battlePoint { get; set; }
            public int sightPoint { get; set; }
            public int towerAttackPoint { get; set; }
            public int backAttackCount { get; set; }
            public int comboCount { get; set; }
            public int spellCount { get; set; }
            public int healAmount { get; set; }
            public int sentinelKillCount { get; set; }
            public int demolisherKillCount { get; set; }
            public int trooperKillCount { get; set; }
            public int guardianKillCount { get; set; }
            public int guardTowerKillCount { get; set; }
            public int getCoin { get; set; }
            public int spendCoin { get; set; }
            public int spendConsumablesCoin { get; set; }
            public int responseTime { get; set; }
            public int minLifeTime { get; set; }
            public int maxLifeTime { get; set; }
            public Multikillcount multiKillCount { get; set; }
            public int praiseCount { get; set; }
        }

        public class Multikillcount
        {
            public int _double { get; set; }
            public int triple { get; set; }
            public int quadruple { get; set; }
            public int genocide { get; set; }
        }

        public class Position
        {
            public string name { get; set; }
            public string explain { get; set; }
            public Attribute[] attribute { get; set; }
        }

        public class Attribute
        {
            public int level { get; set; }
            public string id { get; set; }
            public string name { get; set; }
        }

        public class Item
        {
            public string itemId { get; set; }
            public string itemName { get; set; }
            public string slotCode { get; set; }
            public string slotName { get; set; }
            public string rarityCode { get; set; }
            public string rarityName { get; set; }
            public string equipSlotCode { get; set; }
            public string equipSlotName { get; set; }
            public int upgrade { get; set; }
        }

    }

	internal class CyphersAPIRankingData : CyphersAPIData
	{
        public Row[] rows { get; set; }

        public class Row
        {
            public int rank { get; set; }
            public int beforeRank { get; set; }
            public string playerId { get; set; }
            public string nickname { get; set; }
            public int grade { get; set; }
            public int ratingPoint { get; set; }
            public string clanName { get; set; }
            public Represent represent { get; set; }
        }

        public class Represent
        {
            public string characterId { get; set; }
            public string characterName { get; set; }
        }

    }

	internal class CyphersAPICharacterRankingData : CyphersAPIData
	{
        public Row[] rows { get; set; }

        public class Row
        {
            public int rank { get; set; }
            public int beforeRank { get; set; }
            public string playerId { get; set; }
            public string nickname { get; set; }
            public int exp { get; set; }
        }

    }

    internal class CyphersAPICharacterData : CyphersAPIData
	{
        public Row[] rows { get; set; }

        public class Row
        {
            public string characterId { get; set; }
            public string characterName { get; set; }
        }
    }
}
