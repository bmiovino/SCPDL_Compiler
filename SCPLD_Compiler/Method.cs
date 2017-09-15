using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SCPLD_Compiler
{
    public class Method
    {
        public string Name;
        protected string[] lines;
        protected Wire[] wires; //internal to method
        protected List<Command> commands;

        List<CommandRegex> regexs;

        public Method()
        {
            regexs = new List<CommandRegex>()
                {
                    new CommandRegex(Command.CommandType.Assignment, new Regex(@"([a-zA-Z0-9]*)\s+=\s+(.*);") ),
                    new CommandRegex(Command.CommandType.Wait, new Regex(@"(WAIT)\s*(\d*)\s*(ms|s);") )
                }; //in order of operation
        }

        /*
         * 
        MAIN
            OUT0 = (IN0&IN1&IN2&IN3&IN4&IN5);	
            WAIT 10ms;
            [CLK0-LOW->HIGH]:OUT1 = OUT0 & IN8;
            [CLK0-HIGH->LOW]:OUT1 = LOW;
            W0=OUT1;
            CALL:SUBROUTINE0;
            BLOCK A BEGIN
                WAIT 14ms;
            BLOCK A END
            WHILE[CLK0=LOW] BEGIN
                
            WHILE END
            LOOP X:5 BEGIN

            LOOP X END

            TRUTH_TABLE A [IN0,IN1,IN2,IN3:OUT1,OUT2] BEGIN
                0.0.1.1:0.1
                0.1.1.0:1.1
                X.X.X.X:0.0
            END TRUTH_TABLE

            FSM A

                STATE[1]:
                    OUT1 = IN1;
                    TRANSISTION[CLK-LOW->HIGH,IN0-LOW->HIGH]->STATE[2];
                STATE[2]:
                    OUT2 = IN2;
                    RELEASE-RETURN; //allow the loop to run other logic, save state and return.
                    RELEASE-RESET->STATE[1]; //allow the loop to run other logic and the reset to state 1
            END FSM A


        END MAIN*/
        
        public void Parse(string name, string[] lines)
        {
            this.Name = name;
            this.lines = lines;

            commands = new List<Command>();

            //parse each one
            foreach (var line in lines)
                foreach(CommandRegex cr in regexs)
                    if (ParseLine(cr, line))
                        break;
        }

        bool ParseLine(CommandRegex cr, string line)
        {
            if(cr.RegularExpression.IsMatch(line))
            {
                var match = cr.RegularExpression.Match(line);
                var command = new Command();
                command.Type = cr.Type;
                command.Parameters = new string[match.Groups.Count];
                for (int i = 0; i < match.Groups.Count; i++)
                    command.Parameters[i] = match.Groups[i].Value;
                commands.Add(command);
                return true;
            }

            return false;
        }

        //regex for each line type
        //assignment
        //wait
        //signal transistion - with assignment and boolean.
        //block

        public string ToCode()
        {
            return "";
        }

    }
}
 