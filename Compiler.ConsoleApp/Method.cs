using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Compiler.ConsoleApp
{
    public class Method
    {
        public string Name = "NOTSET";
        public string[] Lines = new string[] { };
        public Wire[] Wires; //internal to method
        public Pin[] Pins; //interface to external pins
        public List<Command> Commands = new List<Command>();
        public List<TruthTable> TruthTables = new List<TruthTable>();
        public int LineNumber = 0;

        List<CommandRegex> regexs;

        public Method()
        {
            regexs = new List<CommandRegex>()
                {
                    new CommandRegex(Command.CommandType.Assignment, new Regex(@"([a-zA-Z0-9]*)\s+=\s+(.*);") ),
                    new CommandRegex(Command.CommandType.Wait, new Regex(@"(WAIT)\s*(\d*)\s*(ms|s);") ),
                    new CommandRegex(Command.CommandType.TruthTable, new Regex(@"(TRUTH\s+TABLE)\s+\[(.*):(.*)\]")),
                    new CommandRegex(Command.CommandType.BlockScope, new Regex(@"(BEGIN|END)")){Skip = true },
                    new CommandRegex(Command.CommandType.EmptyLine, new Regex(@"\s*")){ Skip = true }
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

        public void Parse()
        {
            Commands = new List<Command>();

            //parse each one
            for(LineNumber = 0; LineNumber < Lines.Length; LineNumber++)
                foreach (CommandRegex cr in regexs)
                    if (ParseLine(cr, Lines[LineNumber]))
                        break;
        }

        bool ParseLine(CommandRegex cr, string line)
        {
            if (cr.RegularExpression.IsMatch(line))
            {
                if(!cr.Skip)
                { 
                    var match = cr.RegularExpression.Match(line);
                    var command = Command.Factory(cr.Type);
                    command.Type = cr.Type;
                    command.Parameters = new string[match.Groups.Count - 1];
                    for (int i = 1; i < match.Groups.Count; i++)
                        command.Parameters[i - 1] = match.Groups[i].Value;
                    LineNumber = command.BlockParse(LineNumber, Lines); //if command is a block command
                    Commands.Add(command);
                }
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// looking at all assignment and conditional expression terms, set the list of all that
        /// exist in this method block and determine which are pins and which are wires.
        /// </summary>
        /// <param name="pins"></param>
        public void SetPinsAndWires(Pin[] pins)
        {
            #region #------------------- Scan Assignments -----------------------------#
            var commands = (from c in Commands where c.Type == Command.CommandType.Assignment select c).ToList();

            var terms = new HashSet<string>();

            var wires = new List<Wire>();

            var tpins = new List<Pin>();

            commands.ForEach(c => {
                for(int i = 0; i < c.Parameters.Length; i++)
                {
                    var match = Compiler.rx_terms.Match(c.Parameters[i]);
                    for(int j= 0; j < match.Groups.Count; j++)
                        if (!terms.Contains(match.Groups[j].Value))
                            terms.Add(match.Groups[j].Value);
                }
            });

            foreach(var term in terms)
            {
                var pin = pins.FirstOrDefault(p => { return p.Name == term; });
                if (pin == null)
                    wires.Add(new Wire() { Name = term });
                else
                    tpins.Add(pin);
            }

            #endregion

            #region #------------------  Scan all other command children classes -----------------------#

            commands = (from c in Commands where c.Type == Command.CommandType.TruthTable //|| other command types
                        select c).ToList();

            foreach(var command in commands)
            {
                var res = command.FindPinsAndWires(pins);

                foreach(var pin in res.Item1)
                {
                    var foundPin = (from p in tpins where p.Name == pin.Name select p).FirstOrDefault();
                    if(foundPin == null)
                        tpins.Add(pin);
                }

                foreach (var wire0 in res.Item2)
                {
                    var foundWire = (from w in wires where w.Name == wire0.Name select w).FirstOrDefault();
                    if (foundWire == null)
                        wires.Add(wire0);
                }
            }

            #endregion

            Wires = wires.ToArray();
            Pins = tpins.ToArray();
        }
        
        public string ToCode(Compiler.TargetFrameworkEnum target)
        {
            string c =  $"void fn_{Name}(){{\r\n";
            
            //gather all inputs - readvalues to create variables.
            foreach(var inPin in Pins.Where(p=> p.Direction == Pin.DirectionEnum.In).ToArray())
                c += $"\tint {inPin.Name}_localvar = digitalRead({inPin.Name});\r\n";

            //all assignments -> create variables
            //find all unqiue assignment variables
            //create them here
            foreach (var outPin in Pins.Where(p => p.Direction == Pin.DirectionEnum.Out).ToArray())
                c += $"\tint {outPin.Name}_localvar = 0;\r\n";

            foreach (var wire in Wires)
                c += $"\tint {wire.Name}_localvar = 0;\r\n";

            //perform all assignments
            foreach (var command in Commands.Where(cm => cm.Type == Command.CommandType.Assignment).ToArray())
                c += $"\t{command.Parameters[0]}_localvar = {ToLocalVariables(command.Parameters[1])};\r\n";

            //perform all others --> add them as developed here
            foreach (var command in Commands.Where(cm => cm.Type == Command.CommandType.TruthTable).ToArray())
                c += $"\t{command.Parameters[0]}_localvar = {ToLocalVariables(command.Parameters[1])};\r\n";

            //write all outputs
            foreach (var outPin in Pins.Where(p => p.Direction == Pin.DirectionEnum.Out).ToArray())
                c += $"\tdigitalWrite({outPin.Name}, {outPin.Name}_localvar);\r\n";
            
            c += "}\r\n";

            return c;
        }

        public string ToLocalVariables(string expression)
        {
            var names = (from w in Wires select w.Name).Union((from p in Pins select p.Name)).OrderByDescending(s => s.Length);

            foreach(var name in names)
                expression = expression.Replace(name, $"{name}_localvar");

            return expression;
        }


        public static Method Extract(string name, string[] lines)
        {
            Regex endMethodrx = new Regex($"END\\s*{name}", RegexOptions.IgnoreCase);

            Method method = new Method();

            method.Name = name;

            for (int i = 0; i < lines.Length; i++)
                if (endMethodrx.IsMatch(lines[i]))
                {
                    method.Lines = lines.ToList().GetRange(0, i + 1).ToArray();
                    break;
                }

            return method;
        }

        public static Method ExtractAndParse(string name, string[] lines)
        {
            var method = Extract(name, lines);

            method.Parse();

            return method;
        }
    }
}
 