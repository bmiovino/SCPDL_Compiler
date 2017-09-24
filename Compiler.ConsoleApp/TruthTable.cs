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
        public List<string> Lines = new List<string>();

        Regex truthtable_line = new Regex(@"", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        Regex truthtable_end = new Regex(@"", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override int BlockParse(int lineNumber, string[] lines)
        {
            return lineNumber;
        }

        public override Tuple<Pin[], Wire[]> FindPinsAndWires(Pin[] pins)
        {
            List<Pin> tpins = new List<Pin>();
            List<Wire> twires = new List<Wire>();



            return new Tuple<Pin[], Wire[]>(tpins.ToArray(), twires.ToArray());
        }

        public override string ToCode()
        {
            return base.ToCode();
        }

    }
}
