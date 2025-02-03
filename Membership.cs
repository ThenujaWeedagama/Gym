using System;
using System.Data;
using System.Windows.Forms;

namespace GymManagementSystem
{
    public partial class Membership : Form
    {
     
        
        public Membership(int LoggedInUserID)
        {
            InitializeComponent();
            DisplayMembershipInfo();
        }


        private void DisplayMembershipInfo()
        {
            MembershipClass membershipClass = new MembershipClass();
            var membershipData = membershipClass.GetMembershipInfo(Person.LoggedInUserID);

            if (!string.IsNullOrEmpty(membershipData.membershipID))  
            {
                label7.Text =  membershipData.membershipID;
                label8.Text = membershipData.type;
                label9.Text = membershipData.startDate;
                label10.Text = membershipData.endDate;
                label12.Text = membershipData.status;
            }
            else
            {
                MessageBox.Show("No membership found for this user!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();    
            Membership membership = new Membership(Person.LoggedInUserID);
            membership.Show();  
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string membershipID = textBox1.Text;
            string type = comboBox2.Text;
            string status = comboBox1.Text;

            if (string.IsNullOrEmpty(membershipID))
            {
                MessageBox.Show("Please enter a Membership ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MembershipClass membership = new MembershipClass();
            membership.UpdateMembership(membershipID, type, status);
            DisplayMembershipInfo();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Profile profile = new Profile(Person.LoggedInUserID);
            profile.Show();
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
