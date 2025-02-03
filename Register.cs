using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GymManagementSystem
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LandingPage landingPage = new LandingPage();
            landingPage.Show();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string first = textBox1.Text;
            string last = textBox2.Text;

            string email = textBox3.Text;
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            string phoneNumber = textBox5.Text;
            string phonePattern = @"^07\d{8}$";

            string password = textBox6.Text;
            string confirmPassword = textBox7.Text;

            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$";

            string selectedText = comboBox1.Text;


            DateTime selectedDate = dateTimePicker1.Value;
            DateTime today = DateTime.Today;

            if (string.IsNullOrEmpty(first))
            {
                MessageBox.Show("First Name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(last))
            {
                MessageBox.Show("Last Name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate email field
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Email cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Invalid email format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate phone number field
            if (string.IsNullOrEmpty(phoneNumber))
            {
                MessageBox.Show("Phone number cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Regex.IsMatch(phoneNumber, phonePattern))
            {
                MessageBox.Show("Phone number must start with '07' and be followed by exactly 8 digits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate Date of Birth
            if (selectedDate == null || selectedDate == DateTime.MinValue)
            {
                MessageBox.Show("DOB cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Calculate the age
            int age = today.Year - selectedDate.Year;
            if (selectedDate > today.AddYears(-age))
            {
                age--;
            }

            // Validate age
            if (age < 16)
            {
                MessageBox.Show("You must be at least 16 years old to register.",
                                "Age Restriction",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            //validate membership type 
            if (string.IsNullOrEmpty(selectedText))
            {
                MessageBox.Show("Membership Type cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // Validate password field
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Regex.IsMatch(password, passwordPattern))
            {
                MessageBox.Show("Password must be at least 8 characters long, include one uppercase letter, one lowercase letter, one digit, and one special character.",
                                "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate confirm password field
            if (string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Confirm Password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (password != confirmPassword)
            {
                MessageBox.Show("Password and Confirm Password do not match.",
                                "Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // if everything valid
            Person person = new Person();
            person.Name = first + " " + last;
            person.Email = email;
            person.PhoneNo = phoneNumber;
            person.Password = password;
            person.Role = "Member";

            int userID = person.Register("Server = localhost; Database = gymsystem; User Id = root; Password = ''");

            if (userID != -1) // Ensure user is successfully registered
            {
                DateTime startDate = DateTime.Now;
                DateTime endDate;

                switch (selectedText)
                {
                    case "Daily":
                        endDate = startDate.AddDays(1);
                        break;
                    case "Weekly":
                        endDate = startDate.AddDays(7);
                        break;
                    case "Monthly":
                        endDate = startDate.AddMonths(1);
                        break;
                    case "Yearly":
                        endDate = startDate.AddYears(1);
                        break;
                    default:
                        endDate = startDate;
                        break;
                }

                // Membership Status is Active upon creation
                string membershipStatus = "Active";

                MembershipClass membership = new MembershipClass(userID, selectedText, startDate, endDate, membershipStatus);

                // Register the membership in the database
                membership.RegisterMembership("Server = localhost; Database = gymsystem; User Id = root; Password = ''");

                MessageBox.Show("User and Membership Registered Successfully" , "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Hide();
                MemberDashboard memberDashboard = new MemberDashboard();
                memberDashboard.Show();

            }
        }




















        private void button5_Click(object sender, EventArgs e)
        {
            string fullname = textBox8.Text;
            string email = textBox9.Text;
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            string phoneNumber = textBox10.Text;
            string phonePattern = @"^07\d{8}$";

            string password = textBox13.Text;
            string confirmPassword = textBox14.Text;

            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$";

            if (string.IsNullOrEmpty(fullname))
            {
                MessageBox.Show("Full Name cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate email field
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Email cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Regex.IsMatch(email, emailPattern))
            {
                MessageBox.Show("Invalid email format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate phone number field
            if (string.IsNullOrEmpty(phoneNumber))
            {
                MessageBox.Show("Phone number cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Regex.IsMatch(phoneNumber, phonePattern))
            {
                MessageBox.Show("Phone number must start with '07' and be followed by exactly 8 digits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate password field
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Regex.IsMatch(password, passwordPattern))
            {
                MessageBox.Show("Password must be at least 8 characters long, include one uppercase letter, one lowercase letter, one digit, and one special character.",
                                "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate confirm password field
            if (string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Confirm Password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (password != confirmPassword)
            {
                MessageBox.Show("Password and Confirm Password do not match.",
                                "Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // If everything is valid
            Person person = new Person();
            person.Name = fullname;
            person.Email = email;
            person.PhoneNo = phoneNumber;
            person.Password = password;
            person.Role = "Trainer";

            int userID = person.Register("Server = localhost; Database = gymsystem; User Id = root; Password = ''");

            this.Hide();
            TrainerDashboard trainerDashboard = new TrainerDashboard();
            trainerDashboard.Show();

            MessageBox.Show("User Registered Successfully!",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }
    }
}
