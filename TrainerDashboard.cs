using GymManagementSystem;
using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace GymManagementSystem
{
    public partial class TrainerDashboard : Form
    {

        public TrainerDashboard()
        {
            InitializeComponent();
            dataGridView1.CellContentClick += new DataGridViewCellEventHandler(dataGridView1_CellContentClick);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();    
            LandingPage landingPage = new LandingPage();    
            landingPage.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Get the trainer's ID from the Person class (logged in trainer)
            int trainerID = Person.LoggedInUserID;

            // Create an instance of GymClass and get assigned classes
            GymClass gymClass = new GymClass(); 
            DataTable dt = gymClass.GetAssignedClasses(trainerID);

            // Set the DataSource of the DataGridView to the DataTable
            dataGridView1.DataSource = dt;

            // Optional: Customize your DataGridView columns for a nicer appearance
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dataGridView1.Columns.Contains("classID"))
                dataGridView1.Columns["classID"].HeaderText = "Class ID";
            if (dataGridView1.Columns.Contains("className"))
                dataGridView1.Columns["className"].HeaderText = "Class Name";
            if (dataGridView1.Columns.Contains("classSchedule"))
                dataGridView1.Columns["classSchedule"].HeaderText = "Schedule";
            if (dataGridView1.Columns.Contains("capacity"))
                dataGridView1.Columns["capacity"].HeaderText = "Capacity";
            if (dataGridView1.Columns.Contains("description"))
                dataGridView1.Columns["description"].HeaderText = "Description";
            if (dataGridView1.Columns.Contains("duration"))
                dataGridView1.Columns["duration"].HeaderText = "Duration";
            if (dataGridView1.Columns.Contains("type"))
                dataGridView1.Columns["type"].HeaderText = "Type";

            if (!dataGridView1.Columns.Contains("MarkAttendance") && !dataGridView1.Columns.Contains("Update"))
            {
                AddButtonColumns();
            }

            panel2.Visible = true;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void AddButtonColumns()
        {
            // Create the Mark Attendance button column
            DataGridViewButtonColumn markAttendanceColumn = new DataGridViewButtonColumn();
            markAttendanceColumn.Name = "MarkAttendance";
            markAttendanceColumn.HeaderText = "Mark Attendance";
            markAttendanceColumn.Text = "Mark";
            markAttendanceColumn.UseColumnTextForButtonValue = true;

            // Create the Update button column
            DataGridViewButtonColumn updateColumn = new DataGridViewButtonColumn();
            updateColumn.Name = "Update";
            updateColumn.HeaderText = "Update";
            updateColumn.Text = "Update";
            updateColumn.UseColumnTextForButtonValue = true;

            // Add the columns to your DataGridView (dataGridView1)

            dataGridView1.Columns.Add(markAttendanceColumn);
            dataGridView1.Columns.Add(updateColumn);

            Debug.WriteLine("Button columns added.");
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Debug.WriteLine("CellContentClick event fired. Row: " + e.RowIndex + ", Column: " + e.ColumnIndex);

            // Ensure we're not handling header clicks or out-of-range indexes
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            // Retrieve the classID from the current row (assuming it is in a column named "classID")
            string classID = dataGridView1.Rows[e.RowIndex].Cells["classID"].Value.ToString();
            Debug.WriteLine("Retrieved classID: " + classID);

            // Determine which button was clicked based on the column name
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;

            if (columnName == "MarkAttendance")
            {
                // Call your method to mark attendance, passing classID
                MarkAttendance(classID);
            }
            else if (columnName == "Update")
            {
                // Call your method to update the class, passing classID
                UpdateClassDetails(classID);
            }
        }


        private void MarkAttendance(string classID)
        {
            try
            {
                
                int classIdInt = Convert.ToInt32(classID);
                MarkAttendance markAttendanceForm = new MarkAttendance(classIdInt);
                markAttendanceForm.Show();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid Class ID format. Please enter a valid number. " + ex.Message);
            }
        }

        private void UpdateClassDetails(string classID)
        {
            try
            {
                int classIdInt = Convert.ToInt32(classID);
                TrainerUpdateClass trainerUpdateClass = new TrainerUpdateClass(classIdInt);
                trainerUpdateClass.Show();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Invalid Class ID format. Please enter a valid number. " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            TrainerProfile trainerProfile = new TrainerProfile(Person.LoggedInUserID);
            trainerProfile.Show();
        }
    }
}
