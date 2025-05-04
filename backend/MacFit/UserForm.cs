using Npgsql;
using Guna.UI2.WinForms;
using Guna.Charts.WinForms;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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
            this.userId = userId;
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

        private void ShowMembershipAndSessionPaymentPanel()
        {
            ClearPanels();

            var panel = new Guna2Panel
            {
                Size = new Size(850, 600),
                Location = new Point(300, 10),
                BorderRadius = 10,
                BorderColor = Color.Gray,
                BorderThickness = 1,
                BackColor = Color.White
            };
            this.Controls.Add(panel);

            Label lblTitle = new Label
            {
                Text = "Seans Kaydı ve Ödeme",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblTitle);

            Label lblCard = new Label { Text = "Kart Seç:", Location = new Point(20, 60), AutoSize = true };
            panel.Controls.Add(lblCard);

            Guna2ComboBox cmbCard = new Guna2ComboBox { Location = new Point(100, 55), Size = new Size(300, 36) };
            panel.Controls.Add(cmbCard);

            Label lblSession = new Label { Text = "Seans Seç:", Location = new Point(20, 110), AutoSize = true };
            panel.Controls.Add(lblSession);

            Guna2ComboBox cmbSession = new Guna2ComboBox { Location = new Point(100, 105), Size = new Size(500, 36) };
            panel.Controls.Add(cmbSession);

            Guna2Button btnPayAndRegister = new Guna2Button
            {
                Text = "Ödeme Yap ve Seansı Ayır",
                Location = new Point(100, 160),
                Size = new Size(300, 45),
                BorderRadius = 10
            };
            panel.Controls.Add(btnPayAndRegister);

            Guna2DataGridView dgvPreviousSessions = new Guna2DataGridView
            {
                Location = new Point(20, 220),
                Size = new Size(800, 330),
                ReadOnly = true,
                AllowUserToAddRows = false,
                ColumnHeadersHeight = 30,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            panel.Controls.Add(dgvPreviousSessions);

            dgvPreviousSessions.Columns.Add("Tarih", "Tarih");
            dgvPreviousSessions.Columns.Add("Saat", "Saat");
            dgvPreviousSessions.Columns.Add("Tür", "Tür");
            dgvPreviousSessions.Columns.Add("Durum", "Durum");

            Dictionary<string, string> cardMap = new Dictionary<string, string>();
            Dictionary<string, int> sessionMap = new Dictionary<string, int>();

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                // Kartları getir
                using (var cmd = new NpgsqlCommand("SELECT title, number FROM card WHERE member_id = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string number = reader.GetString(1);
                            string last4 = number.Substring(number.Length - 4);
                            string display = reader.GetString(0) + " (" + last4 + ")";
                            cardMap[display] = number;
                            cmbCard.Items.Add(display);
                        }
                    }
                }

                // Seansları getir
                string sessionQuery = "SELECT id, date, \"start\", \"end\", type, current_capacity, total_capacity FROM session WHERE status = 1 AND current_capacity < total_capacity";
                using (var cmd = new NpgsqlCommand(sessionQuery, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        DateTime date = reader.GetDateTime(1);
                        TimeSpan start = reader.GetTimeSpan(2);
                        TimeSpan end = reader.GetTimeSpan(3);
                        int type = reader.GetInt32(4);
                        int current = reader.GetInt32(5);
                        int total = reader.GetInt32(6);

                        string typeLabel = type == 1 ? "Grup Seansı" :
                                           type == 2 ? "Bireysel Seans" :
                                           "Antrenörlü Seans";

                        string label = string.Format(
                            "{0:yyyy-MM-dd} | {1:hh\\:mm} - {2:hh\\:mm} | {3} ({4}/{5})",
                            date, start, end, typeLabel, current, total);

                        sessionMap[label] = id;
                        cmbSession.Items.Add(label);
                    }
                }

                // Kullanıcının geçmiş seanslarını getir
                string pastQuery = @"SELECT s.date, s.start, s.end, s.type, a.status
                             FROM appointment a
                             JOIN session s ON a.session_id = s.id
                             WHERE a.member_id = @id";
                using (var cmd = new NpgsqlCommand(pastQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string date = reader.GetDateTime(0).ToString("yyyy-MM-dd");
                            string saat = reader.GetTimeSpan(1).ToString(@"hh\:mm") + " - " + reader.GetTimeSpan(2).ToString(@"hh\:mm");
                            string type = reader.GetInt32(3) == 1 ? "Grup" : reader.GetInt32(3) == 2 ? "Bireysel" : "Antrenörlü";
                            string status = reader.GetInt32(4) == 1 ? "Aktif" : "Pasif";
                            dgvPreviousSessions.Rows.Add(date, saat, type, status);
                        }
                    }
                }
            }

            btnPayAndRegister.Click += (s, e) =>
            {
                if (cmbCard.SelectedIndex == -1 || cmbSession.SelectedIndex == -1)
                {
                    MessageBox.Show("Lütfen kart ve seans seçiniz.");
                    return;
                }

                string selectedCardDisplay = cmbCard.SelectedItem.ToString();
                string selectedCardNumber = cardMap[selectedCardDisplay];

                string selectedSessionDisplay = cmbSession.SelectedItem.ToString();
                int selectedSessionId = sessionMap[selectedSessionDisplay];

                double amount = 150.0;
                int invoiceId = new Random().Next(2000, 9999); // örnek için

                using (var checkConn = new NpgsqlConnection(connString))
                {
                    checkConn.Open();
                    string checkQuery = "SELECT COUNT(*) FROM appointment WHERE member_id = @mid AND session_id = @sid";
                    using (var checkCmd = new NpgsqlCommand(checkQuery, checkConn))
                    {
                        checkCmd.Parameters.AddWithValue("@mid", userId);
                        checkCmd.Parameters.AddWithValue("@sid", selectedSessionId);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            MessageBox.Show("Bu seansı zaten seçtiniz. Lütfen başka bir seans seçin.");
                            return;
                        }
                    }
                }

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            // Transaction ekle
                            string insertTransaction = @"
                        INSERT INTO transaction (member_id, card_number, invoice_id, total_amount, points_used, date)
                        VALUES (@mid, @cnum, @inv, @total, @points, CURRENT_DATE)";
                            using (var cmd = new NpgsqlCommand(insertTransaction, conn))
                            {
                                cmd.Parameters.AddWithValue("@mid", userId);
                                cmd.Parameters.AddWithValue("@cnum", selectedCardNumber);
                                cmd.Parameters.AddWithValue("@inv", invoiceId);
                                cmd.Parameters.AddWithValue("@total", amount);
                                cmd.Parameters.AddWithValue("@points", 0);
                                cmd.ExecuteNonQuery();
                            }

                            // Appointment oluştur
                            string insertAppointment = @"
                        INSERT INTO appointment (id, session_id, member_id, status, date, workout_plan_id)
                        VALUES (@id, @sid, @mid, 1, CURRENT_DATE, NULL)";
                            using (var cmd = new NpgsqlCommand(insertAppointment, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", invoiceId);
                                cmd.Parameters.AddWithValue("@sid", selectedSessionId);
                                cmd.Parameters.AddWithValue("@mid", userId);
                                cmd.ExecuteNonQuery();
                            }

                            // current_capacity artır
                            using (var cmd = new NpgsqlCommand("UPDATE session SET current_capacity = current_capacity + 1 WHERE id = @id", conn))
                            {
                                cmd.Parameters.AddWithValue("@id", selectedSessionId);
                                cmd.ExecuteNonQuery();
                            }

                            tran.Commit();
                            MessageBox.Show("Ödeme ve seans kaydı başarılı!");
                            ShowMembershipAndSessionPaymentPanel(); // ekranı yenile
                        }
                        catch (Exception ex)
                        {
                            tran.Rollback();
                            MessageBox.Show("Hata oluştu: " + ex.Message);
                        }
                    }
                }
            };
        }



        private void SeansOdemeBtn_Click(object sender, EventArgs e)
        {
            ShowMembershipAndSessionPaymentPanel();
        }

        private string GetCategory(string exercise)
        {
            var categoryMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Squat", "Leg" }, { "Deadlift", "Leg" }, { "Lunges", "Leg" },
                { "Leg Press", "Leg" }, { "Leg Curl", "Leg" }, { "Hip Thrust", "Leg" },
                { "Calf Raise", "Leg" },

                { "Push-up", "Chest" }, { "Bench Press", "Chest" },
                { "Tricep Dips", "Chest" }, { "Dumbell Press", "Chest" },
                { "Dumbell Incline Press", "Chest" },

                { "Pull-up", "Back" }, { "Dumbbell Row", "Back" },
                { "Barbell Row", "Back" }, { "Machine Row", "Back" },
                { "T-Row", "Back" },

                { "Shoulder Press", "Shoulder" }, { "Lateral Raise", "Shoulder" },
                { "Machine Press", "Shoulder" },

                { "Bicep Curl", "Arms" }, { "Dumbell Curl", "Arms" },
                { "Triceps Pushdown", "Arms" }, { "Rope Pushdown", "Arms" },

                { "Plank", "Abs" }, { "Russian Twist", "Abs" },
                { "Mountain Climbers", "Abs" }, { "Kettlebell Swing", "Abs" },
                { "Ab Roll-out", "Abs" }
            };

            return categoryMap.TryGetValue(exercise, out var category) ? category : "Other";
        }

        private void ShowWorkoutPlanManagerForUser()
        {
            ClearPanels();

            var panel = new Guna2Panel
            {
                Size = new Size(850, 600),
                Location = new Point(300, 10),
                BorderRadius = 10,
                BorderColor = Color.Gray,
                BorderThickness = 1,
                BackColor = Color.White
            };
            this.Controls.Add(panel);

            Label title = new Label
            {
                Text = "Workout Planlarım",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(title);

            var dgvPlans = new Guna2DataGridView
            {
                Location = new Point(20, 50),
                Size = new Size(400, 200),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            panel.Controls.Add(dgvPlans);

            var dgvDetails = new Guna2DataGridView
            {
                Location = new Point(20, 270),
                Size = new Size(800, 230),
                ReadOnly = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            panel.Controls.Add(dgvDetails);

            dgvDetails.Columns.Add("Egzersiz", "Egzersiz");
            dgvDetails.Columns.Add("Set", "Set");
            dgvDetails.Columns.Add("Tekrar", "Tekrar");
            dgvDetails.Columns.Add("Ağırlık", "Ağırlık");
            dgvDetails.Columns.Add("Kalori", "Kalori");

            int selectedPlanId = -1;

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT id AS \"Plan ID\", title AS \"Plan Adı\" FROM workout_plan ORDER BY id";
                var da = new NpgsqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@userId", userId);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvPlans.DataSource = dt;
            }

            dgvPlans.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    selectedPlanId = Convert.ToInt32(dgvPlans.Rows[e.RowIndex].Cells["Plan ID"].Value);
                    dgvDetails.Rows.Clear();

                    using (var conn = new NpgsqlConnection(connString))
                    {
                        conn.Open();
                        string query = @"SELECT e.name, wpe.sets, wpe.reps, wpe.weight, wpe.calories_burnt 
                                 FROM workout_plan_exercise wpe 
                                 JOIN exercise e ON wpe.exercise_id = e.id 
                                 WHERE wpe.workout_plan_id = @id";
                        using (var cmd = new NpgsqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", selectedPlanId);
                            using (var reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string name = reader.GetString(0);
                                    string sets = reader.IsDBNull(1) ? "" : reader.GetInt32(1).ToString();
                                    string reps = reader.IsDBNull(2) ? "" : reader.GetInt32(2).ToString();
                                    string weight = reader.IsDBNull(3) ? "" : reader.GetInt32(3).ToString();
                                    string cal = reader.IsDBNull(4) ? "" : reader.GetInt32(4).ToString();
                                    dgvDetails.Rows.Add(name, sets, reps, weight, cal);
                                }
                            }
                        }
                    }
                }
            };
        }

        private void ShowWorkoutPlanCreatorPanel()
        {
            ClearPanels();

            var panel = new Guna2Panel
            {
                Size = new Size(850, 600),
                Location = new Point(300, 10),
                BorderRadius = 15,
                BorderColor = Color.Gray,
                BorderThickness = 1,
                BackColor = Color.White
            };
            this.Controls.Add(panel);

            Label lblTitle = new Label
            {
                Text = "Workout Plan Oluştur",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblTitle);

            var txtPlanName = new Guna2TextBox
            {
                PlaceholderText = "Plan İsmi",
                Location = new Point(20, 60),
                Size = new Size(300, 35),
                BorderRadius = 8
            };
            panel.Controls.Add(txtPlanName);

            var cmbCategory = new Guna2ComboBox
            {
                Location = new Point(20, 110),
                Size = new Size(200, 35),
                BorderRadius = 8,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategory.Items.AddRange(new string[] { "Leg", "Chest", "Back", "Shoulder", "Arms", "Abs" });
            panel.Controls.Add(cmbCategory);

            var lstExercises = new ListBox
            {
                Location = new Point(20, 160),
                Size = new Size(200, 200)
            };
            panel.Controls.Add(lstExercises);

            var dgvSelected = new Guna2DataGridView
            {
                Location = new Point(240, 110),
                Size = new Size(580, 250),
                ReadOnly = true,
                AllowUserToAddRows = false,
                ColumnCount = 5
            };
            dgvSelected.Columns[0].Name = "Egzersiz";
            dgvSelected.Columns[1].Name = "Set";
            dgvSelected.Columns[2].Name = "Tekrar";
            dgvSelected.Columns[3].Name = "Ağırlık";
            dgvSelected.Columns[4].Name = "Kalori";
            panel.Controls.Add(dgvSelected);

            var txtSet = new Guna2TextBox { PlaceholderText = "Set", Location = new Point(20, 370), Size = new Size(60, 30) };
            var txtReps = new Guna2TextBox { PlaceholderText = "Tekrar", Location = new Point(90, 370), Size = new Size(60, 30) };
            var txtWeight = new Guna2TextBox { PlaceholderText = "Kg", Location = new Point(160, 370), Size = new Size(60, 30) };
            var txtCal = new Guna2TextBox { PlaceholderText = "Kalori", Location = new Point(230, 370), Size = new Size(60, 30) };
            var btnAdd = new Guna2Button { Text = "Ekle", Location = new Point(300, 370), Size = new Size(80, 30) };

            panel.Controls.Add(txtSet);
            panel.Controls.Add(txtReps);
            panel.Controls.Add(txtWeight);
            panel.Controls.Add(txtCal);
            panel.Controls.Add(btnAdd);

            var btnSave = new Guna2Button
            {
                Text = "Planı Kaydet",
                Location = new Point(650, 370),
                Size = new Size(170, 40),
                BorderRadius = 10
            };
            panel.Controls.Add(btnSave);

            cmbCategory.SelectedIndexChanged += (s, e) =>
            {
                lstExercises.Items.Clear();
                string selected = cmbCategory.SelectedItem.ToString();

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT name FROM exercise";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.GetString(0);
                            if (GetCategory(name) == selected)
                            {
                                lstExercises.Items.Add(name);
                            }
                        }
                    }
                }
            };

            btnAdd.Click += (s, e) =>
            {
                if (lstExercises.SelectedItem == null) return;
                dgvSelected.Rows.Add(lstExercises.SelectedItem.ToString(), txtSet.Text, txtReps.Text, txtWeight.Text, txtCal.Text);
            };

            btnSave.Click += (s, e) =>
            {
                if (txtPlanName.Text.Trim() == "" || dgvSelected.Rows.Count == 0)
                {
                    MessageBox.Show("Plan ismi ve egzersizler girilmelidir.");
                    return;
                }

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    string insertPlan = "INSERT INTO workout_plan (id, title, member_id) VALUES ((SELECT COALESCE(MAX(id), 0) + 1 FROM workout_plan), @title, @userId) RETURNING id";
                    int planId = 0;
                    using (var cmd = new NpgsqlCommand(insertPlan, conn))
                    {
                        cmd.Parameters.AddWithValue("@title", txtPlanName.Text);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        planId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    foreach (DataGridViewRow row in dgvSelected.Rows)
                    {
                        if (row.IsNewRow) continue;
                        string exerciseName = row.Cells[0].Value.ToString();

                        int exerciseId;
                        using (var cmd = new NpgsqlCommand("SELECT id FROM exercise WHERE name = @name", conn))
                        {
                            cmd.Parameters.AddWithValue("@name", exerciseName);
                            exerciseId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        string insertDetail = @"INSERT INTO workout_plan_exercise 
                    (workout_plan_id, exercise_id, sets, reps, weight, calories_burnt) 
                    VALUES (@planId, @exId, @sets, @reps, @weight, @cal)";
                        using (var cmd = new NpgsqlCommand(insertDetail, conn))
                        {
                            cmd.Parameters.AddWithValue("@planId", planId);
                            cmd.Parameters.AddWithValue("@exId", exerciseId);
                            cmd.Parameters.AddWithValue("@sets", int.TryParse(row.Cells[1].Value?.ToString(), out int sets) ? sets : 0);
                            cmd.Parameters.AddWithValue("@reps", int.TryParse(row.Cells[2].Value?.ToString(), out int reps) ? reps : 0);
                            cmd.Parameters.AddWithValue("@weight", int.TryParse(row.Cells[3].Value?.ToString(), out int weight) ? weight : 0);
                            cmd.Parameters.AddWithValue("@cal", int.TryParse(row.Cells[4].Value?.ToString(), out int cal) ? cal : 0);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Plan başarıyla oluşturuldu.");
                    WorkoutBtn_Click(null, null); // Plan kaydından sonra geri dön
                }
            };
        }

        private void WorkoutBtn_Click(object sender, EventArgs e)
        {
            ClearPanels();

            Guna2Panel workoutPanel = new Guna2Panel
            {
                Size = new Size(850, 600),
                Location = new Point(300, 10),
                BorderColor = Color.Black,
                BorderThickness = 1,
                BorderRadius = 15,
                BackColor = Color.White
            };
            this.Controls.Add(workoutPanel);

            Label lblTitle = new Label
            {
                Text = "Workout Plan İşlemleri",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point((workoutPanel.Width - 250) / 2, 30)
            };
            workoutPanel.Controls.Add(lblTitle);

            // Yeni Plan Oluştur Butonu
            Guna2Button createPlanBtn = new Guna2Button
            {
                Text = "Yeni Plan Oluştur",
                Size = new Size(200, 50),
                BorderRadius = 10,
                FillColor = Color.DodgerBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // Planları Görüntüle / Güncelle / Sil Butonu
            Guna2Button btnManagePlans = new Guna2Button
            {
                Text = "Planları Görüntüle",
                Size = new Size(300, 50),
                BorderRadius = 10,
                FillColor = Color.ForestGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            // Ortalamak için konumları ayarla
            int totalWidth = createPlanBtn.Width + 20 + btnManagePlans.Width;
            int startX = (workoutPanel.Width - totalWidth) / 2;

            createPlanBtn.Location = new Point(startX, 100);
            btnManagePlans.Location = new Point(startX + createPlanBtn.Width + 20, 100);

            workoutPanel.Controls.Add(createPlanBtn);
            workoutPanel.Controls.Add(btnManagePlans);

            // Event binding
            btnManagePlans.Click += (s, evt) => ShowWorkoutPlanManagerForUser();
            createPlanBtn.Click += (s, evt) => ShowWorkoutPlanCreatorPanel();
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
            ShowCalorieCalculatorPanel();
        }
        private void ShowCalorieCalculatorPanel()
        {
            ClearPanels();

            var panel = new Guna2Panel
            {
                Size = new Size(850, 600),
                Location = new Point(300, 10),
                BorderRadius = 15,
                BorderColor = Color.Gray,
                BorderThickness = 1,
                BackColor = Color.White
            };
            this.Controls.Add(panel);

            Label lblTitle = new Label
            {
                Text = "Kalori İhtiyacı Hesapla",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };
            panel.Controls.Add(lblTitle);

            // Labels
            Label lblActivity = new Label { Text = "Aktivite Düzeyi:", Location = new Point(20, 60), AutoSize = true };
            panel.Controls.Add(lblActivity);
            Guna2ComboBox cmbActivity = new Guna2ComboBox
            {
                Location = new Point(150, 55),
                Size = new Size(300, 35),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items =
        {
            "Sedanter (Hareketsiz)",
            "Orta Aktivite (2-3 Gün Spor)",
            "Hareketli (4-5 Gün Spor)"
        }
            };
            panel.Controls.Add(cmbActivity);

            Label lblGoal = new Label { Text = "Hedef:", Location = new Point(20, 110), AutoSize = true };
            panel.Controls.Add(lblGoal);
            Guna2ComboBox cmbGoal = new Guna2ComboBox
            {
                Location = new Point(150, 105),
                Size = new Size(300, 35),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items =
        {
            "Yağ Kaybetme",
            "Kas Kazanma",
            "Kütle Koruma"
        }
            };
            panel.Controls.Add(cmbGoal);

            // Hesapla Butonu
            Guna2Button btnCalculate = new Guna2Button
            {
                Text = "Hesapla",
                Location = new Point(470, 105),
                Size = new Size(100, 35),
                BorderRadius = 8
            };
            panel.Controls.Add(btnCalculate);

            // Sonuç alanı
            Label lblResult = new Label
            {
                Location = new Point(20, 160),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(800, 30)
            };
            panel.Controls.Add(lblResult);

            GunaChart chart = new GunaChart
            {
                Size = new Size(400, 300),
                Location = new Point(20, 200)
            };
            panel.Controls.Add(chart);

            btnCalculate.Click += (s, e) =>
            {
                double kilo = 0, boy = 0, yagOrani = 0;
                int yas = 0;
                string gender = "";

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT weight, height, birth_date, gender FROM Member WHERE id = @id";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", this.userId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                kilo = Convert.ToDouble(reader[0]);
                                boy = Convert.ToDouble(reader[1]);
                                DateTime dogum = reader.GetDateTime(2);
                                yas = DateTime.Now.Year - dogum.Year;
                                if (DateTime.Now.DayOfYear < dogum.DayOfYear)
                                    yas--;
                                gender = reader.GetString(3);
                                yagOrani = (1.20 * (kilo / (boy / 100 * boy / 100))) + (0.23 * yas) - 16.2;
                            }
                        }
                    }
                }

                double bmr = gender == "Erkek" ? (10 * kilo + 6.25 * boy - 5 * yas + 5) : (10 * kilo + 6.25 * boy - 5 * yas - 161);
                double activityFactor = 1.2; // default

                switch (cmbActivity.SelectedIndex)
                {
                    case 0:
                        activityFactor = 1.2;
                        break;
                    case 1:
                        activityFactor = 1.375;
                        break;
                    case 2:
                        activityFactor = 1.55;
                        break;
                }

                double calorieNeed = bmr * activityFactor;

                switch (cmbGoal.SelectedIndex)
                {
                    case 0: calorieNeed *= 0.85; break; // yağ kaybı
                    case 1: calorieNeed *= 1.15; break; // kas kazanımı
                }

                int cal = (int)calorieNeed;
                lblResult.Text = $"Tahmini günlük kalori ihtiyacınız: {cal} kcal";

                int carb = (int)(cal * 0.5 / 4);
                int protein = (int)(cal * 0.3 / 4);
                int fat = (int)(cal * 0.2 / 9);

                chart.Datasets.Clear();
                chart.Datasets.Add(new GunaPieDataset
                {
                    Label = "Makro Dağılım",
                    DataPoints =
            {
                new LPoint("Karbonhidrat", carb),
                new LPoint("Protein", protein),
                new LPoint("Yağ", fat)
            }
                });
            };
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
                this.odemePageNr = 0;

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

            // enable/disable buttons
            if (this.odemePageNr == 0)
            {

                odemelerPrevPageBt.Enabled = this.odemePageNr > 0;
                odemelerNextPageBt.Enabled = this.odemePageNr < this.odemePageNrCount - 1;
                odemelerPrevPageBt.Enabled = false;
            }
            if (this.odemePageNr == this.odemePageNrCount)
            {
                odemelerNextPageBt.Enabled = false;
            }
            if (this.odemePageNr > 0 && this.odemePageNr < this.odemePageNrCount - 1)
            {
                odemelerNextPageBt.Enabled = true;
                odemelerPrevPageBt.Enabled = true;
            }
        }

        private void AddPastPayments(Guna2Panel userInfoPanel, int index, Odeme odeme)
        {
            string cardNum = odeme.cardNum;
            int invoiceId = odeme.invoiceId;
            double pointsUsed = odeme.pointsUsed;
            double totalAmount = odeme.totalAmount;
            NpgsqlTypes.NpgsqlDate date = odeme.date;

            Guna2Panel cardPanel = new Guna2Panel
            {
                Location = new Point(20, 20 + index * 170),
                Size = new Size(760, 150),
                BorderRadius = 15,
                BorderThickness = 1,
                BorderColor = Color.LightGray,
                FillColor = Color.White,
                ShadowDecoration = { Enabled = true, Shadow = new Padding(3, 3, 5, 5) }
            };

            Label lblInvoice = new Label
            {
                Text = $"💳 Fatura No: {invoiceId}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };

            Label lblAmount = new Label
            {
                Text = $"💰 Tutar: {totalAmount} ₺",
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                Location = new Point(20, 45),
                AutoSize = true
            };

            Label lblCard = new Label
            {
                Text = $"💳 Kart: **** **** **** {cardNum.Substring(cardNum.Length - 4)}",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                Location = new Point(20, 105),
                AutoSize = true
            };

            Label lblDate = new Label
            {
                Text = $"🗓 Tarih: {date}",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(550, 110),
                AutoSize = true
            };

            cardPanel.Controls.Add(lblInvoice);
            cardPanel.Controls.Add(lblAmount);
            cardPanel.Controls.Add(lblCard);
            cardPanel.Controls.Add(lblDate);

            userInfoPanel.Controls.Add(cardPanel);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ClearPanels();

            Guna2Panel profilePanel = new Guna2Panel
            {
                Location = new Point(200, 0),
                Size = new Size(1200, 800),
                BorderColor = Color.Black,
                BorderThickness = 1,
                BackColor = Color.White
            };
            this.Controls.Add(profilePanel);

            Label lblProfileTitle = new Label
            {
                Text = "Profil Bilgileri",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };
            profilePanel.Controls.Add(lblProfileTitle);

            // Bilgi alanları
            Label lblName = new Label { Text = "Ad Soyad:", Location = new Point(20, 70) };
            Guna2TextBox txtName = new Guna2TextBox { Location = new Point(150, 65), Size = new Size(200, 30) };

            Label lblMail = new Label { Text = "E-Posta:", Location = new Point(20, 110) };
            Guna2TextBox txtMail = new Guna2TextBox { Location = new Point(150, 105), Size = new Size(200, 30) };

            Label lblWeight = new Label { Text = "Kilo:", Location = new Point(20, 150) };
            Guna2TextBox txtWeight = new Guna2TextBox { Location = new Point(150, 145), Size = new Size(200, 30) };

            Label lblHeight = new Label { Text = "Boy:", Location = new Point(20, 190) };
            Guna2TextBox txtHeight = new Guna2TextBox { Location = new Point(150, 185), Size = new Size(200, 30) };

            Guna2Button btnUpdate = new Guna2Button
            {
                Text = "Bilgileri Güncelle",
                Location = new Point(150, 230),
                Size = new Size(200, 35),
                BorderRadius = 10
            };

            profilePanel.Controls.Add(lblName);
            profilePanel.Controls.Add(txtName);
            profilePanel.Controls.Add(lblMail);
            profilePanel.Controls.Add(txtMail);
            profilePanel.Controls.Add(lblWeight);
            profilePanel.Controls.Add(txtWeight);
            profilePanel.Controls.Add(lblHeight);
            profilePanel.Controls.Add(txtHeight);
            profilePanel.Controls.Add(btnUpdate);

            // Mevcut bilgileri doldur
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT name, mail, weight, height FROM member WHERE id = @id";
                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtName.Text = reader.GetString(0);
                            txtMail.Text = reader.GetString(1);
                            txtWeight.Text = reader.GetDouble(2).ToString("F1");
                            txtHeight.Text = reader.GetDouble(3).ToString("F1");
                        }
                    }
                }
            }

            btnUpdate.Click += (s, evt) =>
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string updateQuery = "UPDATE member SET name = @name, mail = @mail, weight = @weight, height = @height WHERE id = @id";
                    using (var cmd = new NpgsqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", txtName.Text);
                        cmd.Parameters.AddWithValue("@mail", txtMail.Text);
                        cmd.Parameters.AddWithValue("@weight", double.Parse(txtWeight.Text));
                        cmd.Parameters.AddWithValue("@height", double.Parse(txtHeight.Text));
                        cmd.Parameters.AddWithValue("@id", userId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Bilgiler başarıyla güncellendi!");
            };

            // Kredi Kartı Bilgileri
            Label lblCards = new Label { Text = "Kayıtlı Kartlar:", Location = new Point(400, 70), Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            ListBox lstCards = new ListBox { Location = new Point(400, 100), Size = new Size(300, 100) };
            profilePanel.Controls.Add(lblCards);
            profilePanel.Controls.Add(lstCards);

            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string cardQuery = "SELECT title, number FROM card WHERE member_id = @id";
                using (var cmd = new NpgsqlCommand(cardQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@id", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string title = reader.GetString(0);
                            string number = reader.GetString(1);
                            lstCards.Items.Add($"{title} - **** **** **** {number.Substring(number.Length - 4)}");
                        }
                    }
                }
            }

            // Yeni Kart Ekle
            Label lblNewCard = new Label { Text = "Yeni Kart Ekle:", Location = new Point(400, 220), Font = new Font("Segoe UI", 12, FontStyle.Bold) };
            Guna2TextBox txtCardTitle = new Guna2TextBox { PlaceholderText = "Kart Adı", Location = new Point(400, 260), Size = new Size(200, 30) };
            Guna2TextBox txtCardNumber = new Guna2TextBox { PlaceholderText = "Kart Numarası", Location = new Point(400, 300), Size = new Size(200, 30) };
            Guna2Button btnAddCard = new Guna2Button
            {
                Text = "Kartı Ekle",
                Location = new Point(400, 340),
                Size = new Size(200, 35),
                BorderRadius = 10
            };

            profilePanel.Controls.Add(lblNewCard);
            profilePanel.Controls.Add(txtCardTitle);
            profilePanel.Controls.Add(txtCardNumber);
            profilePanel.Controls.Add(btnAddCard);

            btnAddCard.Click += (s, evt) =>
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string insertQuery = "INSERT INTO card (member_id, title, number) VALUES (@id, @title, @number)";
                    using (var cmd = new NpgsqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", userId);
                        cmd.Parameters.AddWithValue("@title", txtCardTitle.Text);
                        cmd.Parameters.AddWithValue("@number", txtCardNumber.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Kart başarıyla eklendi!");
                guna2Button1_Click(null, null); // sayfayı yenile
            };
        }

        private void UyelikBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
