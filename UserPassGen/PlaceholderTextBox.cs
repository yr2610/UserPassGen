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

    protected override void OnCreateControl()
    {
        base.OnCreateControl();
        if (string.IsNullOrEmpty(this.Text))
        {
            ShowPlaceholder();
        }
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
            HidePlaceholder();
        }
    }

    protected override void OnLostFocus(EventArgs e)
    {
        base.OnLostFocus(e);
        if (string.IsNullOrEmpty(this.Text))
        {
            ShowPlaceholder();
        }
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        if (!this.Focused && string.IsNullOrEmpty(this.Text))
        {
            ShowPlaceholder();
        }
        else
        {
            HidePlaceholder();
        }
    }

    private void ShowPlaceholder()
    {
        this.Text = placeholderText;
        this.ForeColor = Color.Gray;
        isPlaceholderActive = true;
    }

    private void HidePlaceholder()
    {
        this.Text = "";
        this.ForeColor = Color.Black;
        isPlaceholderActive = false;
    }
}
