using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyphersSupporterBot
{
    public enum CommandType
    {
        None = -1,
        Tier = 0,
        RatingBattleHistory = 1,
        NormalBattleHistory = 2,
        CharacterHistory = 3,
        Party = 4,
        RandomCharacter = 5,
        RandomCharacterByPosition = 6,
        Information = 7,
        CharacterBGM = 8,
        JazzBGM = 9,
    }

    public class Command
    {
        public readonly CommandType commandType = CommandType.None;

        public Command(CommandType commandType)
        {
            this.commandType = commandType;
        }
    }

    public class NameCommand : Command
    {
        public readonly string name = string.Empty;

        public NameCommand(CommandType commandType, string name) : base(commandType)
        {
            this.name = name;
        }
    }

    public class RatingAndNameCommand : Command
    {
        public readonly string name = string.Empty;
        public readonly bool isRating = false;

        public RatingAndNameCommand(CommandType commandType, string name, bool isRating) : base(commandType)
        {
            this.name = name;
            this.isRating = isRating;
        }

        public string GetRatingString()
        {
            return isRating ? "rating" : "normal";
        }
    }
}
