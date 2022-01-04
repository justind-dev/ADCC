namespace ADCC
{
    public partial class Form1 : Form
    {
        ADM manager = new ADM("domainname.com");
        public Form1()
        {
            InitializeComponent();
            
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}