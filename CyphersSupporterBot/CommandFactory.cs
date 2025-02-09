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
    /// ex. http://localhost:8000/Kaluha?p1=완료&p2=유진
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
                case CommandType.Tier:

                    if (commandParameters.Length < 2)
                        return null;

                    string name = commandParameters[1];

                    return new NameCommand(commandType, name);
            }

            return null;
        }

        private static CommandType GetCommandType(string commandType)
        {
            switch (commandType)
            {
                case "그님티":
                case "티어":
                    return CommandType.Tier;
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
