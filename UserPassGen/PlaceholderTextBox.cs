using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class PlaceholderTextBox : TextBox
{
    private string placeholderText;

    [Category("Placeholder")]
    [Description("The placeholder text that is displayed when the textbox is empty.")]
    public string PlaceholderText
    {
        get { return placeholderText; }
        set { placeholderText = value; Invalidate(); }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        if (string.IsNullOrEmpty(this.Text) && !string.IsNullOrEmpty(this.PlaceholderText))
        {
            using (Brush brush = new SolidBrush(Color.Gray))
            {
                e.Graphics.DrawString(this.PlaceholderText, this.Font, brush, new PointF(0, 0));
            }
        }
    }

    protected override void OnGotFocus(EventArgs e)
    {
        base.OnGotFocus(e);
        this.Invalidate();
    }

    protected override void OnLostFocus(EventArgs e)
    {
        base.OnLostFocus(e);
        this.Invalidate();
    }
}
