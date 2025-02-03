using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymManagementSystem
{
    internal class MembershipClass
    {
        public int MembershipID { get; set; }
        public int UserID { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }



        // Constructor 
        public MembershipClass() {}
        public MembershipClass(int userID, string type, DateTime startDate, DateTime endDate, string status)
        {
            UserID = userID;
            Type = type;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
        }

        MySqlConnection mySqlConnection = new MySqlConnection("Server = localhost; Database = gymsystem; User Id = root; Password = ''");

        // registration of membership 
        public void RegisterMembership(string connection)
        {
            try
            {
                mySqlConnection.Open();

                // Check if the user already has a membership
                string checkQuery = "SELECT * FROM Membership WHERE userID = @userID";
                MySqlCommand checkCommand = new MySqlCommand(checkQuery, mySqlConnection);
                checkCommand.Parameters.AddWithValue("@userID", UserID);

                using (MySqlDataReader reader = checkCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        MessageBox.Show("This user already has a membership.");
                    }
                    else
                    {
                        reader.Close();

                        // Insert new membership record
                        string insertQuery = "INSERT INTO membership (userID, type, startDate, endDate, status) VALUES (@UserID, @Type, @StartDate, @EndDate, @Status)";
                        MySqlCommand insertCommand = new MySqlCommand(insertQuery, mySqlConnection);

                        insertCommand.Parameters.AddWithValue("@UserID", UserID);
                        insertCommand.Parameters.AddWithValue("@Type", Type);
                        insertCommand.Parameters.AddWithValue("@StartDate", StartDate);
                        insertCommand.Parameters.AddWithValue("@EndDate", EndDate);
                        insertCommand.Parameters.AddWithValue("@Status", Status);

                        insertCommand.ExecuteNonQuery();
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


        public DataTable GetMembershipDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                mySqlConnection.Open();

                string query = @"
                    SELECT 
                        m.membershipID, 
                        m.userID, 
                        p.name AS PersonName, 
                        m.type, 
                        m.startDate, 
                        m.endDate, 
                        m.status
                    FROM membership m
                    JOIN persons p ON m.userID = p.userID";

                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt); // Fill DataTable with results
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching membership details: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }
            return dt;
        }


        public void UpdateMembership(string membershipID, string type, string status)
        {
            try
            {
                mySqlConnection.Open();

                // Check if Membership ID exists
                string checkQuery = "SELECT COUNT(*) FROM membership WHERE membershipID = @membershipID";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, mySqlConnection);
                checkCmd.Parameters.AddWithValue("@membershipID", membershipID);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count == 0)
                {
                    MessageBox.Show("Invalid Membership ID. Please enter a valid one.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update membership type and status
                string updateQuery = "UPDATE membership SET type = @type, status = @status WHERE membershipID = @membershipID";
                MySqlCommand updateCmd = new MySqlCommand(updateQuery, mySqlConnection);
                updateCmd.Parameters.AddWithValue("@membershipID", membershipID);
                updateCmd.Parameters.AddWithValue("@type", type);
                updateCmd.Parameters.AddWithValue("@status", status);

                int rowsAffected = updateCmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Membership updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to update membership.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating membership: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }


        public (string membershipID, string type, string startDate, string endDate, string status) GetMembershipInfo(int userID)
        {
            string membershipID = "", type = "", startDate = "", endDate = "", status = "";

            try
            {
                mySqlConnection.Open();
                string query = @"
                SELECT membershipID, type, startDate, endDate, status
                FROM membership 
                WHERE userID = @UserID";

                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                cmd.Parameters.AddWithValue("@UserID", userID);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    membershipID = reader["membershipID"].ToString();
                    type = reader["type"].ToString();
                    startDate = Convert.ToDateTime(reader["startDate"]).ToString("yyyy-MM-dd");
                    endDate = Convert.ToDateTime(reader["endDate"]).ToString("yyyy-MM-dd");
                    status = reader["status"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching membership details: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }

            return (membershipID, type, startDate, endDate, status);  // Return values as a tuple
        }

    }
}
