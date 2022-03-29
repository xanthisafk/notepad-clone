using System.Drawing;
using System.Windows.Forms;

namespace NotePad
{
    class DarkMenuRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
            var back = Color.FromArgb(255, 60, 60, 60);
            var selected = Color.FromArgb(255, 100, 100, 100);
            Color c = e.Item.Selected ? selected : back;
            using (SolidBrush brush = new SolidBrush(c))
            {
                e.Graphics.FillRectangle(brush, rc);
                e.Item.ForeColor = Color.White;

            }
        }

    }
}
