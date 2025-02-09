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
}
