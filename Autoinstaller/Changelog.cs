using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoinstaller
{
    public partial class Changelog : Form
    {
        public Changelog()
        {
            InitializeComponent();

            //Changelog 
            // 03.10.2016
            // - create Project
            //19.10.2016 
            // - add new search function for the text
            // - add options on startform
            // - add Changelog form
            listBox1.Items.Add("- - - -03.10.2016- - - -");
            listBox1.Items.Add("- create Project");
            listBox1.Items.Add("- add basic stuff");
            listBox1.Items.Add(" ");
            listBox1.Items.Add("- - - -19.10.2016- - - -");
            listBox1.Items.Add("- add new search function for the text");
            listBox1.Items.Add("- add options on startform");
            listBox1.Items.Add("- add Changelog form");
            listBox1.Items.Add(" ");
            listBox1.Items.Add("- - - -03.11.2016- - - -");
            listBox1.Items.Add("- add log file writer");
            listBox1.Items.Add("- scan installer and check if is an installer with exe, msi end");
            listBox1.Items.Add(" ");
            listBox1.Items.Add("- - - -12.11.2016- - - -");
            listBox1.Items.Add("- Create Install directory, if it does not exist");
            listBox1.Items.Add("- remove bug from double click on checkboxes");
            listBox1.Items.Add(" ");
            listBox1.Items.Add("- - - -27.03.2017- - - -");
            listBox1.Items.Add("- add install routine with timer");

            listBox1.Items.Add("- - - -TODO- - - -");
            listBox1.Items.Add("- ");
            listBox1.Items.Add("- ");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
