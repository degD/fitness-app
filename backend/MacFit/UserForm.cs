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
using System.Reflection;
using NpgsqlTypes;

namespace MacFit
{
    public partial class UserForm : Form
    {
        private string connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=fitnessdb";
        private string userId;

        private int odemePageNr = 0;
        private int odemePageNrCount = 0;
        private List<Odeme> odemeler = new List<Odeme>();

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

        private void SeansBtn_Click(object sender, EventArgs e)
        {
            ShowSessionSelector();
        }
        private void ShowSessionSelector()
        {
            ClearPanels();

            Guna2Panel sessionPanel = new Guna2Panel();
            sessionPanel.Size = new Size(800, 600);
            sessionPanel.Location = new Point(300, 10);
            sessionPanel.BorderColor = Color.Black;
            sessionPanel.BorderThickness = 1;
            this.Controls.Add(sessionPanel);

            Label label = new Label();
            label.Text = "Seans Seç:";
            label.Location = new Point(20, 20);
            label.AutoSize = true;
            sessionPanel.Controls.Add(label);

            Guna2ComboBox sessionSelector = new Guna2ComboBox();
            sessionSelector.Location = new Point(100, 15);
            sessionSelector.Size = new Size(300, 40);
            sessionPanel.Controls.Add(sessionSelector);

            DataGridView sessionDetailsGrid = new DataGridView();
            sessionDetailsGrid.Location = new Point(20, 70);
            sessionDetailsGrid.Size = new Size(750, 400);
            sessionDetailsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            sessionPanel.Controls.Add(sessionDetailsGrid);

            Dictionary<int, string> sessionMap = new Dictionary<int, string>();
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT id, date, \"start\", \"end\" FROM session WHERE status = 1";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            DateTime date = reader.GetDateTime(1);
                            TimeSpan start = reader.GetTimeSpan(2);
                            TimeSpan end = reader.GetTimeSpan(3);
                            string labelText = string.Format("{0} ({1:hh\\:mm} - {2:hh\\:mm})", date.ToShortDateString(), start, end);
                            sessionMap.Add(id, labelText);
                            sessionSelector.Items.Add(labelText);
                        }
                    }
                }
            }

            sessionSelector.SelectedIndexChanged += delegate (object s, EventArgs evt)
            {
                string selectedLabel = sessionSelector.SelectedItem.ToString();
                int selectedSessionId = -1;
                foreach (KeyValuePair<int, string> entry in sessionMap)
                {
                    if (entry.Value == selectedLabel)
                    {
                        selectedSessionId = entry.Key;
                        break;
                    }
                }
                if (selectedSessionId != -1)
                {
                    LoadSessionDetails(selectedSessionId, sessionDetailsGrid);
                }
            };
        }

        private void LoadSessionDetails(int sessionId, DataGridView grid)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Session Type");
            dt.Columns.Add("Total Capacity");
            dt.Columns.Add("Current Capacity");
            dt.Columns.Add("Trainer");

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = @"SELECT s.type, s.total_capacity, s.current_capacity, t.name FROM session s LEFT JOIN trainer t ON s.trainer_id = t.id WHERE s.id = @sessionId";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@sessionId", sessionId);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string type = "";
                            int typeVal = reader.GetInt32(0);
                            if (typeVal == 1) type = "Grup Seansı";
                            else if (typeVal == 2) type = "Bireysel Seans";
                            else if (typeVal == 3) type = "Antrenörlü Seans";

                            int total = reader.GetInt32(1);
                            int current = reader.GetInt32(2);
                            string trainer = reader.IsDBNull(3) ? "-" : reader.GetString(3);

                            dt.Rows.Add(type, total.ToString(), current.ToString(), trainer);
                        }
                    }
                }
            }

            grid.DataSource = dt;
        }

        private void WorkoutBtn_Click(object sender, EventArgs e)
        {
            ClearPanels();

            Guna2Panel workoutPanel = new Guna2Panel();
            workoutPanel.Size = new Size(800, 600);
            workoutPanel.Location = new Point(300, 10);
            workoutPanel.BorderColor = Color.Black;
            workoutPanel.BorderThickness = 1;
            this.Controls.Add(workoutPanel);

            Guna2ComboBox planSelector = new Guna2ComboBox();
            planSelector.Location = new Point(20, 20);
            planSelector.Size = new Size(300, 40);
            workoutPanel.Controls.Add(planSelector);

            DataGridView exercisesGrid = new DataGridView();
            exercisesGrid.Location = new Point(20, 80);
            exercisesGrid.Size = new Size(750, 400);
            exercisesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            workoutPanel.Controls.Add(exercisesGrid);

            Dictionary<int, string> planMap = new Dictionary<int, string>();
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT id, title FROM workout_plan WHERE member_id = @userId";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userId", userId);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string title = reader.GetString(1);
                            planMap.Add(id, title);
                            planSelector.Items.Add(title);
                        }
                    }
                }
            }

            planSelector.SelectedIndexChanged += delegate (object s, EventArgs evt)
            {
                string selectedTitle = planSelector.SelectedItem.ToString();
                int selectedPlanId = -1;
                foreach (KeyValuePair<int, string> entry in planMap)
                {
                    if (entry.Value == selectedTitle)
                    {
                        selectedPlanId = entry.Key;
                        break;
                    }
                }
                if (selectedPlanId != -1)
                {
                    LoadExercisesForPlan(selectedPlanId, exercisesGrid);
                }
            };
        }

        private void LoadExercisesForPlan(int planId, DataGridView grid)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Exercise");
            dt.Columns.Add("Reps");
            dt.Columns.Add("Sets");
            dt.Columns.Add("Weight");
            dt.Columns.Add("Calories Burnt");

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = @"SELECT e.name, wpe.reps, wpe.sets, wpe.weight, wpe.calories_burnt FROM workout_plan_exercise wpe JOIN exercise e ON e.id = wpe.exercise_id WHERE wpe.workout_plan_id = @planId";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@planId", planId);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dt.Rows.Add(reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4));
                        }
                    }
                }
            }

            grid.DataSource = dt;
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

        public class Odeme
        {
            public string cardNum;
            public int invoiceId;
            public double totalAmount;
            public double pointsUsed;
            public NpgsqlTypes.NpgsqlDate date;

            public Odeme(string cardNum, int invoiceId, double totalAmount, double pointsUsed, NpgsqlTypes.NpgsqlDate date)
            {
                this.cardNum = cardNum;
                this.invoiceId = invoiceId;
                this.totalAmount = totalAmount;
                this.pointsUsed = pointsUsed;
                this.date = date;
            }
        }

        private void OdemelerNextPageBt_Click(object sender, EventArgs e)
        {
            this.odemePageNr++;
            odemelerPrevPageBt.Enabled = true;
            if (this.odemePageNr == this.odemePageNrCount - 1)
            {
                odemelerNextPageBt.Enabled = false;
            }
            else
            {
                odemelerNextPageBt.Enabled = true;
            }
            LoadOdemelerPage();
            Console.WriteLine($"Current Page Number: {this.odemePageNr}");

        }

        private void OdemelerPrevPageBt_Click(object sender, EventArgs e)
        {
            this.odemePageNr--;
            odemelerNextPageBt.Enabled = true;
            if (this.odemePageNr == 0)
            {
                odemelerPrevPageBt.Enabled = false;
            }
            else
            {
                odemelerPrevPageBt.Enabled = true;
            }
            LoadOdemelerPage();
            Console.WriteLine($"Current Page Number: {this.odemePageNr}");
        }

        readonly Guna2Button odemelerPrevPageBt = new Guna2Button
        {
            Text = "Prev",
            BorderColor = Color.Red,
            BackColor = Color.Red,
            BorderThickness = 1,
            Location = new Point(0, 0),
            Size = new Size(100, 40),
            Enabled = false
        };

        readonly Guna2Button odemelerNextPageBt = new Guna2Button
        {
            Text = "Next",
            BorderColor = Color.Blue,
            BackColor = Color.Blue,
            BorderThickness = 1,
            Location = new Point(100, 0),
            Size = new Size(100, 40),
        };

        readonly Label odemelerPageInfoLbl = new Label
        {
            Text = "test",
            Location = new Point(200, 0),
            Size = new Size(100, 40),
            TextAlign = ContentAlignment.MiddleCenter,
        };

        // Main panel for user past payment info
        readonly Guna2Panel odemelerUserInfoPanel = new Guna2Panel
        {
            Size = new Size(800, 1000),
            BorderColor = Color.Black,
            BorderThickness = 1,
            Location = new Point(300, 40)
        };

        readonly Guna2Panel odemelerUserInfoBtPanel = new Guna2Panel
        {
            Size = new Size(300, 40),
            BorderColor = Color.Black,
            BorderThickness = 1,
            Location = new Point(300, 0)
        };

        private void GOdemelerBtn_Click(object sender, EventArgs e)
        {

            try
            {
                ClearPanels();

                odemelerNextPageBt.Click += OdemelerNextPageBt_Click;
                odemelerPrevPageBt.Click += OdemelerPrevPageBt_Click;

                odemelerUserInfoBtPanel.Controls.Add(odemelerPrevPageBt);
                odemelerUserInfoBtPanel.Controls.Add(odemelerNextPageBt);
                odemelerUserInfoBtPanel.Controls.Add(odemelerPageInfoLbl);
                this.Controls.Add(odemelerUserInfoPanel);
                this.Controls.Add(odemelerUserInfoBtPanel);


                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                       
                    odemeler.Clear();
                    string query = "SELECT card_number, invoice_id, total_amount, points_used, date, member_id FROM Transaction WHERE member_id = @userId";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", this.userId);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string cardNum = reader.GetString(0);
                                int invoiceId = reader.GetInt32(1);
                                double totalAmount = reader.GetDouble(2);
                                double pointsUsed = reader.GetDouble(3);
                                NpgsqlTypes.NpgsqlDate date = reader.GetDate(4);
                                odemeler.Add(new Odeme(cardNum, invoiceId, totalAmount, pointsUsed, date));
                            }
                            this.odemePageNrCount = 1 + this.odemeler.Count() / 4;
                            this.odemePageNr = 0;
                            LoadOdemelerPage();
                            Console.WriteLine($"count: {this.odemeler.Count()}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void LoadOdemelerPage()
        {
            int page = this.odemePageNr;

            odemelerUserInfoPanel.Controls.Clear(); // Clear all controls from userInfoPanel
            odemelerPageInfoLbl.Text = $"Page {page + 1} / {this.odemePageNrCount}";
            for (int i = page * 4; i < page * 4 + 4 && i < this.odemeler.Count; i++)
            {
                AddPastPayments(odemelerUserInfoPanel, i % 4, odemeler[i]);
            }
        }

        private void AddPastPayments(Guna2Panel userInfoPanel, int index, Odeme odeme)
        {
            string cardNum = odeme.cardNum;
            int invoiceId = odeme.invoiceId;
            double pointsUsed = odeme.pointsUsed;
            double totalAmount = odeme.totalAmount;
            NpgsqlTypes.NpgsqlDate date = odeme.date;

            // Info bar
            Guna2Panel infoBar = new Guna2Panel
            {
                Location = new Point(0, index * 160),
                Size = new Size(800, 160),
                BorderColor = Color.Black,
                BorderThickness = 1,
                AutoSize = true,
            };

            Label lbInvoice = new Label { Text = $"Invoice {invoiceId}", Location = new Point(20, 20) };
            Label lbTotalAmount = new Label { Text = $"Payment: {totalAmount}", Location = new Point(20, 50) };
            Label lbPoints = new Label { Text = $"Points Used: {pointsUsed}", Location = new Point(20, 80) };
            Label lbCardNum = new Label { Text = $"Card Number: {cardNum}", Location = new Point(20, 110), AutoSize = true };
            Label lbDate = new Label { Text = $"Date: {date.ToString()}", Location = new Point(20, 140) };

            //// Labels for user fitness information
            //Label lblHeight = new Label { Text = $"Height: {boy} m", Location = new Point(20, 20), AutoSize = true };
            //Label lblWeight = new Label { Text = $"Weight: {kilo} kg", Location = new Point(20, 50), AutoSize = true };
            //Label lblBodyFat = new Label { Text = $"Body Fat: {yagOrani:F2} %", Location = new Point(20, 80), AutoSize = true };
            //Label lblBMI = new Label { Text = $"BMI: {(kilo / (boy / 100 * boy / 100)):F2}", Location = new Point(20, 110), AutoSize = true };
            //Label lblIdealWeight = new Label { Text = $"Ideal Weight: {idealKilo:F2} kg", Location = new Point(20, 140), AutoSize = true };
            //Label lblMetabolicAge = new Label { Text = $"Metabolic Age: {yas}", Location = new Point(20, 170), AutoSize = true };

            infoBar.Controls.Add(lbInvoice);
            infoBar.Controls.Add(lbTotalAmount);
            infoBar.Controls.Add(lbPoints);
            infoBar.Controls.Add(lbCardNum);
            infoBar.Controls.Add(lbDate);
            userInfoPanel.Controls.Add(infoBar);

        }

        private void UyelikBtn_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ClearPanels();

            // Info bar
            Guna2Panel panel = new Guna2Panel
            {
                Location = new Point(200, 0),
                Size = new Size(800, 600),
                BorderColor = Color.Black,
                AutoSize = true,
            };
            panel.Controls.Add(new ProfileControl(userId, connString));
            this.Controls.Add(panel);
        }
    }
}
