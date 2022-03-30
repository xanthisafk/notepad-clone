using NotePad.Renderers;
using System;
using System.Drawing;
using System.Drawing.Printing;
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

        protected void UpdateRowAndColumn()
        {
            Point pt;
            int line, col, index;

            // get the current line

            index = mainTextBox.SelectionStart;

            line = mainTextBox.GetLineFromCharIndex(index);

            // get the caret position in pixel coordinates

            pt = mainTextBox.GetPositionFromCharIndex(index);

            // now get the character index at the start of the line, and

            // subtract from the current index to get the column

            pt.X = 0;

            col = index - mainTextBox.GetCharIndexFromPosition(pt);

            rowStatus.Text = $"Row: {++line}";
            colStatus.Text = $"Col: {++col}";
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

        protected void PreviewDocuments()
        {
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += doc_PrintPage;
            doc.DocumentName = mainTitle;
            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = doc;
            ppd.ShowDialog();
        }


        protected void PrintDocuments()
        {
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += doc_PrintPage;
            doc.DocumentName = mainTitle;
            printDialog.Document = doc;
            doc.Print();
        }


        /*
         * Events
         */

        private void doc_PrintPage(object sender, PrintPageEventArgs e)
        {
            string stringToPrint = mainTextBox.Text;
            Font font = mainTextBox.Font;
            int charactersOnPage = 0;
            int linesPerPage = 0;

            // Sets the value of charactersOnPage to the number of characters
            // of stringToPrint that will fit within the bounds of the page.
            e.Graphics.MeasureString(stringToPrint, font,
                e.MarginBounds.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            // Draws the string within the bounds of the page
            e.Graphics.DrawString(stringToPrint, font, Brushes.Black,
                e.MarginBounds, StringFormat.GenericTypographic);

            // Remove the portion of the string that has been printed.
            stringToPrint = stringToPrint.Substring(charactersOnPage);

            // Check to see if more pages are to be printed.
            e.HasMorePages = (stringToPrint.Length > 0);
        }

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
                UpdateRowAndColumn();
            }

        }

        private void mainTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            UpdateRowAndColumn();
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

            //// Tool tip searators
            //toolStripSeparator1.BackColor = control;
            //toolStripSeparator2.BackColor = control;
            //toolStripSeparator3.BackColor = control;

            //toolStripSeparator1.ForeColor = controlDark;
            //toolStripSeparator2.ForeColor = controlDark;
            //toolStripSeparator3.ForeColor = controlDark;

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

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = printDialog.ShowDialog();
            if (res == DialogResult.Cancel)
            {
                return;
            }

            else if (res == DialogResult.OK)
            {
                PrintDocuments();
            }



        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {


            if (mainTextBox.SelectedText == "")
            {
                if (boldToolStripMenuItem.Checked)
                {
                    boldToolStripMenuItem.Checked = false;
                    mainTextBox.Font = new Font(mainTextBox.Font, FontStyle.Regular);
                }
                else
                {
                    boldToolStripMenuItem.Checked = true;
                    mainTextBox.Font = new Font(mainTextBox.Font, FontStyle.Bold);
                }
                return;
            }

            if (mainTextBox.SelectionFont.Bold)
            {
                mainTextBox.SelectionFont = new Font(mainTextBox.Font, FontStyle.Regular);
            }
            else
            {
                mainTextBox.SelectionFont = new Font(mainTextBox.Font, FontStyle.Bold);
            }
        }

        private void italicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainTextBox.SelectedText == "")
            {
                if (italicToolStripMenuItem.Checked)
                {
                    italicToolStripMenuItem.Checked = false;
                    mainTextBox.Font = new Font(mainTextBox.Font, FontStyle.Regular);
                }
                else
                {
                    italicToolStripMenuItem.Checked = true;
                    mainTextBox.Font = new Font(mainTextBox.Font, FontStyle.Italic);
                }
                return;
            }

            if (mainTextBox.SelectionFont.Italic)
            {
                mainTextBox.SelectionFont = new Font(mainTextBox.Font, FontStyle.Regular);
            }
            else
            {
                mainTextBox.SelectionFont = new Font(mainTextBox.Font, FontStyle.Italic);
            }
        }

        private void underlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mainTextBox.SelectedText == "")
            {
                if (underlineToolStripMenuItem.Checked)
                {
                    underlineToolStripMenuItem.Checked = false;
                    mainTextBox.Font = new Font(mainTextBox.Font, FontStyle.Regular);
                }
                else
                {
                    underlineToolStripMenuItem.Checked = true;
                    mainTextBox.Font = new Font(mainTextBox.Font, FontStyle.Underline);
                }
                return;
            }

            if (mainTextBox.SelectionFont.Underline)
            {
                mainTextBox.SelectionFont = new Font(mainTextBox.Font, FontStyle.Regular);
            }
            else
            {
                mainTextBox.SelectionFont = new Font(mainTextBox.Font, FontStyle.Underline);
            }
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreviewDocuments();
        }
    }
}
