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

        private void ClearPanels()
        {
            List<Control> controlsToRemove = new List<Control>();

            foreach (Control control in this.Controls)
            {
                if (control is Guna2Panel || control is DataGridView)
                {
                    controlsToRemove.Add(control);
                }
            }

            foreach (Control control in controlsToRemove)
            {
                this.Controls.Remove(control);
            }
        }

        private void YagOraniBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ClearPanels();
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    string query = "SELECT weight, height, birth_date FROM Member WHERE id = @userId";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", this.userId);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                double kilo = reader.GetDouble(0);
                                double boy = reader.GetDouble(1);
                                DateTime birthdate = reader.GetDateTime(2);
                                int yas = DateTime.Now.Year - birthdate.Year;
                                if (DateTime.Now.DayOfYear < birthdate.DayOfYear)
                                    yas--;

                                double yagOrani;
                                yagOrani = (1.20 * (kilo / (boy / 100 * boy / 100))) + (0.23 * yas) - 16.2;
                                double idealKilo = 22 * ((boy / 100) * (boy / 100));

                                ShowYagOraniDetails(kilo, boy, yas, yagOrani, idealKilo);
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

        private void ShowYagOraniDetails(double kilo, double boy, int yas, double yagOrani, double idealKilo)
        {
            // Main panel for user fitness information
            Guna2Panel userInfoPanel = new Guna2Panel
            {
                Size = new Size(800, 1000),
                BorderColor = Color.Black,
                BorderThickness = 1,
                Location = new Point(300, 10)
            };
            this.Controls.Add(userInfoPanel);

            // Labels for user fitness information
            Label lblHeight = new Label { Text = $"Height: {boy} m", Location = new Point(20, 20), AutoSize = true };
            Label lblWeight = new Label { Text = $"Weight: {kilo} kg", Location = new Point(20, 50), AutoSize = true };
            Label lblBodyFat = new Label { Text = $"Body Fat: {yagOrani:F2} %", Location = new Point(20, 80), AutoSize = true };
            Label lblBMI = new Label { Text = $"BMI: {(kilo / (boy / 100 * boy / 100)):F2}", Location = new Point(20, 110), AutoSize = true };
            Label lblIdealWeight = new Label { Text = $"Ideal Weight: {idealKilo:F2} kg", Location = new Point(20, 140), AutoSize = true };
            Label lblMetabolicAge = new Label { Text = $"Metabolic Age: {yas}", Location = new Point(20, 170), AutoSize = true };

            userInfoPanel.Controls.Add(lblHeight);
            userInfoPanel.Controls.Add(lblWeight);
            userInfoPanel.Controls.Add(lblBodyFat);
            userInfoPanel.Controls.Add(lblBMI);
            userInfoPanel.Controls.Add(lblIdealWeight);
            userInfoPanel.Controls.Add(lblMetabolicAge);

            // Progress bar for ideal weight
            Guna2ProgressBar progressBar = new Guna2ProgressBar
            {
                Size = new Size(500, 30),
                Location = new Point(20, 200),
                Value = (int)((kilo / idealKilo) * 100)
            };
            userInfoPanel.Controls.Add(progressBar);

            // Button to update data
            Guna2Button updateDataButton = new Guna2Button
            {
                Text = "Update Data",
                Location = new Point(20, 240),
                Size = new Size(100, 30)
            };
            updateDataButton.Click += UpdateDataButton_Click;
            userInfoPanel.Controls.Add(updateDataButton);

            // Charts and graphs
            GunaChart weightTrendChart = new GunaChart
            {
                Size = new Size(780, 200),
                Location = new Point(10, 280)
            };
            userInfoPanel.Controls.Add(weightTrendChart);

            GunaChart bodyFatChart = new GunaChart
            {
                Size = new Size(380, 200),
                Location = new Point(10, 490)
            };
            userInfoPanel.Controls.Add(bodyFatChart);

            GunaChart bmiComparisonChart = new GunaChart
            {
                Size = new Size(380, 200),
                Location = new Point(410, 490)
            };
            userInfoPanel.Controls.Add(bmiComparisonChart);

            // Add data to weight trend chart
            GunaLineDataset weightDataset = new GunaLineDataset
            {
                Label = "Weight Trend",
                DataPoints = { new LPoint("Current Weight", kilo), new LPoint("Ideal Weight", idealKilo) }
            };
            weightTrendChart.Datasets.Add(weightDataset);

            // Add data to body fat chart
            GunaPieDataset bodyFatDataset = new GunaPieDataset
            {
                Label = "Body Fat",
                DataPoints = { new LPoint("Body Fat", yagOrani), new LPoint("Lean Mass", 100 - yagOrani) }
            };
            bodyFatChart.Datasets.Add(bodyFatDataset);

            // Add data to BMI comparison chart
            GunaBarDataset bmiDataset = new GunaBarDataset
            {
                Label = "BMI Comparison",
                DataPoints = { new LPoint("Current BMI", kilo / (boy / 100 * boy / 100)), new LPoint("Ideal BMI", 22) }
            };
            bmiComparisonChart.Datasets.Add(bmiDataset);

            // Tooltip system
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(lblHeight, "User's height in meters.");
            toolTip.SetToolTip(lblWeight, "User's weight in kilograms.");
            toolTip.SetToolTip(lblBodyFat, "User's body fat percentage.");
            toolTip.SetToolTip(lblBMI, "User's Body Mass Index.");
            toolTip.SetToolTip(lblIdealWeight, "User's ideal weight based on height.");
            toolTip.SetToolTip(lblMetabolicAge, "User's metabolic age compared to actual age.");
        }

        private void UpdateDataButton_Click(object sender, EventArgs e)
        {
            try
            {
                ClearPanels();
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    string query = "SELECT weight, height, birth_date FROM Member WHERE id = @userId";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", this.userId);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                double kilo = reader.GetDouble(0);
                                double boy = reader.GetDouble(1);
                                DateTime birthdate = reader.GetDateTime(2);
                                int yas = DateTime.Now.Year - birthdate.Year;
                                if (DateTime.Now.DayOfYear < birthdate.DayOfYear)
                                    yas--;

                                double yagOrani;
                                yagOrani = (1.20 * (kilo / (boy / 100 * boy / 100))) + (0.23 * yas) - 16.2;
                                double idealKilo = 22 * ((boy / 100) * (boy / 100));

                                // Clear previous details
                                this.Controls.Clear();
                                InitializeComponent();

                                ShowYagOraniDetails(kilo, boy, yas, yagOrani, idealKilo);
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

        private void KaloriBtn_Click(object sender, EventArgs e)
        {
            try
            {
                ClearPanels();
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
                this.Controls.Add(this.appointmentsDataGridView);
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
                Size = new Size(800, 650),
                Location = new Point(500, 50),
                BorderColor = Color.Black,
                BorderThickness = 1
            };
            this.Controls.Add(panel);

            GunaChart exerciseChart = new GunaChart
            {
                Size = new Size(780, 650),
                Location = new Point(10, 0)
            };
            panel.Controls.Add(exerciseChart);

            GunaStackedBarDataset exerciseDataset = new GunaStackedBarDataset { Label = "Egzersiz Bazında Kalori" };
            foreach (var entry in exerciseCalories)
                exerciseDataset.DataPoints.Add(new LPoint(entry.Key, entry.Value));
            exerciseChart.Datasets.Add(exerciseDataset);
        }
    }
}
