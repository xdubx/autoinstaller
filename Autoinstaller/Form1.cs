using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoinstaller
{
    public partial class Form1 : Form
    {
        public List<string> files;

        public Form1()
        {
            InitializeComponent();
            // Search for Installer
            files = getInstaller();
            // Add Filenames to Listbox1 
            foreach (string element in files)
            {
                this.listBox1.Items.Add(Path.GetFileName(element));
            }
            // select box 3 options
            listBox3.SetSelected(0, true);
        }

        // Open next form and give List to other var 
        private void button1_Click(object sender, EventArgs e)
        {
             if (listBox2.Items.Count == 0)
                {
                    MessageBox.Show("Bitte Installer auswählen");
                }
                else
                {

                    Install install = new Install(listBox2, cBpcDown.Checked, cBdeskShort.Checked, cBinstallLog.Checked);
                    install.Show();
                    Hide();
                }
            
        }

        // Add to listbox 2 and Remove from listbox1 
        private void button2_Click(object sender, EventArgs e)
        {

            string selected = "";
            int stelle = 0;
            try
            {
                stelle = listBox1.SelectedIndex;
                selected = listBox1.SelectedItem.ToString();
            }
            catch (NullReferenceException)
            {
                try
                {
                    listBox1.SetSelected(0, true);
                }
                catch (ArgumentOutOfRangeException)
                {

                }   
            }
            if (!selected.Equals(""))
            {
                listBox1.Items.Remove(selected);
                listBox2.Items.Add(selected);
                
                if (stelle == 0)
                {
                    try
                    {
                        listBox1.SetSelected(stelle, true);
                    }
                    catch(ArgumentOutOfRangeException)
                    {

                    } 
                }
                else
                {
                    stelle--;
                    listBox1.SetSelected(stelle, true);
                }
            }else{

            }

        }
        
        // Add to listbox 1 and Remove from listbox2 
        private void button3_Click(object sender, EventArgs e)
        {

            string selected = "";
            int stelle = 0;
            try
            {
                stelle = listBox2.SelectedIndex;
                selected = listBox2.SelectedItem.ToString();
            }
            catch (NullReferenceException)
            {
                try
                {
                    listBox2.SetSelected(0, true);
                }
                catch (ArgumentOutOfRangeException)
                {

                }
            }
            if (!selected.Equals(""))
            {
                listBox2.Items.Remove(selected);
                listBox1.Items.Add(selected);

                if (stelle == 0)
                {
                    try
                    {
                        listBox2.SetSelected(stelle, true);
                    }
                    catch (ArgumentOutOfRangeException)
                    {

                    }
                }
                else
                {
                    stelle--;
                    listBox2.SetSelected(stelle, true);
                }
            }
            else
            {

            }

        }

        //Reset Button
        private void button4_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            listBox1.Items.Clear();
            listBox3.SetSelected(0, true);
            cBpcDown.Checked = false;
            cBdeskShort.Checked = false;
            cBinstallLog.Checked = false;
            addInstalltoBox1(files);
          
        }
       
        // Extra functions // 

        // get Path and the exe
        private List<string> getInstaller()
        {

           List<string> file = new List<string> { };
            try
            {
                // Get Installpath items
                string [] fileEntries = System.IO.Directory.GetFiles(System.Environment.CurrentDirectory + @"\Installer");

                foreach (string elements in fileEntries)
                {
                    if (elements.Contains("exe") || elements.Contains("msi"))
                    {
                        file.Add(elements);
                    }
                }

            }
            catch (System.IO.DirectoryNotFoundException)
            {

                System.Windows.Forms.MessageBox.Show(@"Hinweis:",@" Verzeichnis  '\Installer' wurde nicht gefunden und daher angelegt!");
                Directory.CreateDirectory("Installer");

            }
            return file;
        }

        // add Items to Installbox1
        private void addInstalltoBox1(List<string> fileEntries)
        {
            foreach (string element in fileEntries)
            {
                this.listBox1.Items.Add(Path.GetFileName(element));
            }

        }

        // open changelog
        private void button5_Click(object sender, EventArgs e)
        {
            Changelog show = new Changelog();
            show.Show();
        }
    }
}
