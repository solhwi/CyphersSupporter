﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyphersSupporterBot
{
    /// <summary>
	/// 입력받은 커맨드에 적절한 행동을 취하고 응답을 돌려줌
	/// </summary>
	internal class CommandExecutor
    {
        private Dictionary<CommandType, Func<Command, Task<Message>>> taskDictionary = null;

        public CommandExecutor()
        {
            taskDictionary = new Dictionary<CommandType, Func<Command, Task<Message>>>
            {
                { CommandType.Tier, OnGetTier },
                { CommandType.RatingBattleHistory, OnGetBattleHistory },
                { CommandType.NormalBattleHistory, OnGetBattleHistory },
                { CommandType.CharacterHistory, OnGetCharacterHistory },
                { CommandType.Party, OnGetParty },
                { CommandType.RandomCharacter, OnGetRandomCharacter },
                { CommandType.RandomCharacterByPosition, OnGetRandomCharacterByPosition },
                { CommandType.Information, OnGetInformation },
                { CommandType.CharacterBGM, OnGetCharacterBGM },
                { CommandType.JazzBGM, OnGetJazzBGM },
            };
        }

        public async Task<string> Execute(Command command)
        {
            return await ProcessResponse(command);
        }

        private async Task<string> ProcessResponse(Command command)
        {
            Message data = null;
            if (taskDictionary.TryGetValue(command.commandType, out var doTask))
            {
                data = await doTask(command);
            }

            return data?.ReadMessage() ?? string.Empty;
        }

        private async Task<Message> OnGetTier(Command command)
        {
            if (command is NameCommand nameCommand == false)
                return null;

            var responseData = new TierMessage();
            await responseData.MakeMessage(nameCommand);

            return responseData;
        }

        private async Task<Message> OnGetBattleHistory(Command command)
        {
            if (command is RatingAndNameCommand rnCommand == false)
                return null;

            var responseData = new BattleHistoryMessage();
            await responseData.MakeMessage(rnCommand);

            return responseData;
        }

        private async Task<Message> OnGetCharacterHistory(Command command)
        {
            if (command is RatingAndNameCommand rnCommand == false)
                return null;

            var responseData = new CharacterHistoryMessage();
            await responseData.MakeMessage(rnCommand);

            return responseData;
        }

        private async Task<Message> OnGetParty(Command command)
        {
            if (command is NameCommand nameCommand == false)
                return null;

            var responseData = new PartyMessage();
            await responseData.MakeMessage(nameCommand);

            return responseData;
        }

        private async Task<Message> OnGetRandomCharacter(Command command)
        {
            var responseData = new RandomCharacterMessage();
            await responseData.MakeMessage(command);

            return responseData;
        }

        private async Task<Message> OnGetRandomCharacterByPosition(Command command)
        {
            if (command is NameCommand nameCommand == false)
                return null;

            var responseData = new RandomCharacterByPositionMessage();
            await responseData.MakeMessage(nameCommand);

            return responseData;
        }

        private async Task<Message> OnGetInformation(Command command)
        {
            var responseData = new InformationMessage();
            await responseData.MakeMessage(command);

            return responseData;
        }

        private async Task<Message> OnGetCharacterBGM(Command command)
        {
            if (command is NameCommand nameCommand == false)
                return null;

            var responseData = new CharacterBGMMessage();
            await responseData.MakeMessage(nameCommand);

            return responseData;
        }

        private async Task<Message> OnGetJazzBGM(Command command)
        {
            var responseData = new JazzBGMMessage();
            await responseData.MakeMessage(command);

            return responseData;
        }

    }
}
