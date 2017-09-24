using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.ConsoleApp
{
    public class TruthTable : Command
    {
        public List<string> Lines = new List<string>();

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
