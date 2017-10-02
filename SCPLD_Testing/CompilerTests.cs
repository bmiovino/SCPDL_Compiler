using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SCPLD_Testing
{
    [TestClass]
    public class CompilerTests
    {
        [TestMethod]
        public void ManualTest()
        {
            Compiler.ConsoleApp.Compiler C = new Compiler.ConsoleApp.Compiler();

            var code = "PIN 1 IN = IN1;\r\n" +
                "PIN 2 OUT = OUT1;\r\n" +
                "\r\n" +
                "BEGIN MAIN\r\n" +
                "TRUTH TABLE [IN1:OUT1]\r\n" +
                "0:1\r\n" + 
                "END TRUTH TABLE\r\n"+
                "END MAIN";

            C.Compile(code, Compiler.ConsoleApp.Compiler.TargetFrameworkEnum.ARV);

           
        }
    }
}
