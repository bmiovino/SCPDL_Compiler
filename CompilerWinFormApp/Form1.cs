using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _compiler = Compiler.ConsoleApp.Compiler;

namespace CompilerWinFormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCompile_Click(object sender, EventArgs e)
        {
            _compiler compiler = new _compiler();
            compiler.Compile(txtCode.Text, _compiler.TargetFrameworkEnum.ARV);
            txtCompile.Text = compiler.Code;
        }
    }
}
