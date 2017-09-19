using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Compiler.ConsoleApp;
using System.Linq;

namespace SCPLD_Testing
{
    [TestClass]
    public class MethodTests
    {
        [TestMethod]
        public void MethodExtractTest()
        {
            string[] lines = {
                "BEGIN MAIN",
                "   OUT1 = IN1;",
                "END MAIN"
            };

            var method = Method.Extract("MAIN",lines);

            Assert.AreEqual(3, method.Lines.Length);

            CollectionAssert.AreEqual(lines, method.Lines);

        }

        [TestMethod]
        public void MethodExtractParse()
        {
            string[] lines = {
                "BEGIN MAIN",
                "   OUT1 = IN1;",
                "END MAIN"
            };

            var method = Method.Extract("MAIN", lines);

            method.Parse();

            Assert.AreEqual(3, method.Commands.Count);
            Assert.AreEqual(Command.CommandType.BlockScope, method.Commands[0].Type);
            Assert.AreEqual(Command.CommandType.Assignment, method.Commands[1].Type);
            Assert.AreEqual(Command.CommandType.BlockScope, method.Commands[2].Type);
            Assert.AreEqual("OUT1", method.Commands[1].Parameters[0]);
            Assert.AreEqual("IN1", method.Commands[1].Parameters[1]);
            Assert.AreEqual(Command.CommandType.BlockScope, method.Commands[0].Type);
            Assert.AreEqual(Command.CommandType.BlockScope, method.Commands[2].Type);
            
        }

        [TestMethod]
        public void MethodSetPinsAndWires()
        {
            string[] lines = {
                "BEGIN MAIN",
                "   WIRE1 = IN2;",
                "   OUT1 = IN1;",
                "   WIRE2 = IN1&IN2;",
                "END MAIN"
            };

            var method = Method.Extract("MAIN", lines);

            method.Parse();

            method.SetPinsAndWires(new Pin[]
            {
                new Pin() { Name = "IN1" },
                new Pin() { Name = "IN2" },
                new Pin() { Name = "OUT1" },
                new Pin() { Name = "OUT2" }
            });

            var tpins = new string[] {
                "IN1", "IN2", "OUT1" 
            };

            var pnames = method.Pins.Select(p => { return p.Name; }).ToArray();

            CollectionAssert.AreEquivalent(tpins, pnames);

            var twires = new string[] { "WIRE1", "WIRE2" };

            var wnames = method.Wires.Select(w => { return w.Name; }).ToArray();

            CollectionAssert.AreEquivalent(twires, wnames);

        }


        [TestMethod]
        public void MethodToCode()
        {
            string[] lines = {
                "BEGIN MAIN",
                "   WIRE1 = IN2;",
                "   OUT1 = IN1;",
                "   WIRE2 = IN1&IN2;",
                "END MAIN"
            };

            var method = Method.Extract("MAIN", lines);

            method.Parse();

            method.SetPinsAndWires(new Pin[]
            {
                new Pin() { Name = "IN1", Direction = Pin.DirectionEnum.In },
                new Pin() { Name = "IN2", Direction = Pin.DirectionEnum.In },
                new Pin() { Name = "OUT1", Direction = Pin.DirectionEnum.Out },
                new Pin() { Name = "OUT2", Direction = Pin.DirectionEnum.Out }
            });

            var code = method.ToCode(Compiler.ConsoleApp.Compiler.TargetFrameworkEnum.ARV);

        }
    }
}
