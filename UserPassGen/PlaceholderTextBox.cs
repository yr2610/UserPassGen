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

    private void ShowPlaceholder()
    {
        isPlaceholderActive = true;
        this.Text = placeholderText;
        this.ForeColor = Color.Gray;
    }

    private void HidePlaceholder()
    {
        isPlaceholderActive = false;
        this.Text = "";
        this.ForeColor = Color.Black;
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        if (!this.Focused && string.IsNullOrEmpty(this.Text))
        {
            ShowPlaceholder();
        }
        else if (isPlaceholderActive && this.Text != placeholderText)
        {
            isPlaceholderActive = false;
            this.ForeColor = Color.Black;
        }
    }
}
