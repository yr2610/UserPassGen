using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

public class PasswordForm : Form
{
    private PlaceholderTextBox userNameTextBox;
    private PlaceholderTextBox projectNameTextBox;
    private DateTimePicker expirationDatePicker;
    private Button generateButton;
    private TextBox passwordTextBox;
    private Button copyButton;

    public PasswordForm()
    {
        this.Text = "UserPassGen"; // ウィンドウの名称にプロジェクト名を表示
        this.Size = new System.Drawing.Size(450, 400); // ウィンドウの初期サイズを大きく設定

        projectNameTextBox = new PlaceholderTextBox { PlaceholderText = "プロジェクト名を入力", Top = 10, Left = 10, Width = 400, Font = new System.Drawing.Font("Microsoft Sans Serif", 16F) };
        userNameTextBox = new PlaceholderTextBox { PlaceholderText = "ユーザー名を入力", Top = 60, Left = 10, Width = 400, Font = new System.Drawing.Font("Microsoft Sans Serif", 16F) };
        expirationDatePicker = new DateTimePicker { Top = 110, Left = 10, Width = 400, Font = new System.Drawing.Font("Microsoft Sans Serif", 16F) };
        generateButton = new Button { Text = "パスワード生成", Top = 160, Left = 10, Width = 400, Height = 50, Font = new System.Drawing.Font("Microsoft Sans Serif", 16F) };
        passwordTextBox = new TextBox { Top = 220, Left = 10, Width = 400, ReadOnly = true, Font = new System.Drawing.Font("Microsoft Sans Serif", 16F) };
        copyButton = new Button { Text = "クリップボードにコピー", Top = 280, Left = 10, Width = 400, Height = 50, Font = new System.Drawing.Font("Microsoft Sans Serif", 16F) };

        generateButton.Click += GenerateButton_Click;
        copyButton.Click += CopyButton_Click;

        Controls.Add(projectNameTextBox);
        Controls.Add(userNameTextBox);
        Controls.Add(expirationDatePicker);
        Controls.Add(generateButton);
        Controls.Add(passwordTextBox);
        Controls.Add(copyButton);
    }

    private void GenerateButton_Click(object sender, EventArgs e)
    {
        string projectName = projectNameTextBox.Text;
        string userName = userNameTextBox.Text;
        DateTime expirationDate = expirationDatePicker.Value;

        string password = GeneratePassword(projectName, userName, expirationDate);
        passwordTextBox.Text = password;
    }

    private void CopyButton_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(passwordTextBox.Text))
        {
            Clipboard.SetText(passwordTextBox.Text);
            MessageBox.Show("パスワードがクリップボードにコピーされました。", "コピー完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        else
        {
            MessageBox.Show("コピーするパスワードがありません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private string GeneratePassword(string projectName, string userName, DateTime expirationDate)
    {
        DateTime baseDate = new DateTime(1970, 1, 1);
        int daysSinceBase = (expirationDate - baseDate).Days;
        string hexDays = daysSinceBase.ToString("x4"); // 16進数の4文字で小文字表示

        string input = projectName + userName + hexDays;

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hexDays + hashString.Substring(0, 8); // 有効期限とハッシュ値の一部を結合
        }
    }
}

public static class Program
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new PasswordForm());
    }
}

#if false
// アドイン側でパスワードを検証し、有効期限が過ぎていないかをチェックするコード
using ExcelDna.Integration;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;

public static class AddInManager
{
    [ExcelFunction(Description = "Check if the password is valid")]
    public static bool IsPasswordValid(string password)
    {
        // Excelからログインユーザー名を取得
        string userName = Environment.UserName;

        // Visual Studioのプロジェクト名を取得
        string projectName = Assembly.GetExecutingAssembly().GetName().Name;

        // パスワードから有効期限を抽出
        string hexDays = password.Substring(0, 4);
        int daysSinceBase;
        if (!int.TryParse(hexDays, System.Globalization.NumberStyles.HexNumber, null, out daysSinceBase))
        {
            return false; // 有効期限の形式が不正
        }

        DateTime baseDate = new DateTime(1970, 1, 1);
        DateTime expirationDate = baseDate.AddDays(daysSinceBase);

        // 有効期限が過ぎていないかチェック
        if (DateTime.Now > expirationDate)
        {
            return false; // 有効期限切れ
        }

        // プロジェクト名、ユーザー名と有効期限を連結
        string input = projectName + userName + hexDays;

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            string expectedPassword = hexDays + hashString.Substring(0, 8);

            return password == expectedPassword;
        }
    }
}
#endif
