using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Compiler.ConsoleApp
{
    public class TruthTable : Command
    {
        //public List<string> Lines = new List<string>();
        public List<string> Inputs = new List<string>();
        public List<string> Outputs = new List<string>();

        string headerLine = "";
        List<string> mappingLines = new List<string>();
        string defaultLine = null;

        Regex truthtable_line = new Regex(@"(.*):(.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        Regex truthtable_end = new Regex(@"END\s+TRUTH\s+TABLE", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        Regex truthtable_header = new Regex(@"TRUTH\s+TABLE\s+\[(.*):(.*)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override int BlockParse(int lineNumber, string[] lines)
        {
            if(truthtable_header.IsMatch(lines[lineNumber]))
            {
                var match = truthtable_header.Match(lines[lineNumber]);

                headerLine = lines[lineNumber];

                var ins = match.Groups[1].Value.Split(',');

                foreach(var inv in ins)
                    Inputs.Add(inv.Trim());

                var outs = match.Groups[2].Value.Split(',');

                foreach (var outv in outs)
                    Outputs.Add(outv.Trim());

                lineNumber++;
            }
            else
            {
                //error
            }

            bool truthTableEndFound = false;

            while(!truthTableEndFound)
            {
                truthTableEndFound = truthtable_end.IsMatch(lines[lineNumber]);

                if(!truthTableEndFound)
                {
                    var match = truthtable_line.Match(lines[lineNumber]);

                    var ins = match.Groups[1].Value.Trim();
                    
                    if(ins.ToLower() == "default")
                        defaultLine = lines[lineNumber];
                    else
                        mappingLines.Add(lines[lineNumber]);
                }

                lineNumber++;
            }
            
            return lineNumber;
        }

        public override Tuple<Pin[], Wire[]> FindPinsAndWires(Pin[] pins)
        {
            List<Pin> tpins = new List<Pin>();
            List<Wire> twires = new List<Wire>();

            var elements = Inputs.Union(Outputs).Distinct();

            tpins = (from p in pins join e in elements on p.Name equals e select p).ToList();

            twires = (from p in elements where !tpins.Where(tp => tp.Name == p).Any() select new Wire() { Name = p }).ToList();

            return new Tuple<Pin[], Wire[]>(tpins.ToArray(), twires.ToArray());
        }

        public override string ToCode()
        {
            var code = "//Truth Table\r\n\r\n";

            for(int i = 0; i < mappingLines.Count; i++)
            {
                if (i > 0)
                    code += "else ";

                code += "if (" + CodeCondition(mappingLines[i]) + ") {\r\n";
                code += Assignment(mappingLines[i]);
                code += "}\r\n";
            }

            code += "else {\r\n";

            if (defaultLine != null)
                code += Assignment(defaultLine);

            code += "}\r\n\r\n";

            return code;
        }

        private string Assignment(string v)
        {
            var code = "";

            var e = v.Split(':')[1].Split(',');

            int c = 0;

            foreach(var i in e)
            {
                var assignment = i.Trim();

                code += "\t" + Outputs[c] + "_localvar = " + assignment + ";\r\n";

                c++;
            }

            return code;
        }

        private string CodeCondition(string v)
        {
            var code = "";

            var e = v.Split(':')[0].Split(',');

            int c = 0;

            foreach (var i in e)
            {
                var condition = i.Trim();

                if (code.Length > 0)
                    code += " && ";

                code += Inputs[c] + "_localvar == " + condition;

                c++;
            }

            return code;
        }
    }
}
