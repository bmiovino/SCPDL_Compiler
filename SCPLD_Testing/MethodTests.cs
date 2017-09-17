using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Compiler.ConsoleApp;

namespace SCPLD_Testing
{
    [TestClass]
    public class MethodTests
    {
        [TestMethod]
        public void MethodExtractTest()
        {
            string[] lines = {
                "MAIN BEGIN",
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
                "MAIN BEGIN",
                "   OUT1 = IN1;",
                "END MAIN"
            };

            var method = Method.Extract("MAIN", lines);

            method.Parse();

            Assert.AreEqual(3, method.commands.Count);
            Assert.AreEqual(Command.CommandType.BlockScope, method.commands[0].Type);
            Assert.AreEqual(Command.CommandType.Assignment, method.commands[1].Type);
            Assert.AreEqual(Command.CommandType.BlockScope, method.commands[2].Type);
            Assert.AreEqual("OUT1", method.commands[1].Parameters[0]);
            Assert.AreEqual("IN1", method.commands[1].Parameters[1]);
            Assert.AreEqual(Command.CommandType.BlockScope, method.commands[0].Type);
            Assert.AreEqual(Command.CommandType.BlockScope, method.commands[2].Type);
            
        }
    }
}
