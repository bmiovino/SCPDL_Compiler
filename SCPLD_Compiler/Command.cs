using System.Text.RegularExpressions;

namespace SCPLD_Compiler
{
    public class Command
    {
        public CommandType Type;
        public string[] Parameters;

        public enum CommandType
        {
            None = 0,
            Assignment,
            Wait
        }
    }

    public class CommandRegex
    {
        public Command.CommandType Type;
        public Regex RegularExpression;

        public CommandRegex(Command.CommandType type, Regex regex)
        {
            Type = type;
            RegularExpression = regex;
        }

        
    }
}