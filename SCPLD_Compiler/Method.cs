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

            TRUTH_TABLE A [IN0,IN1,IN2,IN3:OUT1,OUT2] BEGIN
                0.0.1.1:0.1
                0.1.1.0:1.1
                X.X.X.X:0.0
            END TRUTH_TABLE

            FSM A

                STATE[1]:
                    OUT1 = IN1;
                    TRANSISTION[CLK-LOW->HIGH,IN0-LOW->HIGH]->STATE[2];
                STATE[2]:
                    OUT2 = IN2;
                    RELEASE-RETURN; //allow the loop to run other logic, save state and return.
                    RELEASE-RESET->STATE[1]; //allow the loop to run other logic and the reset to state 1
            END FSM A


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
 