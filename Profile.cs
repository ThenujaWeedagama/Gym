using System;
using System.Diagnostics;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace GymManagementSystem
{
    public partial class Profile : Form
    {
        private int userID;
        public Profile(int LoggedInUserID)
        {
            InitializeComponent();
            userID = LoggedInUserID;
            Debug.WriteLine($"Calling this ID: {userID}");
            LoadProfileData();
        }


        private void LoadProfileData()
        {
            Person person = new Person();
            Person profileData = person.ViewProfile(userID);

            if (profileData != null)
            {
               label10.Text = profileData.UserID.ToString();
               label11.Text = profileData.Name;
               label12.Text = profileData.Email;
               label13.Text = profileData.PhoneNo;
            }
            else
            {
                MessageBox.Show("Profile not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Profile profile = new Profile(Person.LoggedInUserID);
            Debug.WriteLine("profile button clicked");
            profile.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Membership membership = new Membership(Person.LoggedInUserID);
            membership.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            MemberDashboard memberDashboard = new MemberDashboard();
            memberDashboard.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LandingPage landingPage = new LandingPage();
            landingPage.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string password = textBox2.Text;

            DialogResult result = MessageBox.Show(
               "Are you sure you want to delete your account? This action cannot be undone.",
               "Confirm Deletion",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Warning
           );

            if (result == DialogResult.Yes)
            {
                Person person = new Person();
                bool isDeleted = person.DeleteAccount(email, password);

                if (isDeleted)
                {
                    MessageBox.Show("Account deleted. Closing application.");
                    Application.Exit(); 
                }
            }
        }

        private void Profile_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {

            Person person = new Person();
            Person profileData = person.ViewProfile(userID);

            if (profileData != null)
            {
                textBox3.Text = profileData.UserID.ToString();
                textBox4.Text = profileData.Name;
                textBox5.Text = profileData.Email;
                textBox6.Text = profileData.PhoneNo;
            }


            panel4.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string Name = textBox3.Text;
            string Email = textBox4.Text;
            string password = textBox5.Text;
            string phoneNo = textBox6.Text;

            Person person = new Person();
            person.UpdateProfile(Name, Email, phoneNo, password);

            panel4.Visible = false;
        }
    }
}
