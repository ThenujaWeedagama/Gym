using System;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Diagnostics;
using System.Data;

namespace GymManagementSystem
{
    public class Person
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public static int LoggedInUserID { get; set; }

        // Constructor 
        public void connect(string name, string email, string phoneNo, string password, string role)
        {
            Name = name;
            Email = email;
            PhoneNo = phoneNo;
            Password = password;
            Role = role;
        }

        // Database connection
        MySqlConnection mySqlConnection = new MySqlConnection("Server = localhost; Database = gymsystem; User Id = root; Password = ''");

        //methods
        public int Register(string connection)
        {
            try
            {
                mySqlConnection.Open();

                string checkQuery = "SELECT * FROM persons WHERE email = @Email OR phoneNo = @PhoneNo";
                MySqlCommand mySqlCommand = new MySqlCommand(checkQuery, mySqlConnection);
                mySqlCommand.Parameters.AddWithValue("@Email", Email);
                mySqlCommand.Parameters.AddWithValue("@PhoneNo", PhoneNo);

                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MessageBox.Show("Email or Phone Number already exists, Please check your email and phone number.");
                        return -1;
                    }
                    else
                    {
                        reader.Close();


                        string addQuery = "INSERT INTO persons (name, email, phoneNo, password, role) VALUES (@Name, @Email, @PhoneNo, @Password, @Role)";
                        MySqlCommand addCommand = new MySqlCommand(addQuery, mySqlConnection);
                        addCommand.Parameters.AddWithValue("@Name", Name);
                        addCommand.Parameters.AddWithValue("@Email", Email);
                        addCommand.Parameters.AddWithValue("@PhoneNo", PhoneNo);
                        addCommand.Parameters.AddWithValue("@Password", Password);
                        addCommand.Parameters.AddWithValue("@Role", Role);
                        addCommand.ExecuteNonQuery();


                        string query = "SELECT LAST_INSERT_ID()";
                        MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                        UserID = Convert.ToInt32(cmd.ExecuteScalar());
                        LoggedInUserID = UserID;
                        return UserID;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
            finally
            {
                mySqlConnection.Close();
            }
        }


        public string Login(string email, string password)
        {
            string role = null;

            try
            {
                mySqlConnection.Open();

                string checkQuery = "SELECT userID, role FROM persons WHERE email = @Email AND password = @Password";
                MySqlCommand mySqlCommand = new MySqlCommand(checkQuery, mySqlConnection);
                mySqlCommand.Parameters.AddWithValue("@Email", email);
                mySqlCommand.Parameters.AddWithValue("@Password", password);

                using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        role = reader["role"].ToString();
                        LoggedInUserID = Convert.ToInt32(reader["userID"]);
                        Debug.WriteLine($"get id when logging: {LoggedInUserID}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }

            return role;
        }


        public void Logout()
        {

        }

        public bool DeleteAccount(string email, string password)
        {
            try
            {
                mySqlConnection.Open();

                string checkQuery = "SELECT * FROM persons WHERE email = @Email AND password = @Password";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, mySqlConnection);
                checkCommand.Parameters.AddWithValue("@Email", email);
                checkCommand.Parameters.AddWithValue("@Password", password);

                using (MySqlDataReader reader = checkCommand.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        MessageBox.Show("Incorrect email or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                string deleteQuery = "DELETE FROM persons WHERE email = @Email AND password = @Password";
                MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, mySqlConnection);
                deleteCommand.Parameters.AddWithValue("@Email", email);
                deleteCommand.Parameters.AddWithValue("@Password", password);

                int rowsAffected = deleteCommand.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Account deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Failed to delete account.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            finally
            {
                mySqlConnection.Close();
            }
        }



        public Person ViewProfile(int LoggedInUserID)
        {
            Person person = null;

            try
            {
                mySqlConnection.Open();

                string query = "SELECT name, email, phoneNo FROM persons WHERE userID = @UserID";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                command.Parameters.AddWithValue("@UserID", LoggedInUserID);

                Debug.WriteLine($"Executing Query: {query} with UserID: {LoggedInUserID}");

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        person = new Person
                        {
                            UserID = LoggedInUserID,
                            Name = reader["name"].ToString(),
                            Email = reader["email"].ToString(),
                            PhoneNo = reader["phoneNo"].ToString()
                        };

                        Debug.WriteLine($"Retrieved Profile: {person.UserID}, {person.Name}, {person.Email}, {person.PhoneNo}");
                    }
                    else
                    {
                        Debug.WriteLine("No profile found for the given userID.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mySqlConnection.Close();
            }

            return person;
        }



        // Update profile method
        public void UpdateProfile(string name, string email, string phoneNo, string password)
        {
            try
            {
                mySqlConnection.Open();


                string query = @"
                UPDATE persons 
                SET name = @Name, email = @Email, phoneNo = @PhoneNo, password = @Password 
                WHERE userID = @UserID";

                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);


                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@PhoneNo", phoneNo);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@UserID", LoggedInUserID);


                int rowsAffected = cmd.ExecuteNonQuery();


                if (rowsAffected > 0)
                {
                    MessageBox.Show("Profile updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Profile update failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating profile: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mySqlConnection.Close();
            }


        }
    }
}
