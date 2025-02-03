using System;
using System.Data;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GymManagementSystem
{
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
            LoadClasses();
        }

        private void LoadClasses()
        {
            GymClass gymClass = new GymClass();
            DataTable dt = gymClass.GetAllClasses();
            dataGridView1.DataSource = dt;  
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


  

        private void button3_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            AssignTrainer assignTrainer = new AssignTrainer();
            assignTrainer.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            LandingPage landingPage = new LandingPage();
            landingPage.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string className = textBox2.Text;
            string email = textBox5.Text;
            string password = textBox6.Text;

            DialogResult result = MessageBox.Show(
               "Are you sure you want to delete your class? This action cannot be undone.",
               "Confirm Deletion",
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Warning
           );

            if (result == DialogResult.Yes)
            {
                GymClass gymClass = new GymClass();
                bool isDeleted =  gymClass.DeleteClassByName(className, email, password);

                if (isDeleted)
                {
                    this.Hide();
                    AdminDashboard adminDashboard = new AdminDashboard();
                    adminDashboard.Show();
                }
            }
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            
        }




        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminMembership adminMembership = new AdminMembership();
            adminMembership.Show();

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            panel6.Visible = false;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            panel5.Visible =false;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            panel6.Visible = true;
        }

        private void panel4_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button10_Click_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Retrieve the values from the input fields
            string classname = textBox1.Text;
            string classSchedule = comboBox4.Text + " " + numericUpDown1.Value.ToString() + ":" + numericUpDown2.Value.ToString("00") + " " + comboBox3.Text;

            // Validate the class name
            if (string.IsNullOrWhiteSpace(classname))
            {
                MessageBox.Show("Please enter a valid Class Name.");
                return;
            }

            // Validate class schedule (ensure none of the components are null or empty)
            if (string.IsNullOrWhiteSpace(comboBox4.Text) || numericUpDown1.Value == 0 || numericUpDown2.Value == 0 || string.IsNullOrWhiteSpace(comboBox3.Text))
            {
                MessageBox.Show("Please select a valid Class Schedule.");
                return;
            }

            // Validate capacity (ensure it is a valid number)
            int capacity;
            if (!int.TryParse(textBox3.Text, out capacity) || capacity <= 0)
            {
                MessageBox.Show("Please enter a valid number for Capacity greater than 0.");
                return;
            }

            string description = textBox4.Text;

            // Validate description (optional field, so only check if not empty)
            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please enter a valid Description.");
                return;
            }

            // Validate duration and type (both are required)
            string duration = comboBox2.Text;
            string type = comboBox1.Text;
            if (string.IsNullOrWhiteSpace(duration) || string.IsNullOrWhiteSpace(type))
            {
                MessageBox.Show("Please select both Duration and Type.");
                return;
            }

            // Create a new instance of GymClass and add the class
            GymClass gymClass = new GymClass(classname, classSchedule, capacity, description, duration, type);
            gymClass.AddNewClass();

            // Hide the panel and refresh the form
            panel4.Visible = false;
            LoadClasses();
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            panel6.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel6.Visible = true;
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            string classID = textBox12.Text.Trim();


            if (string.IsNullOrEmpty(classID))
            {
                MessageBox.Show("Please enter a valid Class ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GymClass gymClass = new GymClass();
            GymClass classDetails = gymClass.GetClassDetails(classID);

            if (classDetails != null)
            {

                textBox8.Text = classDetails.ClassName;
                textBox9.Text = classDetails.ClassSchedule;
                textBox10.Text = classDetails.Capacity.ToString();
                textBox11.Text = classDetails.Description;
                comboBox5.Text = classDetails.Duration;
                comboBox6.Text = classDetails.Type;
            }
            else
            {
                MessageBox.Show("No class found with the specified ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            panel6.Visible = false;
            panel5.Visible = true;
        }
    }
}
