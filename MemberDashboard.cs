using System;
using System.Data;
using System.Windows.Forms;

namespace GymManagementSystem
{
    public partial class MemberDashboard : Form
    {

        public MemberDashboard()
        {
            InitializeComponent();
            dataGridView1.CellContentClick += new DataGridViewCellEventHandler(dataGridView1_CellContentClick);
            dataGridView2.CellContentClick += new DataGridViewCellEventHandler(dataGridView2_CellContentClick);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int trainerID = Person.LoggedInUserID;
            GymClass gymClass = new GymClass();
            DataTable dt = gymClass.GetMemeberClasses(trainerID);

            if (dt.Rows.Count == 0)
            {
                // If no classes, show a message
                label4.Text = "You have not registered for any classes.";
                label4.Visible = true;
            }

            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (!dataGridView1.Columns.Contains("Attendance") && !dataGridView1.Columns.Contains("Unregister"))
            {
                AddTrainerClassButtons();
            }

            panel3.Visible = true;
        }

        private void AddTrainerClassButtons()
        {
            DataGridViewButtonColumn attendanceColumn = new DataGridViewButtonColumn
            {
                Name = "Attendance",
                HeaderText = "Attendance",
                Text = "view",
                UseColumnTextForButtonValue = true
            };

            DataGridViewButtonColumn unregisterColumn = new DataGridViewButtonColumn
            {
                Name = "Unregister",
                HeaderText = "Unregister",
                Text = "Unregister",
                UseColumnTextForButtonValue = true
            };

            dataGridView1.Columns.Add(attendanceColumn);
            dataGridView1.Columns.Add(unregisterColumn);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string classID = dataGridView1.Rows[e.RowIndex].Cells["classID"].Value.ToString();
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            if (columnName == "Attendance")
            {
                int classIdInt = Convert.ToInt32(classID);
                Attendance attendance = new Attendance(classIdInt);
                attendance.Show();
            }
            else if (columnName == "Unregister")
            {
                // Ask for confirmation before unregistering
                DialogResult result = MessageBox.Show($"Are you sure you want to unregister from Class ID: {classID}?",
                                                      "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    int classIdInt = Convert.ToInt32(classID);
                    GymClass gymClass = new GymClass();
                    gymClass.UnregisterClass(classIdInt, Person.LoggedInUserID);

                    this.Hide();
                    MemberDashboard memberDashboard = new MemberDashboard();
                    memberDashboard.Show();
                }
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {

        }



        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            MemberDashboard memberDashboard = new MemberDashboard();
            memberDashboard.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Membership membership = new Membership(Person.LoggedInUserID);
            membership.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Profile profile = new Profile(Person.LoggedInUserID);
            profile.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LandingPage landingPage = new LandingPage();
            landingPage.Show();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            GymClass gymClass = new GymClass();
            DataTable dt = gymClass.GetAllClasses();

            dataGridView2.DataSource = dt;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (!dataGridView2.Columns.Contains("Register"))
            {
                AddRegisterButton();
            }

            panel3.Visible = false;
        }

        private void AddRegisterButton()
        {
            DataGridViewButtonColumn registerColumn = new DataGridViewButtonColumn
            {
                Name = "Register",
                HeaderText = "Register",
                Text = "Register",
                UseColumnTextForButtonValue = true
            };



            dataGridView2.Columns.Add(registerColumn);
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            string classID = dataGridView2.Rows[e.RowIndex].Cells["classID"].Value.ToString();
            string columnName = dataGridView2.Columns[e.ColumnIndex].Name;

            if (columnName == "Register")
            {
                int classIdInt = Convert.ToInt32(classID);
                GymClass gymClass = new GymClass();
                gymClass.RegisterClass(classIdInt);
            }
        }

    }
}
