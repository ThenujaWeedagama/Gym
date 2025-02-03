using System;
using System.Data;
using System.Windows.Forms;

namespace GymManagementSystem
{
    public partial class AssignTrainer : Form
    {
        public AssignTrainer()
        {
            InitializeComponent();
            LoadClasses();
            LoadTrainerSchedules();
        }

        private void LoadClasses()
        {
            GymClass gymClass = new GymClass();
            DataTable dt = gymClass.GetClassWithoutTrainers();
            dataGridView1.DataSource = dt;
        }

        private void LoadTrainerSchedules()
        {
            GymClass gymClass = new GymClass();
            DataTable trainerSchedules = gymClass.GetTrainerSchedules();

            if (trainerSchedules.Rows.Count == 0)
            {
                // Display message if no trainers are assigned
                DataTable emptyTable = new DataTable();
                emptyTable.Columns.Add("Message", typeof(string));
                emptyTable.Rows.Add("No trainers found with schedules.");
                dataGridView2.DataSource = emptyTable;
            }
            else
            {
                dataGridView2.DataSource = trainerSchedules;
              
            }
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
            this.Refresh();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string classID = textBox2.Text;
            string trainerID = textBox1.Text;

            // Validate the inputs
            if (string.IsNullOrWhiteSpace(classID) || string.IsNullOrWhiteSpace(trainerID))
            {
                MessageBox.Show("Please enter both Class ID and Trainer ID.");
                return;
            }

            // Create an instance of GymClass
            GymClass gymClass = new GymClass();

            // Assign trainer to the class
            gymClass.AssignTrainerToClass(classID, trainerID);


        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminMembership adminMembership = new AdminMembership();
            adminMembership.Show();

        }

        private void AssignTrainer_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
