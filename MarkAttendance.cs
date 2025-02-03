using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymManagementSystem
{
    public partial class MarkAttendance : Form
    {
        private int  classID;
        private GymClass attendanceObj = new GymClass();
        private AttendanceClass attendanceClass = new AttendanceClass();

        public MarkAttendance(int classID)
        {
            InitializeComponent();
            this.classID = classID;
            Debug.WriteLine("MarkAttendance form constructed with Class ID: " + classID);
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dddd h:mm tt"; 
            LoadMembers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Load members enrolled in the class into the DataGridView
        private void LoadMembers()
        {
            DataTable dt = attendanceObj.GetMembersForClass(classID);
            dataGridView1.DataSource = dt;

           
            if (!dataGridView1.Columns.Contains("Status"))
            {
                DataGridViewComboBoxColumn statusColumn = new DataGridViewComboBoxColumn();
                statusColumn.Name = "Status";
                statusColumn.HeaderText = "Attendance Status";
                statusColumn.Items.AddRange("Present", "Absent", "Late");
                dataGridView1.Columns.Add(statusColumn);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime attendanceDate = dateTimePicker1.Value;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
              
                if (row.IsNewRow)
                    continue;

              
                int memberID = Convert.ToInt32(row.Cells["userID"].Value);

              
                string status = row.Cells["Status"].Value != null ? row.Cells["Status"].Value.ToString() : "Record No-Shows"; 

            
                attendanceClass.MarkAttendance(classID, memberID, attendanceDate, status);
            }
            MessageBox.Show("Attendance marked successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close(); 
        }
    }
}
