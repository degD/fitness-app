using Npgsql;
using Guna.UI2.WinForms;
using Guna.Charts.WinForms;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.Charts.Interfaces;

namespace MacFit
{
    public partial class UserForm : Form
    {
        private string connString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=fitness_db";
        private string userId;

        public UserForm(string userId)
        {
            InitializeComponent();
            this.userId = "12345678901";
        }

        private void YagOraniBtn_Click(object sender, EventArgs e)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    // Kullanıcıdan giriş alınacak (örnek olarak user_id=1 alıyoruz)
                    string userId = "12345678901";


                    string query = "SELECT weight, height, age, gender FROM Member WHERE id = @id";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                double kilo = reader.GetDouble(0);
                                double boy = reader.GetDouble(1);
                                int yas = reader.GetInt32(2);
                                string cinsiyet = reader.GetString(3);

                                double yagOrani;
                                if (cinsiyet.ToLower() == "erkek")
                                    yagOrani = (1.20 * (kilo / (boy * boy))) + (0.23 * yas) - 16.2;
                                else
                                    yagOrani = (1.20 * (kilo / (boy * boy))) + (0.23 * yas) - 5.4;

                                MessageBox.Show($"Vücut Yağ Oranınız: %{yagOrani:F2}");

                                // Hesaplanan yağ oranını kaydet
                                SaveBodyFatPercentage(userId, yagOrani);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void SaveBodyFatPercentage(string userId, double bodyFat)
        {
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string updateQuery = "UPDATE Member SET body_fat = @bodyFat WHERE id = @id";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@bodyFat", bodyFat);
                        cmd.Parameters.AddWithValue("@id", userId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata (Yağ Oranı Kaydetme): " + ex.Message);
            }
        }

        private void KaloriBtn_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable appointmentsTable = new DataTable();
                appointmentsTable.Columns.Add("Appointment ID");
                appointmentsTable.Columns.Add("Session Date");
                appointmentsTable.Columns.Add("Workout Plan");
                appointmentsTable.Columns.Add("Calories Burnt");

                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = @"
            SELECT 
                a.id AS appointment_id,
                s.date AS session_date,
                wp.title AS workout_plan,
                SUM(wpe.calories_burnt) AS calories_burnt
            FROM 
                session s
            JOIN 
                appointment a ON s.id = a.session_id
            JOIN 
                workout_plan wp ON a.workout_plan_id = wp.id
            JOIN 
                workout_plan_exercise wpe ON wp.id = wpe.workout_plan_id
            WHERE 
                a.status = 1 AND a.member_id = @userId
            GROUP BY 
                a.id, s.date, wp.title;"; // Completed sessions

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", this.userId);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow row = appointmentsTable.NewRow();
                                row["Appointment ID"] = reader.GetInt32(0);
                                row["Session Date"] = reader.GetDateTime(1).ToShortDateString();
                                row["Workout Plan"] = reader.GetString(2);
                                row["Calories Burnt"] = reader.GetDouble(3);
                                appointmentsTable.Rows.Add(row);
                            }
                        }
                    }
                }

                appointmentsDataGridView.DataSource = appointmentsTable;
                appointmentsDataGridView.Visible = true; // Görünür yap

                // İlk randevunun detaylarını göster
                if (appointmentsTable.Rows.Count > 0)
                {
                    int firstAppointmentId = Convert.ToInt32(appointmentsTable.Rows[0]["Appointment ID"]);
                    ShowAppointmentDetails(firstAppointmentId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void AppointmentsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = appointmentsDataGridView.Rows[e.RowIndex];
                int appointmentId = Convert.ToInt32(row.Cells["Appointment ID"].Value);

                // Randevuya ait detayları göster
                ShowAppointmentDetails(appointmentId);
            }
        }

        private void ShowAppointmentDetails(int appointmentId)
        {
            try
            {
                Dictionary<string, double> exerciseCalories = new Dictionary<string, double>();

                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = @"
            SELECT 
                e.name AS exercise_name,
                wpe.calories_burnt AS calories_burnt
            FROM 
                appointment a
            JOIN 
                workout_plan wp ON a.workout_plan_id = wp.id
            JOIN 
                workout_plan_exercise wpe ON wp.id = wpe.workout_plan_id
            JOIN 
                exercise e ON wpe.exercise_id = e.id
            WHERE 
                a.id = @appointmentId;";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@appointmentId", appointmentId);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string exerciseName = reader.GetString(0);
                                double calories = reader.GetDouble(1);

                                if (exerciseCalories.ContainsKey(exerciseName))
                                    exerciseCalories[exerciseName] += calories;
                                else
                                    exerciseCalories[exerciseName] = calories;
                            }
                        }
                    }
                }

                // Egzersiz detaylarını göster
                ShowExerciseDetails(exerciseCalories);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void ShowExerciseDetails(Dictionary<string, double> exerciseCalories)
        {
            Guna2Panel panel = new Guna2Panel
            {
                Size = new Size(800, 400),
                Location = new Point(500, 50),
                BorderColor = Color.Black,
                BorderThickness = 1
            };
            this.Controls.Add(panel);

            GunaChart exerciseChart = new GunaChart
            {
                Size = new Size(780, 350),
                Location = new Point(10, 10)
            };
            panel.Controls.Add(exerciseChart);

            GunaStackedBarDataset exerciseDataset = new GunaStackedBarDataset { Label = "Egzersiz Bazında Kalori" };
            foreach (var entry in exerciseCalories)
                exerciseDataset.DataPoints.Add(new LPoint(entry.Key, entry.Value));
            exerciseChart.Datasets.Add(exerciseDataset);
        }

    }
}
