using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymManagementSystem
{
    public partial class AdminMembership : Form
    {
        public AdminMembership()
        {
            InitializeComponent();
            LoadMemberships();
        }


        private void LoadMemberships()
        {
            MembershipClass membership = new MembershipClass();
            DataTable dt = membership.GetMembershipDetails();
            dataGridView1.DataSource = dt; 
        }


        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            LandingPage landingPage = new LandingPage();
            landingPage.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminDashboard adminDashboard = new AdminDashboard();
            adminDashboard.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            AssignTrainer assignTrainer = new AssignTrainer();
            assignTrainer.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
            LoadMemberships();

        }
    }
}
