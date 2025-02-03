using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace GymManagementSystem
{
    public class GymClass
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassSchedule { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string Type { get; set; }

        public int UserID { get; set; }

        // Database connection
        private MySqlConnection mySqlConnection = new MySqlConnection("Server=localhost; Database=gymsystem; User Id=root;Password=;");


        // Constructor

        public GymClass() { }

        public GymClass(string className, string classSchedule, int capacity, string description, string duration, string type)
        {
            ClassName = className;
            ClassSchedule = classSchedule;
            Capacity = capacity;
            Description = description;
            Duration = duration;
            Type = type;
        }

        // Method to add a new class
        public void AddNewClass()
        {
            try
            {
                mySqlConnection.Open();

                // Check if class already exists
                string checkQuery = "SELECT COUNT(*) FROM class WHERE className = @ClassName";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, mySqlConnection);
                checkCmd.Parameters.AddWithValue("@ClassName", ClassName);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Class Name already exists, Please enter a new class name.");
                    return;
                }

                // Insert new class
                string insertQuery = "INSERT INTO class (className, classSchedule, capacity, description, duration, type) " +
                                     "VALUES (@ClassName, @ClassSchedule, @Capacity, @Description, @Duration, @Type)";
                MySqlCommand insertCmd = new MySqlCommand(insertQuery, mySqlConnection);
                insertCmd.Parameters.AddWithValue("@ClassName", ClassName);
                insertCmd.Parameters.AddWithValue("@ClassSchedule", ClassSchedule);
                insertCmd.Parameters.AddWithValue("@Capacity", Capacity);
                insertCmd.Parameters.AddWithValue("@Description", Description);
                insertCmd.Parameters.AddWithValue("@Duration", Duration);
                insertCmd.Parameters.AddWithValue("@Type", Type);

                int rowsAffected = insertCmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Class added successfully!");
                }
                else
                {
                    MessageBox.Show("Failed to add class.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }

        public DataTable GetAllClasses()
        {
            DataTable dataTable = new DataTable();

            try
            {
                mySqlConnection.Open();
                string query = "SELECT classID, className, classSchedule, capacity, description, duration, type, userID FROM class";
                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching classes: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }

            return dataTable;
        }

        public bool AuthenticateAdmin(string email, string password)
        {
            bool isAdmin = false;
            try
            {
                mySqlConnection.Open();
                string query = "SELECT role FROM persons WHERE email = @Email AND password = @Password";
                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                object result = cmd.ExecuteScalar();
                if (result != null && result.ToString() == "Admin")
                {
                    isAdmin = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking admin authentication: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }
            return isAdmin;
        }

        // Method to delete a class by name (only if authenticated)
        public bool DeleteClassByName(string className, string email, string password)
        {
            if (!AuthenticateAdmin(email, password))
            {
                MessageBox.Show("Authentication failed. Only admins can delete classes.");
                return false;
            }

            try
            {
                mySqlConnection.Open();

                string deleteQuery = "DELETE FROM class WHERE className = @ClassName";
                MySqlCommand deleteCmd = new MySqlCommand(deleteQuery, mySqlConnection);
                deleteCmd.Parameters.AddWithValue("@ClassName", className);

                int rowsAffected = deleteCmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Class deleted successfully.");
                    return true;
                }
                else
                {
                    MessageBox.Show("Class not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting class: " + ex.Message);
                return false;
            }
            finally
            {
                mySqlConnection.Close();
            }
        }


        // show classes without trainers
        public DataTable GetClassWithoutTrainers()
        {
            DataTable dataTable = new DataTable();

            try
            {
                mySqlConnection.Open();
                string query = "SELECT classID, className, classSchedule, duration, type FROM class WHERE userID IS NULL;";
                using (MySqlCommand cmd = new MySqlCommand(query, mySqlConnection))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching classes: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }

            return dataTable;
        }

        // assign trainers into classess
        public void AssignTrainerToClass(string classID, string trainerID)
        {
            try
            {
                mySqlConnection.Open();

                // Step 1: Check if the trainer is already assigned to a class at the same time
                string checkQuery = @"
                SELECT COUNT(*) 
                FROM class 
                WHERE UserID = @TrainerID 
                AND classSchedule = (SELECT classSchedule FROM class WHERE classID = @ClassID) 
                AND duration = (SELECT duration FROM class WHERE classID = @ClassID);
            ";

                using (MySqlCommand cmd = new MySqlCommand(checkQuery, mySqlConnection))
                {
                    cmd.Parameters.AddWithValue("@TrainerID", trainerID);
                    cmd.Parameters.AddWithValue("@ClassID", classID);

                    int classCount = Convert.ToInt32(cmd.ExecuteScalar());

                    // If the trainer is already assigned to a class at the same time, show an error
                    if (classCount > 0)
                    {
                        MessageBox.Show("Trainer is already assigned to another class at this time.");
                        return;
                    }
                }

                // Step 2: If not assigned, proceed to update the class with the trainer
                string assignQuery = "UPDATE class SET userID = @TrainerID WHERE classID = @ClassID";

                using (MySqlCommand cmd = new MySqlCommand(assignQuery, mySqlConnection))
                {
                    cmd.Parameters.AddWithValue("@TrainerID", trainerID);
                    cmd.Parameters.AddWithValue("@ClassID", classID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Trainer assigned successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to assign trainer. Please check the class ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }

        // show trainers schedule 

        public DataTable GetTrainerSchedules()
        {
            DataTable dataTable = new DataTable();

            try
            {
                mySqlConnection.Open();

                // SQL query to get trainers assigned to classes
                string query = @"
            SELECT p.userID AS `Trainer ID`, p.name AS `Trainer Name`, p.email AS `Trainer Email`, 
                   c.className AS `Class Name`, c.classSchedule AS `Class Schedule`, 
                   c.duration AS `Duration`, c.type AS `Class Type`
            FROM persons p
            JOIN class c ON p.userID = c.userID
            WHERE p.role = 'Trainer';"; // Ensure you use the correct role name

                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching trainer schedules: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mySqlConnection.Close();
            }

            return dataTable;
        }





        // get class details 
        public GymClass GetClassDetails(string classID)
        {
            GymClass result = null;
            try
            {
                mySqlConnection.Open();

                string query = "SELECT classID, className, classSchedule, capacity, description, duration, type FROM class WHERE classID = @ClassID";
                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                cmd.Parameters.AddWithValue("@ClassID", classID);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new GymClass(
                            reader["className"].ToString(),
                            reader["classSchedule"].ToString(),
                            Convert.ToInt32(reader["capacity"]),
                            reader["description"].ToString(),
                            reader["duration"].ToString(),
                            reader["type"].ToString()
                        );
                        result.ClassID = Convert.ToInt32(reader["classID"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error retrieving class details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mySqlConnection.Close();
            }
            return result;
        }

      

        // Method to get all members enrolled in a class (assumes a table "class_registration" exists)
        public DataTable GetMembersForClass(int classID)
        {
            DataTable dt = new DataTable();
            try
            {
                mySqlConnection.Open();
                string query = @"
                    SELECT p.userID, p.name
                    FROM persons p
                    JOIN class_registration cr ON p.userID = cr.memberID
                    WHERE cr.classID = @ClassID";
                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                cmd.Parameters.AddWithValue("@ClassID", classID);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching members: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }
            return dt;
        }



        // Method to retrieve classes assigned to a given trainer (UserID)
        public DataTable GetAssignedClasses(int trainerID)
        {
            DataTable dataTable = new DataTable();

            try
            {
                mySqlConnection.Open();

                // SQL query to select classes where the trainer (userID) is assigned
                string query = @"
                    SELECT classID, className, classSchedule, capacity, description, duration, type
                    FROM class 
                    WHERE userID = @TrainerID";

                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                cmd.Parameters.AddWithValue("@TrainerID", trainerID);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching assigned classes: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }

            return dataTable;
        }


        public void UpdateClassDetails(int classID, string schedule, int capacity, string description, string duration, string type)
        {
            try
            {
                mySqlConnection.Open();
                string query = "UPDATE class SET classSchedule = @Schedule, capacity = @Capacity, description = @Description, duration = @Duration, type = @Type WHERE classID = @ClassID";

                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                cmd.Parameters.AddWithValue("@Schedule", schedule);
                cmd.Parameters.AddWithValue("@Capacity", capacity);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Duration", duration);
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ClassID", classID);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Class details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No changes were made.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating class details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }

        public DataTable GetMemeberClasses(int trainerID)
        {
            DataTable dataTable = new DataTable();

            try
            {
                mySqlConnection.Open();

                // SQL query to select classes where the trainer (userID) is assigned
                string query = @"
                    SELECT c.classID, c.className, c.classSchedule, c.capacity, c.description, c.duration, c.type
                    FROM class c 
                    JOIN class_registration cr ON c.classID = cr.classID
                    WHERE cr.memberID = @TrainerID";

                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                cmd.Parameters.AddWithValue("@TrainerID", trainerID);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching assigned classes: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }

            return dataTable;
        }


        public void RegisterClass(int classID)
        {
            int memberID = Person.LoggedInUserID; // Get the logged-in user ID

            try
            {
                mySqlConnection.Open();

                // 1. Check if the user has an active membership
                string membershipQuery = @"
            SELECT status 
            FROM membership 
            WHERE memberID = @MemberID";

                MySqlCommand membershipCmd = new MySqlCommand(membershipQuery, mySqlConnection);
                membershipCmd.Parameters.AddWithValue("@MemberID", memberID);

                object membershipStatusObj = membershipCmd.ExecuteScalar();
                string membershipStatus = (membershipStatusObj != DBNull.Value) ? membershipStatusObj.ToString() : "";

                if (membershipStatus != "Active")
                {
                    MessageBox.Show("Your membership is not active. Please activate your membership to register for classes.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 2. Get the class capacity
                string capacityQuery = @"
            SELECT capacity 
            FROM class 
            WHERE classID = @ClassID";

                MySqlCommand capacityCmd = new MySqlCommand(capacityQuery, mySqlConnection);
                capacityCmd.Parameters.AddWithValue("@ClassID", classID);

                object capacityObj = capacityCmd.ExecuteScalar();
                int classCapacity = (capacityObj != DBNull.Value) ? Convert.ToInt32(capacityObj) : 0;

                // 3. Get the current number of members registered in the class
                string countQuery = @"
            SELECT COUNT(*) 
            FROM class_registration 
            WHERE classID = @ClassID";

                MySqlCommand countCmd = new MySqlCommand(countQuery, mySqlConnection);
                countCmd.Parameters.AddWithValue("@ClassID", classID);

                int registeredMembers = Convert.ToInt32(countCmd.ExecuteScalar());

                // 4. Check if there is available capacity
                if (registeredMembers >= classCapacity)
                {
                    MessageBox.Show("Sorry, the class is full.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 5. Check if the user is already registered for the class
                string checkQuery = @"
            SELECT COUNT(*) 
            FROM class_registration 
            WHERE memberID = @MemberID AND classID = @ClassID";

                MySqlCommand checkCmd = new MySqlCommand(checkQuery, mySqlConnection);
                checkCmd.Parameters.AddWithValue("@MemberID", memberID);
                checkCmd.Parameters.AddWithValue("@ClassID", classID);

                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("You are already registered for this class.", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 6. Insert new registration if there is space
                string insertQuery = @"
            INSERT INTO class_registration (memberID, classID, registrationDate) 
            VALUES (@MemberID, @ClassID, NOW())";

                MySqlCommand insertCmd = new MySqlCommand(insertQuery, mySqlConnection);
                insertCmd.Parameters.AddWithValue("@MemberID", memberID);
                insertCmd.Parameters.AddWithValue("@ClassID", classID);

                int rowsAffected = insertCmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Class registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Class registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error registering class: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }



        public void UnregisterClass(int classID, int memberID)
        {
            try
            {
                using (MySqlConnection mySqlConnection = new MySqlConnection("Server=localhost;Database=gymsystem;User Id=root;Password=;"))
                {
                    mySqlConnection.Open();
                    string query = "DELETE FROM class_registration WHERE classID = @ClassID AND memberID = @MemberID";

                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@ClassID", classID);
                    cmd.Parameters.AddWithValue("@MemberID", memberID);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("You have successfully unregistered from the class!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unregister failed! You may not be registered for this class.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error unregistering from class: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}

