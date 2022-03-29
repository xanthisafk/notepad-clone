using System.Windows.Forms;

namespace NotePad
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void logoAttr_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.flaticon.com/free-icons/font");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/xanthisafk");
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
