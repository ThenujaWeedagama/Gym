using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Linq;

namespace GymManagementSystem
{
    public class AttendanceClass
    {

        public int AttendanceId { get; set; }
        public int UserID { get; set; }
        public int ClassID { get; set; }

        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
        
        
        
        private MySqlConnection mySqlConnection = new MySqlConnection("Server=localhost;Database=gymsystem;User Id=root;Password=;");



     
        public void MarkAttendance(int classID, int memberID, DateTime attendanceDate, string status)
        {
            try
            {
                mySqlConnection.Open();
                string query = "INSERT INTO attendance (userID, classID, status, timestamp) VALUES (@MemberID, @ClassID, @Status, @Timestamp)";
                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                cmd.Parameters.AddWithValue("@MemberID", memberID);
                cmd.Parameters.AddWithValue("@ClassID", classID);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Timestamp", attendanceDate);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error marking attendance: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }
        }




        public DataTable GetAttendanceDetails(int classID, int memberID)
        {
            DataTable attendanceDetails = new DataTable();  

            try
            {
                mySqlConnection.Open();

               
                string query = @"
                SELECT timestamp, status 
                FROM attendance 
                WHERE classID = @ClassID AND userID = @MemberID
                ORDER BY timestamp DESC";  

                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                cmd.Parameters.AddWithValue("@ClassID", classID);
                cmd.Parameters.AddWithValue("@MemberID", memberID);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(attendanceDetails);  
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching attendance: " + ex.Message);
            }
            finally
            {
                mySqlConnection.Close();
            }

            return attendanceDetails;  
        }


        public void CancelAttendanceIfClassDayMatches(int classID, DateTime inputDateTime)
        {
            try
            {
                mySqlConnection.Open();

                // Insert a new attendance record for the given date
                string insertAttendanceQuery = @"
            INSERT INTO attendance (classID, userID, status, timestamp)
            VALUES (@ClassID, @UserID, 'Cancel', @Timestamp)";

                MySqlCommand insertCmd = new MySqlCommand(insertAttendanceQuery, mySqlConnection);
                insertCmd.Parameters.AddWithValue("@ClassID", classID);
                insertCmd.Parameters.AddWithValue("@UserID", Person.LoggedInUserID); // Assuming Person.LoggedInUserID is available
                insertCmd.Parameters.AddWithValue("@Timestamp", inputDateTime); // Use the provided date and time

                int rowsAffected = insertCmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Attendance marked successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to mark attendance. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error marking attendance: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mySqlConnection.Close();
            }

        }
    }
}
