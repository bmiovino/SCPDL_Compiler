using System.Text.RegularExpressions;

namespace Compiler.ConsoleApp
{
    public class Command
    {
        public CommandType Type;
        public string[] Parameters;

        public enum CommandType
        {
            None = 0,
            Assignment,
            Wait,
            BlockScope,
            EmptyLine
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