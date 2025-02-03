using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GymManagementSystem
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Register register = new Register();
            register.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            LandingPage landingPage = new LandingPage();
            landingPage.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string password = textBox2.Text;    
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Email cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Invalid email format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Person person = new Person();
            person.Email = email;
            person.Password = password;

            string role = person.Login(email, password);

            if (role == "Member")
            {
                this.Hide();
                MemberDashboard memberDashboard = new MemberDashboard();
                memberDashboard.Show();
            }
            else if (role == "Trainer")
            {
                this.Hide();
                TrainerDashboard trainingDashboard = new TrainerDashboard();
                trainingDashboard.Show();
            }
            else if( role == "Admin")
            {
                this.Hide();
                AdminDashboard adminDashboard = new AdminDashboard();   
                adminDashboard.Show();
            }
            else 
            {
                MessageBox.Show("Login failed. Please check your credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
