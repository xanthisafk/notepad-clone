using System.Drawing;
using System.Windows.Forms;

namespace NotePad.Renderers
{
    class LightMenuRenderer : ToolStripProfessionalRenderer
    {
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
            var back = SystemColors.Control;
            var selected = SystemColors.MenuHighlight;
            Color c = e.Item.Selected ? selected : back;
            using (SolidBrush brush = new SolidBrush(c))
            {
                e.Graphics.FillRectangle(brush, rc);
                e.Item.ForeColor = SystemColors.ControlText;
            }
        }
    }
}
