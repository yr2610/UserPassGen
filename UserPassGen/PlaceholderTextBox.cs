using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

public class PlaceholderTextBox : TextBox
{
    private string placeholderText;
    private bool isPlaceholderActive = true;

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

        if (isPlaceholderActive && !string.IsNullOrEmpty(this.PlaceholderText))
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
        if (isPlaceholderActive)
        {
            this.Text = "";
            isPlaceholderActive = false;
            this.ForeColor = Color.Black;
        }
    }

    protected override void OnLostFocus(EventArgs e)
    {
        base.OnLostFocus(e);
        if (string.IsNullOrEmpty(this.Text))
        {
            isPlaceholderActive = true;
            this.ForeColor = Color.Gray;
            Invalidate();
        }
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        if (!this.Focused && string.IsNullOrEmpty(this.Text))
        {
            isPlaceholderActive = true;
            this.ForeColor = Color.Gray;
            Invalidate();
        }
        else
        {
            isPlaceholderActive = false;
            this.ForeColor = Color.Black;
        }
    }
}
