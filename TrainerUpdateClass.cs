using System;
using System.Windows.Forms;

namespace GymManagementSystem
{
    public partial class TrainerUpdateClass : Form
    {
        private int classID;
        private GymClass gymclass = new GymClass();

        public TrainerUpdateClass(int classID)
        {
            InitializeComponent();
            this.classID = classID;
            LoadClassDetails();
        }


        private void LoadClassDetails()
        {
            GymClass classDetails = gymclass.GetClassDetails(classID.ToString());

            if (classDetails != null)
            {
                textBox1.Text = classDetails.ClassSchedule;
                textBox2.Text = classDetails.Capacity.ToString();
                textBox3.Text = classDetails.Description;
                comboBox1.Text = classDetails.Duration;
                comboBox2.Text = classDetails.Type;
            }
            else
            {
                MessageBox.Show("Class details not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string schedule = textBox1.Text;
            int capacity = int.Parse(textBox2.Text);
            string description = textBox3.Text;
            string duration =  comboBox1.Text;
            string type = comboBox2.Text;

            gymclass.UpdateClassDetails(classID, schedule, capacity, description, duration, type);

            this.Close();

        }
    }
}
