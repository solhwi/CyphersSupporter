using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyphersSupporterBot
{
    internal static class LocalDataManager
    {
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
