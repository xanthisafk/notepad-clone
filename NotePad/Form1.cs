using NotePad.Renderers;
using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NotePad
{
    public partial class Form1 : Form

    {
        /*
         * Declarations
         */


        //Title
        public string mainTitle = "Untitled";

        //File Stream and related flags
        string saveFilePath = null;
        bool isSaved = false;


        // Constructor
        public Form1()
        {
            InitializeComponent();
            topMenu.Renderer = new LightMenuRenderer();
        }

        /*
         * Utilities
         */

        // Update Title
        protected void UpdateTitle()
        {
            this.Text = mainTitle + " - Text Editor";
        }

        protected void NewPage()
        {
            mainTextBox.Text = "";
            mainTitle = "Untitled";
            UpdateTitle();
            statusHappen.Text = "Create new page.";
        }

        protected void SaveFile()
        {
            if (saveFilePath == null)
            {
                SaveAsFile();
                return;
            }

            FileStream fs = new FileStream(saveFilePath, FileMode.Create);
            byte[] writeArr = Encoding.UTF8.GetBytes(mainTextBox.Text);
            fs.Write(writeArr, 0, writeArr.Length);
            string fileName = Path.GetFileName(saveFilePath);
            saveFilePath = fileName;
            UpdateTitle();
            statusHappen.Text = $"Success save as {fileName}.";
            isSaved = true;
            fs.Close();

        }

        protected bool SaveAsFile()
        {

            // Create a new Save File Dialog.
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Documents|*.txt";
            sfd.Title = "Save file";
            sfd.FileName = mainTitle;

            // If saving was cancelled.
            if (sfd.ShowDialog() == DialogResult.Cancel)
            {
                return false;
            }

            FileStream fs = (FileStream)sfd.OpenFile();
            byte[] writeArr = Encoding.UTF8.GetBytes(mainTextBox.Text);
            fs.Write(writeArr, 0, writeArr.Length);
            string fileName = Path.GetFileName(sfd.FileName);
            saveFilePath = fileName;
            mainTitle = fileName;
            UpdateTitle();
            statusHappen.Text = $"Success save as {fileName}.";
            isSaved = true;
            fs.Close();
            return true;

        }

        protected void OpenFileDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Documents|*.txt";
            ofd.Title = "Open file";

            if (ofd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            FileStream file = new FileStream(ofd.FileName, FileMode.Open);
            saveFilePath = ofd.FileName;

            using (StreamReader reader = new StreamReader(file))
            {
                mainTextBox.Text = reader.ReadToEnd();
            }

            mainTitle = ofd.SafeFileName;
            isSaved = true;
            UpdateTitle();
            file.Close();
        }

        protected void ExitApplication()
        {
            if (!isSaved && mainTextBox.Text != "")
            {
                string message = "Do you want to save this document before exiting?";
                string title = "Are you sure?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Cancel)
                {
                    return;
                }
                else if (result == DialogResult.No)
                {
                    Application.Exit();
                }
                else
                {
                    if (SaveAsFile())
                    {
                        Application.Exit();
                    }
                }
            }
            else
            {
                Application.Exit();
            }
        }

        /*
         * Events
         */

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateTitle();
        }

        // All KeyDown Events
        private void KeyDown_Events(object sender, KeyEventArgs e)
        {

            // If text is added
            if (sender == mainTextBox)
            {
                isSaved = false;
            }

        }

        private void toggleDarkModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dynamic text;
            dynamic control;
            dynamic controlDark;
            dynamic inp;
            dynamic renderer;
            string statusText;
            if (toggleDarkModeToolStripMenuItem.Checked)
            {
                toggleDarkModeToolStripMenuItem.Checked = false;
                renderer = new LightMenuRenderer();


                control = System.Drawing.SystemColors.Control;
                controlDark = System.Drawing.SystemColors.ControlDark;

                text = System.Drawing.SystemColors.ControlText;
                inp = System.Drawing.Color.White;

                statusText = "Toggle Light mode.";
            }
            else
            {
                toggleDarkModeToolStripMenuItem.Checked = true;
                renderer = new DarkMenuRenderer();

                controlDark = System.Drawing.Color.FromArgb(255, 33, 33, 33);
                control = System.Drawing.Color.FromArgb(255, 60, 60, 60);
                text = System.Drawing.Color.White;
                inp = System.Drawing.Color.FromArgb(255, 33, 33, 33);

                statusText = "Toggle Dark mode.";

            }

            // Main text box
            mainTextBox.BackColor = inp;
            mainTextBox.ForeColor = text;

            // Top menu bar
            topMenu.BackColor = control;
            topMenu.ForeColor = text;

            // Bottom Status bar
            status.BackColor = control;
            status.ForeColor = text;

            // Tool tip searators
            toolStripSeparator1.BackColor = control;
            toolStripSeparator2.BackColor = control;
            toolStripSeparator3.BackColor = control;

            toolStripSeparator1.ForeColor = controlDark;
            toolStripSeparator2.ForeColor = controlDark;
            toolStripSeparator3.ForeColor = controlDark;

            topMenu.Renderer = renderer;
            statusHappen.Text = statusText;

        }

        private void dateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = mainTextBox.SelectionStart;
            DateTime d = DateTime.Now.Date;
            string date = $"{d.Day}/{d.Month}/{d.Year}";
            mainTextBox.Text += date;
            mainTextBox.SelectionStart = i + date.Length;
            statusHappen.Text = "Insert date.";
        }

        private void timeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = mainTextBox.SelectionStart;
            TimeSpan t = DateTime.Now.TimeOfDay;
            string time = $"{t.Hours}:{t.Minutes}:{t.Seconds}";
            mainTextBox.Text += time;
            mainTextBox.SelectionStart = i + time.Length;
            statusHappen.Text = "Insert time.";
        }

        private void dateTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = mainTextBox.SelectionStart;
            DateTime d = DateTime.Now.Date;
            string date = $"{d.Day}/{d.Month}/{d.Year}";
            TimeSpan t = DateTime.Now.TimeOfDay;
            string time = $"{t.Hours}:{t.Minutes}:{t.Seconds}";
            string dt = date + " " + time;
            mainTextBox.Text += dt;
            mainTextBox.SelectionStart = i + dt.Length;
            statusHappen.Text = "Insert date and time.";
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainTextBox.Text != "" && !isSaved)
            {
                string message = "Do you want to save this document before creating a new one?";
                string title = "Are you sure?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Cancel)
                {
                    return;
                }
                else if (result == DialogResult.No)
                {
                    NewPage();
                }
                else
                {
                    if (SaveAsFile())
                    {
                        NewPage();
                    }
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainTextBox.Text != "" && !isSaved)
            {
                string message = "Do you want to save this document before opening a new one?";
                string title = "Are you sure?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Cancel)
                {
                    return;
                }
                else if (result == DialogResult.No)
                {
                    OpenFileDialog();
                }
                else
                {
                    if (SaveAsFile())
                    {
                        OpenFileDialog();
                    }
                }
            }
            else
            {
                OpenFileDialog();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFilePath != null)
            {
                SaveFile();
                isSaved = true;
                return;
            }

            if (SaveAsFile())
            {
                statusHappen.Text = "Saved";
                isSaved = true;
            }

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new About().Show();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAsFile();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ExitApplication();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(mainTextBox.SelectedText);
            mainTextBox.SelectedText = "";
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(mainTextBox.SelectedText);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTextBox.Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTextBox.SelectedText = "";
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainTextBox.SelectAll();
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (wordWrapToolStripMenuItem.Checked)
            {
                mainTextBox.WordWrap = false;
                wordWrapToolStripMenuItem.Checked = false;
            }
            else
            {
                mainTextBox.WordWrap = true;
                wordWrapToolStripMenuItem.Checked = true;
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontsDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            mainTextBox.Font = fontsDialog.Font;

        }
    }
}
