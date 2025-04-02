using CyphersSupporterBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CyphersSupporterBot
{
    /// <summary>
    /// 입력받은 http request에 맞는 커맨드를 생성해냄
    /// ex. http://localhost:8000?p1=그님티&p2=소리쿤
    /// </summary>
    internal class CommandFactory
    {
        public static Command MakeCommand(HttpListenerRequest request)
        {
            var commandParameters = GetCommandParameters(request).ToArray();
            return MakeCommand_Internal(commandParameters);
        }

        private static Command MakeCommand_Internal(string[] commandParameters)
        {
            if (commandParameters == null || commandParameters.Length < 1)
                return null;

            // 1번 인자가 커맨드 타입
            var commandType = GetCommandType(commandParameters[0]);

            switch (commandType)
            {
                case CommandType.RatingBattleHistory:
                case CommandType.CharacterHistory:

                    if (commandParameters.Length < 2)
                        return null;

                    return new RatingAndNameCommand(commandType, commandParameters[1], true);

                case CommandType.NormalBattleHistory:

                    if (commandParameters.Length < 2)
                        return null;

                    return new RatingAndNameCommand(commandType, commandParameters[1], false);

                case CommandType.Party:
                case CommandType.Tier:
                case CommandType.RandomCharacterByPosition:
                case CommandType.CharacterBGM:

                    if (commandParameters.Length < 2)
                        return null;

                    return new NameCommand(commandType, commandParameters[1]);

                case CommandType.RandomCharacter:
                case CommandType.Information:
                case CommandType.JazzBGM:
                    return new Command(commandType);
            }

            Console.WriteLine("정의되지 않은 Command 만들기를 시도합니다.");
            return null;
        }

        private static CommandType GetCommandType(string commandType)
        {
            switch (commandType)
            {
                case "그님티":
                case "티어":
                    return CommandType.Tier;

                case "공식전적":
                case "공식전적검색":
                    return CommandType.RatingBattleHistory;

                case "일반전적":
                case "일반전적검색":
                    return CommandType.NormalBattleHistory;

                case "캐릭터":
                case "캐릭터통계":
                    return CommandType.CharacterHistory;

                case "파티":
                case "친구":
                    return CommandType.Party;

                case "랜덤":
                    return CommandType.RandomCharacter;

                case "포지션랜덤":
                    return CommandType.RandomCharacterByPosition;

                case "한줄지식":
                    return CommandType.Information;

                case "재즈":
                case "재즈BGM":
                case "재즈브금":
                    return CommandType.JazzBGM;

                case "캐릭터BGM":
                case "캐릭터브금":
                    return CommandType.CharacterBGM;
            }

            return CommandType.None;
        }

        private static IEnumerable<string> GetCommandParameters(HttpListenerRequest request)
        {
            if (request.Url == null)
                yield break;

            string query = request.Url.Query;
            foreach (var p1 in query.Split('&'))
            {
                string[] p2 = p1.Split('=');
                yield return HttpUtility.UrlDecode(p2[1], Encoding.UTF8);
            }
        }
    }
}
