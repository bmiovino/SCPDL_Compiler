using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler.ConsoleApp
{
    public class Compiler
    {
        public Pin[] Pins;

        public Clock[] Clocks;

        public Wire[] Wires;

        public Method[] Methods;

        public void Compile(string code)
        {
            var lines = code.Split(new[] { '\r', '\n' });

            for(int i = 0; i < lines.Length; i++)
            {
                
            }
        }
        
        Regex rx_pin = new Regex(@"(PIN)\s(\d*)\s(IN|OUT|CLK)\s*=\s*(.*);", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        Regex rx_wire = new Regex(@"(WIRE)\s(.*);", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        Regex rx_comment = new Regex(@"\s+\/\/.*", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        Regex rx_method_start = new Regex(@"\s+(BEGIN)(.*)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        Regex rx_method_end = new Regex(@"\s+(END)\s*(.*)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);

        public static Regex rx_terms = new Regex(@"([A-Z0-9]+)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        
        public enum TargetFrameworkEnum
        {
            None,
            ARV
        }

        public enum DeviceEnum
        {
            None
        }

    }
}
