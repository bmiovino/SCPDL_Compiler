namespace SCPLD_Compiler
{
    public class Method
    {
        public string Name;
        protected string[] lines;
        protected Wire[] wires; //internal to method
        protected Command[] commands;

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
        END MAIN*/

        //assignment regex

        public void Parse(string name, string[] lines)
        {
            this.Name = name;
            this.lines = lines;

            //parse each one
        }

        //regex for each line type
        //assignment
        //wait
        //signal transistion - with assignment and boolean.
        //block

        public string ToCode()
        {
            return "";
        }

    }
}
 