namespace ADCC
{
    public partial class Form1 : Form
    {
        readonly ADManager manager = new("domainname.com");
        public Form1()
        {
            InitializeComponent();
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void btn_unlock_userSAM_Click(object sender, EventArgs e)
        {
            if (textbox_userSAM.Text == "")
            {
                MessageBox.Show("Please enter the sAMAccountName for the user to unlock them.");
                return;
            }
            else
            {
                manager.Unlock(textbox_userSAM.Text);
            }
        }

        private void btn_FindUserDN_Click(object sender, EventArgs e)
        {
            if (textBox_userDNFind.Text == "")
            {
                MessageBox.Show("Please enter the sAMAccountName for the user to find.");
                return;
            }
            else
            {
                var userDistinguishedName = manager.GetDistinguishedName(textBox_userDNFind.Text);
                MessageBox.Show($"User DN is : {userDistinguishedName}");
            }
        }
    }
}