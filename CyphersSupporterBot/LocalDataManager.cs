using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyphersSupporterBot
{
    internal static class LocalDataManager
    {
        private static Random random = new Random();

        private static Dictionary<string, List<string>> positionCharacterNameDictionary = new Dictionary<string, List<string>>();
        private static List<string> informationList = new List<string>();
        private static Dictionary<string, string> characterBGMLinkDictionary = new Dictionary<string, string>();
        private static List<string> jazzBGMLinkList = new List<string>();

        public static void PreLoad()
        {
            positionCharacterNameDictionary = MakePositionData();
            informationList = MakeInformationList();
            characterBGMLinkDictionary = MakeCharacterBGMLinkDictionary();
            jazzBGMLinkList = MakeJazzBGMLinkList();
        }

        public static IEnumerable<string> GetAllCharacterNames()
        {
            foreach (var nameList in positionCharacterNameDictionary.Values)
            {
                foreach (var name in nameList)
                {
                    yield return name;
                }
            }
        }

        public static string GetRandomCharacterName()
        {
            var allList = GetAllCharacterNames().ToList();
            if (allList == null || allList.Count == 0)
                return string.Empty;

            int randomIndex = random.Next(0, allList.Count);
            return allList[randomIndex];
        }

        public static List<string> GetCharacterNameByPosition(string position)
        {
            if (positionCharacterNameDictionary.TryGetValue(position, out var nameList) == false)
                return new List<string>();

            return nameList;
        }

        public static string GetRandomCharacterNameByPosition(string position)
        {
            var names = GetCharacterNameByPosition(position);
            if (names == null || names.Count == 0)
                return string.Empty;

            int randomIndex = random.Next(0, names.Count);
            return names[randomIndex];
        }

        public static string GetRandomInformationText()
        {
            int randomIndex = random.Next(0, informationList.Count);
            return informationList[randomIndex];
        }

        public static string GetRandomJazzBGM()
        {
            int randomIndex = random.Next(0, jazzBGMLinkList.Count);
            return jazzBGMLinkList[randomIndex];
        }

        public static string GetRandomCharacterBGM()
        {
            var list = characterBGMLinkDictionary.Values.ToList();
            if (list == null || list.Count == 0)
                return string.Empty;

            int randomIndex = random.Next(0, list.Count);
            return list[randomIndex];
        }

        public static string GetCharacterBGM(string characterName)
        {
            if (characterBGMLinkDictionary.TryGetValue(characterName, out var url) == false)
                return string.Empty;

            return url;
        }

        private static Dictionary<string, string> MakeCharacterBGMLinkDictionary()
        {
            string filePath = $"{Config.ScriptRootPath}/characterBGMLink.csv";
            string rawText = File.ReadAllText(filePath);

            var sheetList = CSVReader.Read(rawText);
            if (sheetList == null || sheetList.Count == 0)
                return null;

            Dictionary<string, string> results = new Dictionary<string, string>();

            foreach (var sheet in sheetList)
            {
                string characterName = (string)sheet["characterName"];
                string url = (string)sheet["url"];
                
                results[characterName] = url;
            }

            return results;
        }

        private static List<string> MakeJazzBGMLinkList()
        {
            string filePath = $"{Config.ScriptRootPath}/jazzBGMLink.csv";
            string rawText = File.ReadAllText(filePath);

            var sheetList = CSVReader.Read(rawText);
            if (sheetList == null || sheetList.Count == 0)
                return null;

            List<string> results = new List<string>();

            foreach (var sheet in sheetList)
            {
                string url = (string)sheet["url"];
                results.Add(url);
            }

            return results;
        }

        private static List<string> MakeInformationList()
        {
            string filePath = $"{Config.ScriptRootPath}/Information.csv";
            string rawText = File.ReadAllText(filePath);

            var sheetList = CSVReader.Read(rawText);
            if (sheetList == null || sheetList.Count == 0)
                return null;

            List<string> results = new List<string>();

            foreach (var sheet in sheetList)
            {
                string text = (string)sheet["text"];
                results.Add(text);
            }

            return results;
        }

        private static Dictionary<string, List<string>> MakePositionData()
        {
            string filePath = $"{Config.ScriptRootPath}/PositionCharacterName.csv";
            string rawText = File.ReadAllText(filePath);

            var sheetList = CSVReader.Read(rawText);
            if (sheetList == null || sheetList.Count == 0)
                return null;

            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>()
            {
                { "탱커", new List<string>() },
                { "근딜", new List<string>() },
                { "원딜", new List<string>() },
                { "서포터", new List<string>() },
            };

            foreach (var sheet in sheetList)
            {
                string position = (string)sheet["position"];
                string characterName = (string)sheet["characterName"];

                if (dictionary.ContainsKey(position))
                {
                    dictionary[position].Add(characterName);
                }
            }

            return dictionary;
        }
    }
}
