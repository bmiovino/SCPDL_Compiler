using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler.ConsoleApp
{
    public class Compiler
    {
        public List<Pin> Pins = new List<Pin>();

        public List<Wire> Wires = new List<Wire>();

        public List<Method> Methods = new List<Method>();

        public string Code = "";

        public void Compile(string code, TargetFrameworkEnum target)
        {
            var lines = code.Split(new[] { '\r', '\n' });

            for(int i = 0; i < lines.Length; i++)
            {
                if (rx_pin.IsMatch(lines[i]))
                    CreatePin(lines[i]);
                else if (rx_wire.IsMatch(lines[i]))
                    CreateWire(lines[i]);
                else if(rx_method_start.IsMatch(lines[i]))
                    i += CreateMethod(lines, i);
                //else ignore for now
            }

            //set pins and wires in methods
            Methods.ToList().ForEach(m => { m.SetPinsAndWires(Pins.ToArray()); });

            GenerateCode(target);
        }

        private void GenerateCode(TargetFrameworkEnum target)
        {
            Code += @"/*****************************************************************/" + "\r\n";
            Code += $"// {DateTime.Now} - BMI - SCPDL Compiler v1.0" + "\r\n";
            Code += @"/*****************************************************************/" + "\r\n";
            
            foreach (var pin in Pins)
                Code += $"#define {pin.Name} {pin.Number}\r\n";

            foreach (var wire in Wires)
                Code += $"int {wire.Name}_globalvar";

            Code += "\r\n";
            Code += "void setup(){\r\n";
            foreach (var pin in Pins)
                Code += $"\tpinMode({pin.Name},{pin.ToDirection()});\r\n";
            Code += "}\r\n\r\n";

            Code += $"void loop(){{\r\n";
            foreach (var method in Methods)
                Code += $"\tfn_{method.Name}();\r\n";
            Code += "}\r\n\r\n";
            
            foreach(var method in Methods)
                Code += method.ToCode(target);
        }

        private int CreateMethod(string[] lines, int i)
        {
            var match = rx_method_start.Match(lines[i]);

            var name = match.Groups[1].Value;

            var method = Method.Extract(name, lines.Skip(i).Take(lines.Length - i).ToArray());

            method.Parse();

            Methods.Add(method);

            return method.Lines.Length;
        }

        private void CreateWire(string v)
        {
            var match = rx_wire.Match(v);

            var name = match.Groups[2].Value.Trim();

            var wire = new Wire();

            wire.Name = name;

            Wires.Add(wire);
        }

        private void CreatePin(string v)
        {
            var match = rx_pin.Match(v);

            var number = int.Parse(match.Groups[2].Value.Trim());
            var direction = match.Groups[3].Value.Trim() == "IN" ? Pin.DirectionEnum.In : Pin.DirectionEnum.Out;
            var name = match.Groups[4].Value.Trim();

            var pin = new Pin();
            pin.Direction = direction;
            pin.Name = name;
            pin.Number = number;

            Pins.Add(pin);
        }

        Regex rx_pin = new Regex(@"(PIN)\s(\d*)\s(IN|OUT|CLK)\s*=\s*(.*);", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        Regex rx_wire = new Regex(@"(WIRE)\s(.*);", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        Regex rx_method_start = new Regex(@"^BEGIN\s+(.*)", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        
        public static Regex rx_comment = new Regex(@"\s+\/\/.*", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
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
