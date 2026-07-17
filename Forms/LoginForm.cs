using SmartInventory.Data;

namespace SmartInventory.Forms
{
    public partial class LoginForm : Form
    {
        private const string UserName = "admin";
        private const string Password = "superadmin";
        public LoginForm()
        {
            InitializeComponent();
            this.tbxName.Text = UserName;
            this.tbxPassword.Text= Password;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool isExists = MySqlDbHelper.CheckAccount(tbxName.Text.Trim(),
                tbxPassword.Text.Trim());

            //if (!(tbxName.Text.Equals(UserName)
            //    && tbxPassword.Text.Equals(Password)))

            if (!isExists)
            {
                MessageBox.Show("帳號或密碼輸入不正確!", "錯誤");
                return;
            }
            this.DialogResult = DialogResult.OK; // 設定結果
            this.Close(); // 關閉自己，回到 Program.cs 繼續執行
        }
    }
}
