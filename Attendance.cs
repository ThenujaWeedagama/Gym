using System;
using System.Data;
using System.Windows.Forms;

namespace GymManagementSystem
{
    public partial class Attendance : Form
    {
        int memberID = Person.LoggedInUserID;
        private int classID;

        public Attendance(int classID)
        {
            InitializeComponent();
            this.classID = classID;  
            LoadAttendance(); 
        }

        private void LoadAttendance()
        {
            AttendanceClass attendanceClass = new AttendanceClass();
            DataTable attendanceDetails = attendanceClass.GetAttendanceDetails(classID, memberID);

            dataGridView1.DataSource = attendanceDetails;

            dataGridView1.Columns[0].HeaderText = "Timestamp";  
            dataGridView1.Columns[1].HeaderText = "Status";

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
            this.Close();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePicker1.Value;

  

            AttendanceClass attendanceClass = new AttendanceClass();
            attendanceClass.CancelAttendanceIfClassDayMatches(classID, selectedDate);
        }
    }
}
