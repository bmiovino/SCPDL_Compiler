using System;
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
            EmptyLine,
            TruthTable
        }

        public virtual int BlockParse(int lineNumber, string[] lines)
        {
            //process additional lines - inherit from command and override
            return lineNumber;
        }

        public virtual Tuple<Pin[], Wire[]> FindPinsAndWires(Pin[] pins)
        {
            return new Tuple<Pin[], Wire[]>(new Pin[] { }, new Wire[] { });
        }

        public virtual string ToCode()
        {
            return "";
        }

        public static Command Factory(CommandType commandType)
        {
            switch(commandType)
            {
                case CommandType.TruthTable:
                    return new TruthTable();
                default:
                    return new Command();
            }
        }
    }

    public class CommandRegex
    {
        public Command.CommandType Type;
        public Regex RegularExpression;
        public bool Skip = false;

        public CommandRegex(Command.CommandType type, Regex regex)
        {
            Type = type;
            RegularExpression = regex;
        }

        
    }
}