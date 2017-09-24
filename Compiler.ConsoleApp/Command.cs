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