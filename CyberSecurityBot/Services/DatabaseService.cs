// ============================================================
// File: Services/DatabaseService.cs
// Purpose: Handles all MySQL database operations for the chatbot.
//          Includes task CRUD, activity logging with detailed descriptions,
//          and retrieval with pagination support.
// ============================================================

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CyberSecurityBot.Services
{
    /// <summary>
    /// Manages database connections and operations for tasks and activity logs.
    /// </summary>
    public class DatabaseService
    {
        // Connection string - connects to your local MySQL database
        private readonly string connectionString = "Server=localhost;Database=cybersecurity_bot;Uid=root;Pwd=mnkfoa6mam;";

        /*
         * Oracle. 2024. MySQL Connector/NET Developer Guide. MySQL Documentation.
         * [Online]. Available at: https://dev.mysql.com/doc/connector-net/en/
         * [Accessed 15 June 2026].
         */
        public bool TestConnection()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Tests the database connection and returns a user-friendly message.
        /// </summary>
        /// <returns>Connection status message</returns>
        public string TestConnectionMessage()
        {
            if (TestConnection())
                return "Database connected successfully!";
            else
                return "Database connection failed. Check your MySQL password.";
        }

        // ==================== TASK OPERATIONS ====================

        /*
         * Stack Overflow Community, 2020. C# MySQL insert query with parameters.
         * Stack Overflow. [Online]. Available at: https://stackoverflow.com/questions/65229757/c-sharp-mysql-insert-query-with-parameters
         * [Accessed 15 June 2026].
         */
        public bool AddTask(string title, string description, DateTime? reminderDate = null)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO tasks (title, description, reminder_date) VALUES (@title, @description, @reminderDate)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@reminderDate", reminderDate ?? (object)DBNull.Value);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /*
         * C-Sharp Corner, 2023. CRUD Operations in C# with MySQL Database.
         * [Online]. Available at: https://www.c-sharpcorner.com/article/crud-operations-in-c-sharp-with-mysql-database/
         * [Accessed 15 June 2026].
         */
        public List<string> GetAllTasks()
        {
            List<string> tasks = new List<string>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT id, title, description, reminder_date, is_completed FROM tasks ORDER BY created_at DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            string title = reader.GetString("title");
                            string description = reader.IsDBNull(reader.GetOrdinal("description")) ? "No description" : reader.GetString("description");

                            // Enhanced reminder display with overdue detection
                            string reminder;
                            if (reader.IsDBNull(reader.GetOrdinal("reminder_date")))
                            {
                                reminder = "No reminder";
                            }
                            else
                            {
                                DateTime remDate = reader.GetDateTime("reminder_date");
                                bool completed = reader.GetBoolean("is_completed");

                                if (remDate < DateTime.Now && !completed)
                                {
                                    reminder = $"OVERDUE since {remDate:yyyy-MM-dd}!";
                                }
                                else if (remDate.Date == DateTime.Now.Date && !completed)
                                {
                                    reminder = $"DUE TODAY ({remDate:yyyy-MM-dd})!";
                                }
                                else if (completed)
                                {
                                    reminder = $"Completed (was due {remDate:yyyy-MM-dd})";
                                }
                                else
                                {
                                    reminder = remDate.ToString("yyyy-MM-dd");
                                }
                            }

                            bool isCompleted = reader.GetBoolean("is_completed");
                            string status = isCompleted ? "[COMPLETED]" : "[PENDING]";
                            tasks.Add($"{status} Task #{id}: {title} - {description} (Reminder: {reminder})");
                        }
                    }
                }
            }
            catch (Exception)
            {
                tasks.Add("Error loading tasks.");
            }

            return tasks;
        }

        /*
         * GeeksforGeeks, 2024. Update and Delete Data in MySQL using C#.
         * [Online]. Available at: https://www.geeksforgeeks.org/update-and-delete-data-in-mysql-using-c-sharp/
         * [Accessed 15 June 2026].
         */
        public bool MarkTaskAsCompleted(int taskId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE tasks SET is_completed = TRUE WHERE id = @taskId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@taskId", taskId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /*
         * TutorialsTeacher, 2024. C# MySQL Delete Operation.
         * [Online]. Available at: https://www.tutorialsteacher.com/csharp/csharp-mysql-delete
         * [Accessed 15 June 2026].
         */
        public bool DeleteTask(int taskId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM tasks WHERE id = @taskId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@taskId", taskId);
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ==================== ACTIVITY LOG OPERATIONS ====================

        /*
         * Stack Overflow Community, 2021. Best practice for logging in C# application with database.
         * Stack Overflow. [Online]. Available at: https://stackoverflow.com/questions/68107055/best-practice-for-logging-in-c-sharp-application-with-database
         * [Accessed 15 June 2026].
         */
        public void LogActivity(string actionType, string description)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO activity_log (action_type, description) VALUES (@actionType, @description)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@actionType", actionType);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // Silently fail - activity log should not crash the bot
            }
        }

        /// <summary>
        /// Logs a task addition with detailed reminder information.
        /// </summary>
        public void LogTaskAdded(string taskTitle, int? daysUntilReminder)
        {
            if (daysUntilReminder.HasValue)
            {
                DateTime reminderDate = DateTime.Now.AddDays(daysUntilReminder.Value);
                LogActivity("Task Added", $"Task '{taskTitle}' added with reminder set for {daysUntilReminder} days from now ({reminderDate:yyyy-MM-dd})");
            }
            else
            {
                LogActivity("Task Added", $"Task '{taskTitle}' added with no reminder");
            }
        }

        /// <summary>
        /// Logs a reminder being set for an existing task.
        /// </summary>
        public void LogReminderSet(string taskTitle, int days)
        {
            DateTime reminderDate = DateTime.Now.AddDays(days);
            LogActivity("Reminder Set", $"Reminder set for '{taskTitle}' on {reminderDate:yyyy-MM-dd} ({days} days from now)");
        }

        /// <summary>
        /// Logs quiz completion with score.
        /// </summary>
        public void LogQuizCompleted(int score, int totalQuestions, double percentage)
        {
            LogActivity("Quiz Completed", $"Quiz completed with score {score}/{totalQuestions} ({percentage:F0}%)");
        }

        /// <summary>
        /// Logs NLP interpretation of user command.
        /// </summary>
        public void LogNLPInteraction(string detectedIntent, string userInput)
        {
            LogActivity("NLP Interaction", $"Detected intent '{detectedIntent}' from input: '{userInput}'");
        }

        /// <summary>
        /// Logs keyword detection response.
        /// </summary>
        public void LogKeywordResponse(string topic, string userInput)
        {
            LogActivity("Keyword Detected", $"Responded to '{topic}' from input: '{userInput}'");
        }

        /*
         * C-Sharp Corner, 2022. Retrieve Data from MySQL Database in C#.
         * [Online]. Available at: https://www.c-sharpcorner.com/article/retrieve-data-from-mysql-database-in-c-sharp/
         * [Accessed 15 June 2026].
         */
        public List<string> GetRecentActivities(int limit = 10)
        {
            List<string> activities = new List<string>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT action_type, description, timestamp FROM activity_log ORDER BY timestamp DESC LIMIT @limit";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@limit", limit);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string actionType = reader.GetString("action_type");
                                string description = reader.GetString("description");
                                DateTime timestamp = reader.GetDateTime("timestamp");

                                activities.Add($"[{timestamp:yyyy-MM-dd HH:mm}] {actionType}: {description}");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                activities.Add("Error loading activity log.");
            }

            return activities;
        }

        /// <summary>
        /// Gets total count of activities for pagination.
        /// </summary>
        public int GetActivityCount()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM activity_log";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        return Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}