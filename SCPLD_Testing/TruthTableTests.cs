using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Compiler.ConsoleApp;
using System.Collections.Generic;

namespace SCPLD_Testing
{
    [TestClass]
    public class TruthTableTests
    {
        [TestMethod]
        public void AssignmentMethodTest()
        {
            TruthTable T = new TruthTable();

            List<string> outs = new List<string>()
            {
                "OUT1",
                "OUT2"
            };

            T.Outputs = outs;

            var code = T.Assignment("0,0,0,1:1,0");

            Assert.AreEqual("\tOUT1_localvar = 1;\r\n" + "\tOUT2_localvar = 0;\r\n", code);
        }

        [TestMethod]
        public void ConditionMethodTest()
        {
            TruthTable T = new TruthTable();

            List<string> ins = new List<string>()
            {
                "IN1",
                "IN2"
            };

            T.Inputs = ins;

            var code = T.CodeCondition("1,1:1,0");

            Assert.AreEqual("IN1_localvar == 1 && IN2_localvar == 1", code);
        }

        [TestMethod]
        public void ParseMethodTest()
        {
            TruthTable T = new TruthTable();

            string[] lines = new string[] {
                "//gibberish",
                "TRUTH TABLE [IN1,IN2,IN3:OUT1,OUT2]",
                "0,0,0:1,1",
                "default:0,1",
                "END TRUTH TABLE",
                "//more gibberish"
            };

            var linenumber = T.BlockParse(1, lines);

            Assert.AreEqual(5, linenumber);

            var code = T.ToCode();

            Assert.AreEqual("IN1_localvar == 1 && IN2_localvar == 1", T.ToCode());
        }

    }
}
