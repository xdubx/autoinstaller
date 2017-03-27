using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Autoinstaller
{
    public partial class Install : Form
    {
        public Install(ListBox listBox2, bool pcdown, bool desktopadd, bool installog)
        {
            this.listBox2 = listBox2;
            Desktopadd = desktopadd;
            pc_down = pcdown;
            install_log = installog;
            InitializeComponent();
        }
        // private vars
        private ListBox listBox2;
        private bool Desktopadd = true;
        private bool pc_down = false;
        private bool install_log = false;
        private static bool timeBool = false;
        private static System.Timers.Timer aTimer;



        // stuff to scan buttons 
        private const int BM_CLICK = 0x00F5;
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr hwnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        // find window
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Auto)]
        static extern IntPtr FindWindowEx1(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        // forground fix
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(int hWnd, StringBuilder title, int size);


        //TODO export for xml -> 
        //TODO 
        // int lists for check
        string[] installqueue = new string[] { "Show", "Mehr", "anzeigen", "install", "progress" };
        string[] buttons = new string[] { "Next", "Weiter", "Contine" };
        // add options - 
        string[] options_checkbox = new string[] { "Shortcut", "Papierkorb", "Recycle Bin", "Standard", "Search", "Such", "Browser" };
        string[] accept_checkbox = new string[] { "Akzeptiere", "accept" };
        string[] install = new string[] { "Install"};
        // end lists
        string[] endpopups = new string[] { "Run", "Show", "Shutdown", "Restart", "View" };
        string[] finish = new string[] { "Finish", "Fertig", "Finish", "Fertig" };

        // Show Listbox
        private void button2_Click(object sender, EventArgs e)
        {
            InstalltheProgramms();
        }

        // start install 
        private void InstalltheProgramms()
        {
            int x = listBox2.Items.Count;
            int y = x;

            timer1.Start();
            for (int i = 0; i < x; i++)
            {
                FileWriter("Noch zu installierende Programme: " + y);
                label1.Text = "Noch zu installierende Programme: " + y;
                // Prepare the process to run
                ProcessStartInfo start = new ProcessStartInfo();
                // Enter in the command line arguments, everything you would enter after the executable name itself
                // start.Arguments = arguments;
                // Enter the executable to run, including the complete path
                start.FileName = System.Environment.CurrentDirectory + @"\Installer\" + listBox2.Items[i].ToString();
                // Do you want to show a console window?
                start.WindowStyle = ProcessWindowStyle.Normal;
                start.CreateNoWindow = true;
                int exitCode;
                // Run the external process & wait for it to finish
                // MessageBox.Show(WinGetHandle("Datei öffnen").ToString());

                using (Process proc = Process.Start(start))
                {
                    //Consolen ausgabe

                    System.Threading.Thread.Sleep(5000);
                    listBox1.Items.Add("Installiere " + proc.MainWindowTitle.ToString());
                    Refresh();

                    string installname = proc.MainWindowTitle.ToString();

                    int a = 0;
                    while (a < 1)
                    {

                        if (proc.MainWindowTitle == "")
                        {
                            a = 1;
                            listBox1.Items.Add("Installation wurde unterbrochen!");
                            break;
                        }
                        // TODO Language Support

                        //check language


                        
                        bool add = false;
                        bool accept = false;
                        bool firstwinodw_install_button = pressButtons(proc.MainWindowHandle, install, false);
                        // scan for buttons and progress
                        if (pressButtons(proc.MainWindowHandle, buttons, false))
                        {
                            // maybe for more options to check for advertisment?
                            // check advertisement
                            if (pressCheckbox(proc.MainWindowHandle, options_checkbox, false)){
                                add = true;
                                pressCheckbox(proc.MainWindowHandle, options_checkbox, true);
                            }
                            // press accept button
                            if (!accept) { 
                                if (pressCheckbox(proc.MainWindowHandle, accept_checkbox, false)){
                                    accept = true;
                                    pressCheckbox(proc.MainWindowHandle, accept_checkbox, true);
                                }
                            }

                            // press next after check all 
                            pressButtons(proc.MainWindowHandle, buttons, true);
                        }
                        // if have the install option press install
                        if (pressButtons(proc.MainWindowHandle, install, false))
                        {

                            // check advertisement
                            if (pressCheckbox(proc.MainWindowHandle, options_checkbox, false))
                            {
                                add = true;
                                pressCheckbox(proc.MainWindowHandle, options_checkbox, true);
                            }
                            // press accept button
                            if (!accept)
                            {
                                if (pressCheckbox(proc.MainWindowHandle, accept_checkbox, false))
                                {
                                    accept = true;
                                    pressCheckbox(proc.MainWindowHandle, accept_checkbox, true);
                                }
                            }

                        }
                        // wait for install

                        while (!timeBool)
                        {
                            Console.WriteLine("waiting");
                        }

                        aTimer.Stop();
                        aTimer.Dispose();
                        //// switch window handle 
                        //IntPtr lol = WinGetHandle(installname);

                        //// search for checkbox
                        //pressCheckbox(lol, options_checkbox);
                        ////Install

                        //Next:
                        //Boolean accept = pressButtons(lol, install_checkbox);

                        //if (!accept)
                        //{
                        //    goto Next;
                        //}


                       


                        //waitforInstall(lol, installqueue);
                        //pressCheckbox(lol, endpopups);
                        //Boolean ac = pressButtons(lol, finish);
                        //    if (ac == true)
                        //    {
                        //        a = 1;
                        //    }

                    }

                    proc.WaitForExit();
                    // Retrieve the app's exit code
                    exitCode = proc.ExitCode;
                    y--;
                    label1.Text = "Noch zu installierende Programme: " + y;
                }


            }
            listBox1.Items.Add("Installer fertig!");
            FileWriter("Installer fertig!");

        }
        // press buttons & checkbox
        private Boolean pressButtons(IntPtr proc, string[] buttons, bool scan) {
            List<process> test = new List<process>();
            Boolean back = false;
            try
            {
                test = GetAllChildrenWindowHandles(proc, 100);
                foreach (var element in test)
                {

                }
            }
            catch (Exception)
            {

            }
            for (int j = 0; j < test.Count; j++)
            {
                StringBuilder title = new StringBuilder(256);
                GetWindowText(test[j].subproc.ToInt32(), title, 256);
                this.listBox1.Items.Add(title);
                Refresh();
                foreach (string elements in buttons)
                {
                    if (title.ToString() != "") {
                        if (title.ToString().IndexOf(elements, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            if (!scan)
                            {
                                press(title, proc, test[j]);
                            }
                            back = true;
                        }
                    }
                }

            }

            return back;
        }
        private Boolean pressCheckbox(IntPtr proc, string[] checkbox, bool scan)
        {
            bool back = false;
            List<process> test = new List<process>();
            try
            {
                test = GetAllChildrenWindowHandles(proc, 100);
            }
            catch (Exception)
            {

            }
            //check for checkbox
            for (int j = 0; j < test.Count; j++)
            {
                List<process> underwindow = GetAllChildrenWindowHandles(test[j].subproc, 100);
                for (int h = 0; h < underwindow.Count; h++)
                {
                    StringBuilder title = new StringBuilder(256);
                    GetWindowText(underwindow[h].subproc.ToInt32(), title, 256);
                    //MessageBox.Show(" " + title);
                    if (title.ToString() != "")
                    {
                        foreach (string elements in checkbox)
                        {
                            if (title.ToString().IndexOf(elements, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                if (scan)
                                {
                                    press(title, proc, underwindow[h]);
                                }
                                back = true;
                            }
                        }
                    }
                }
            }
            return back;
        }
        private Boolean waitforInstall(IntPtr proc, string[] buttons)
        {
            List<process> test = new List<process>();
            Boolean back = false;

            //while loop for wait
            int a = 0;
            while (a == 1)
            {
                try
                {
                    test = GetAllChildrenWindowHandles(proc, 100);
                }
                catch (Exception)
                {

                }

                for (int j = 0; j < test.Count; j++)
                {
                    StringBuilder title = new StringBuilder(256);
                    GetWindowText(test[j].subproc.ToInt32(), title, 256);
                    foreach (string elements in buttons)
                    {
                        if (title.ToString().IndexOf(elements, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            MessageBox.Show("Wait");
                            System.Threading.Thread.Sleep(5000);

                            back = true;
                            continue;
                        }
                        else
                        {
                            a = 1;
                        }
                    }
                }
            }
            return back;
        }

        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            timeBool = true;
          
        }


        //press 
        private void press(StringBuilder title, IntPtr proc, process test)
        {
            if (test.clicked == 0)
            {
                FileWriter("Press " + title.ToString());
                System.Threading.Thread.Sleep(2000);
                // Button click
                SetForegroundWindow(proc);
                SendMessage(test.subproc, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                test.clicked = 1;
            }
        }
        // log
        private void FileWriter(string log)
        {
            log = "[ " + System.DateTime.Today + " ]" + log + "\b";
            System.IO.File.WriteAllText(System.Environment.CurrentDirectory + @"\Log.txt", log);
        }

        // get window handle to list
        private static List<process> GetAllChildrenWindowHandles(IntPtr hParent, int maxCount)
        {
            List<process> parts = new List<process>();
            int ct = 0;
            IntPtr prevChild = IntPtr.Zero;
            IntPtr currChild = IntPtr.Zero;
            while (true && ct < maxCount)

            {
                if (hParent.ToInt32() == null)
                {


                }

                currChild = FindWindowEx1(hParent, prevChild, null, null);
                if (currChild == IntPtr.Zero) break;
                //MessageBox.Show(currChild.ToString("X"));
                parts.Add(new process() { subproc = currChild, clicked = 0 });
                prevChild = currChild;
                ++ct;
            }
            return parts;
        }

        public static IntPtr WinGetHandle(string wName) {
            IntPtr hWnd = default(IntPtr);
            foreach (Process pList in Process.GetProcesses())
            {
                if (pList.MainWindowTitle.Contains(wName))
                {
                    hWnd = pList.MainWindowHandle;
                }
            }
            return hWnd;
        }
    }


     public class process {
            public IntPtr subproc { get; set; }
            public int clicked { get; set; }
        }

}
