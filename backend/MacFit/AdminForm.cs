using Guna.UI2.WinForms;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MacFit
{
    public partial class AdminForm : Form
    {
        private string connString = "Host=localhost;Port=5432;Username=postgres;Password=123456;Database=fitnessdb";

        public AdminForm()
        {
            InitializeComponent();
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            ClearPanels();

            // Info bar
            Guna2Panel panel = new Guna2Panel
            {
                Location = new Point(300, 0),
                Size = new Size(800, 600),
                BorderColor = Color.Black,
                AutoSize = true,
            };
            panel.Controls.Add(new AdminPasswordControl(connString));
            this.Controls.Add(panel);
        }

        private void UyelikBtn_Click(object sender, EventArgs e)
        {
            ShowTrainerPanel();
        }
        private void ShowTrainerPanel()
        {
            ClearPanels();

            var panel = new Guna2Panel
            {
                Size = new Size(850, 600),
                Location = new Point(300, 10),
                BorderRadius = 15,
                BorderColor = Color.Silver,
                BorderThickness = 1,
                BackColor = Color.White
            };
            this.Controls.Add(panel);

            var container = new Guna2Panel
            {
                Size = new Size(800, 200),
                Location = new Point(25, 20),
                BorderRadius = 12,
                FillColor = Color.FromArgb(245, 245, 245)
            };
            panel.Controls.Add(container);

            Label lblTitle = new Label
            {
                Text = "Antrenör Ekle / Güncelle / Sil",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 15),
                AutoSize = true
            };
            container.Controls.Add(lblTitle);

            var txtName = new Guna2TextBox
            {
                PlaceholderText = "Ad Soyad",
                Location = new Point(20, 60),
                Size = new Size(250, 40),
                BorderRadius = 10,
                ForeColor = Color.Black,
                PlaceholderForeColor = Color.DimGray
            };
            container.Controls.Add(txtName);

            var txtPhone = new Guna2TextBox
            {
                PlaceholderText = "Telefon",
                Location = new Point(290, 60),
                Size = new Size(200, 40),
                BorderRadius = 10,
                ForeColor = Color.Black,
                PlaceholderForeColor = Color.DimGray
            };
            container.Controls.Add(txtPhone);

            var btnSave = new Guna2Button
            {
                Text = "Kaydet",
                Location = new Point(510, 60),
                Size = new Size(100, 40),
                BorderRadius = 10
            };
            container.Controls.Add(btnSave);

            var btnUpdate = new Guna2Button
            {
                Text = "Güncelle",
                Location = new Point(620, 60),
                Size = new Size(100, 40),
                BorderRadius = 10,
                Enabled = false,
                FillColor = Color.SeaGreen,
                HoverState = { FillColor = Color.MediumSeaGreen },
                ForeColor = Color.White
            };
            container.Controls.Add(btnUpdate);

            var btnDelete = new Guna2Button
            {
                Text = "Sil",
                Location = new Point(730, 60),
                Size = new Size(60, 40),
                FillColor = Color.Firebrick,
                BorderRadius = 10,
                Enabled = false
            };
            container.Controls.Add(btnDelete);

            var lblStatus = new Label
            {
                Location = new Point(20, 110),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.DarkGreen
            };
            container.Controls.Add(lblStatus);

            var dgv = new Guna2DataGridView
            {
                Location = new Point(25, 240),
                Size = new Size(800, 330),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                ThemeStyle = {
            AlternatingRowsStyle = { BackColor = Color.WhiteSmoke }
        },
                RowTemplate = { Height = 28 }
            };
            panel.Controls.Add(dgv);

            LoadTrainerList(dgv);

            // Kolon başlıklarını düzelt
            dgv.Columns[0].HeaderText = "Id";
            dgv.Columns[1].HeaderText = "Name";
            dgv.Columns[2].HeaderText = "Phone";

            int selectedId = -1;

            dgv.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    selectedId = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["id"].Value);
                    txtName.Text = dgv.Rows[e.RowIndex].Cells["name"].Value.ToString();
                    txtPhone.Text = dgv.Rows[e.RowIndex].Cells["phone"].Value.ToString();
                    btnUpdate.Enabled = btnDelete.Enabled = true;
                }
            };

            btnSave.Click += (s, e) =>
            {
                if (txtName.Text.Trim() == "" || txtPhone.Text.Trim() == "")
                {
                    lblStatus.ForeColor = Color.Red;
                    lblStatus.Text = "Tüm alanları doldurun.";
                    return;
                }

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = "INSERT INTO trainer (id, name, phone) VALUES ((SELECT COALESCE(MAX(id), 0) + 1 FROM trainer), @name, @phone)";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                        cmd.ExecuteNonQuery();
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Text = "Antrenör eklendi.";
                        txtName.Clear(); txtPhone.Clear();
                        LoadTrainerList(dgv);
                    }
                }
            };

            btnUpdate.Click += (s, e) =>
            {
                if (selectedId == -1) return;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = "UPDATE trainer SET name = @name, phone = @phone WHERE id = @id";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedId);
                        cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                        cmd.ExecuteNonQuery();
                        lblStatus.ForeColor = Color.Green;
                        lblStatus.Text = "Antrenör güncellendi.";
                        txtName.Clear(); txtPhone.Clear();
                        selectedId = -1;
                        btnUpdate.Enabled = btnDelete.Enabled = false;
                        LoadTrainerList(dgv);
                    }
                }
            };

            btnDelete.Click += (s, e) =>
            {
                if (selectedId == -1) return;

                if (MessageBox.Show("Silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (var conn = new NpgsqlConnection(connString))
                    {
                        conn.Open();
                        string query = "DELETE FROM trainer WHERE id = @id";
                        using (var cmd = new NpgsqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", selectedId);
                            cmd.ExecuteNonQuery();
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Antrenör silindi.";
                            txtName.Clear(); txtPhone.Clear();
                            selectedId = -1;
                            btnUpdate.Enabled = btnDelete.Enabled = false;
                            LoadTrainerList(dgv);
                        }
                    }
                }
            };
        }



        private void LoadTrainerList(DataGridView grid)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = "SELECT id, name, phone FROM trainer ORDER BY id";
                using (var da = new NpgsqlDataAdapter(query, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    grid.DataSource = dt;
                }
            }
        }

        private void SeansBtn_Click(object sender, EventArgs e)
        {
            ShowSessionAdminPanel();
        }

        private void ShowSessionAdminPanel()
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
                Text = "Seans Yönetimi",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 10),
                AutoSize = true
            };
            panel.Controls.Add(title);

            // Guna2DataGridView
            var sessionGrid = new Guna2DataGridView
            {
                Location = new Point(20, 50),
                Size = new Size(800, 300),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ThemeStyle = { AlternatingRowsStyle = { BackColor = Color.WhiteSmoke } }
            };
            panel.Controls.Add(sessionGrid);

            LoadSessionGrid(sessionGrid);

            // Güncelleme alanları
            Guna2TextBox txtCapacity = new Guna2TextBox
            {
                PlaceholderText = "Kapasite",
                Location = new Point(20, 370),
                Size = new Size(100, 30)
            };
            panel.Controls.Add(txtCapacity);

            Guna2ComboBox cmbStatus = new Guna2ComboBox
            {
                Location = new Point(140, 370),
                Size = new Size(150, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.Add("Aktif");
            cmbStatus.Items.Add("Dolu");
            cmbStatus.StartIndex = 0;
            panel.Controls.Add(cmbStatus);

            Guna2Button btnUpdate = new Guna2Button
            {
                Text = "Güncelle",
                Location = new Point(320, 370),
                Size = new Size(120, 30)
            };
            panel.Controls.Add(btnUpdate);

            Guna2Button btnDelete = new Guna2Button
            {
                Text = "Sil",
                Location = new Point(460, 370),
                Size = new Size(120, 30),
                FillColor = Color.Firebrick
            };
            panel.Controls.Add(btnDelete);

            Guna2Button btnAdd = new Guna2Button
            {
                Text = "Yeni Seans Ekle",
                Location = new Point(600, 370),
                Size = new Size(150, 30),
                FillColor = Color.SeaGreen
            };
            panel.Controls.Add(btnAdd);

            int selectedSessionId = -1;

            sessionGrid.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    selectedSessionId = Convert.ToInt32(sessionGrid.Rows[e.RowIndex].Cells["id"].Value);
                    txtCapacity.Text = sessionGrid.Rows[e.RowIndex].Cells["total_capacity"].Value.ToString();

                    string statusText = sessionGrid.Rows[e.RowIndex].Cells["status"].Value.ToString();
                    cmbStatus.SelectedIndex = statusText == "Aktif" ? 0 : 1;
                }
            };

            btnUpdate.Click += (s, e) =>
            {
                if (selectedSessionId == -1) return;

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = "UPDATE session SET total_capacity = @cap, status = @status WHERE id = @id";
                    var cmd = new NpgsqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@cap", int.Parse(txtCapacity.Text));
                    cmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString() == "Aktif" ? 1 : 0);
                    cmd.Parameters.AddWithValue("@id", selectedSessionId);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Seans güncellendi.");
                LoadSessionGrid(sessionGrid);
                txtCapacity.Clear();
                selectedSessionId = -1;
            };

            btnDelete.Click += (s, e) =>
            {
                if (selectedSessionId == -1) return;

                DialogResult result = MessageBox.Show("Bu seansı silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    using (var conn = new NpgsqlConnection(connString))
                    {
                        conn.Open();
                        string query = "DELETE FROM session WHERE id = @id";
                        var cmd = new NpgsqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@id", selectedSessionId);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Seans silindi.");
                    LoadSessionGrid(sessionGrid);
                    txtCapacity.Clear();
                    selectedSessionId = -1;
                }
            };

            btnAdd.Click += (s, e) =>
            {
                ShowSessionCreatePanel(); // Bu daha önce oluşturduğumuz seans ekleme paneli olmalı
            };
        }

        private void ShowSessionCreatePanel()
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

            int x = 30, y = 30, spacing = 50;

            Label title = new Label
            {
                Text = "Yeni Seans Oluştur",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(x, y),
                AutoSize = true
            };
            panel.Controls.Add(title);
            y += spacing;

            // Tarih
            panel.Controls.Add(new Label { Text = "Tarih:", Location = new Point(x, y + 5), AutoSize = true });
            DateTimePicker datePicker = new DateTimePicker
            {
                Location = new Point(x + 130, y),
                Format = DateTimePickerFormat.Short,
                Width = 200
            };
            panel.Controls.Add(datePicker);

            // Başlangıç Saati
            y += spacing;
            panel.Controls.Add(new Label { Text = "Başlangıç Saati:", Location = new Point(x, y + 5), AutoSize = true });
            DateTimePicker startTime = new DateTimePicker
            {
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true,
                Location = new Point(x + 130, y),
                Width = 200
            };
            panel.Controls.Add(startTime);

            // Bitiş Saati
            y += spacing;
            panel.Controls.Add(new Label { Text = "Bitiş Saati:", Location = new Point(x, y + 5), AutoSize = true });
            DateTimePicker endTime = new DateTimePicker
            {
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true,
                Location = new Point(x + 130, y),
                Width = 200
            };
            panel.Controls.Add(endTime);

            startTime.Value = DateTime.Today.AddHours(10); // 10:00
            endTime.Value = DateTime.Today.AddHours(11);   // 11:00


            // Seans Tipi
            y += spacing;
            panel.Controls.Add(new Label { Text = "Seans Tipi:", Location = new Point(x, y + 5), AutoSize = true });
            Guna2ComboBox cmbType = new Guna2ComboBox
            {
                Location = new Point(x + 130, y),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbType.Items.AddRange(new string[] { "1 - Grup", "2 - Bireysel", "3 - Antrenörlü" });
            panel.Controls.Add(cmbType);

            // Kapasite
            y += spacing;
            panel.Controls.Add(new Label { Text = "Toplam Kapasite:", Location = new Point(x, y + 5), AutoSize = true });
            Guna2TextBox txtCapacity = new Guna2TextBox
            {
                PlaceholderText = "örn. 10",
                Location = new Point(x + 130, y),
                Size = new Size(200, 30)
            };
            panel.Controls.Add(txtCapacity);

            // Antrenör
            y += spacing;
            panel.Controls.Add(new Label { Text = "Antrenör:", Location = new Point(x, y + 5), AutoSize = true });
            Guna2ComboBox cmbTrainer = new Guna2ComboBox
            {
                Location = new Point(x + 130, y),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };
            panel.Controls.Add(cmbTrainer);

            // Kaydet Butonu
            y += spacing + 10;
            Guna2Button btnSave = new Guna2Button
            {
                Text = "Seansı Kaydet",
                Location = new Point(x + 130, y),
                Size = new Size(200, 40),
                FillColor = Color.SeaGreen
            };
            panel.Controls.Add(btnSave);

            // Antrenörleri çek
            Dictionary<int, string> trainerMap = new Dictionary<int, string>();
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                var cmd = new NpgsqlCommand("SELECT id, name FROM trainer", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    cmbTrainer.Items.Add(name);
                    trainerMap[id] = name;
                }
            }

            // Seans tipi seçimine göre antrenör aktif/pasif
            cmbType.SelectedIndexChanged += (s, e) =>
            {
                cmbTrainer.Enabled = cmbType.SelectedItem.ToString().StartsWith("3");
                if (!cmbTrainer.Enabled) cmbTrainer.SelectedIndex = -1;
            };

            // Kaydet Butonu
            btnSave.Click += (s, e) =>
            {
                if (!int.TryParse(txtCapacity.Text, out int cap) || cmbType.SelectedIndex == -1)
                {
                    MessageBox.Show("Lütfen tüm alanları doğru şekilde doldurun.");
                    return;
                }

                int type = int.Parse(cmbType.SelectedItem.ToString().Split('-')[0].Trim());
                object trainerId = DBNull.Value;

                if (type == 3 && cmbTrainer.SelectedIndex >= 0)
                {
                    string selected = cmbTrainer.SelectedItem.ToString();
                    trainerId = trainerMap.FirstOrDefault(t => t.Value == selected).Key;
                }

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string insert = @"
                         INSERT INTO session 
                         (id, type, total_capacity, current_capacity, status, date, ""start"", ""end"", trainer_id)
                         VALUES ((SELECT COALESCE(MAX(id), 0) + 1 FROM session), @type, @cap, 0, 1, @date, @start, @end, @trainer)";
                    var cmd = new NpgsqlCommand(insert, conn);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@cap", cap);
                    cmd.Parameters.AddWithValue("@date", datePicker.Value.Date);
                    cmd.Parameters.AddWithValue("@start", startTime.Value.TimeOfDay);
                    cmd.Parameters.AddWithValue("@end", endTime.Value.TimeOfDay);
                    cmd.Parameters.AddWithValue("@trainer", trainerId ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Seans başarıyla oluşturuldu.");
                ShowSessionAdminPanel(); // Oluşturduktan sonra listeye dön
            };
        }



        private void LoadSessionGrid(Guna2DataGridView grid)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                string query = @"
            SELECT 
                s.id AS ID, 
                s.date AS Tarih, 
                s.start AS Başlangıç, 
                s.end AS Bitiş, 
                CASE 
                    WHEN s.type = 1 THEN 'Grup' 
                    WHEN s.type = 2 THEN 'Bireysel' 
                    WHEN s.type = 3 THEN 'Antrenörlü' 
                END AS Tür,
                s.total_capacity AS ""Toplam Kontenjan"", 
                s.current_capacity AS ""Kalan Kontenjan"", 
                CASE 
                    WHEN s.status = 1 THEN 'Aktif' 
                    ELSE 'Dolu' 
                END AS Durum,
                t.name AS Antrenör
            FROM session s
            LEFT JOIN trainer t ON s.trainer_id = t.id
            ORDER BY s.id;";

                var da = new NpgsqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                grid.DataSource = dt;
            }

            // Genel görünüm: tüm sütunlar yayılsın
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Ama bazı önemli sütunlar sığmazsa özel ayar ver (AllCells)
            foreach (DataGridViewColumn col in grid.Columns)
            {
                string name = col.HeaderText;

                if (name == "Toplam Kontenjan" || name == "Kalan Kontenjan" || name == "Antrenör" || name == "Tarih")
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                // Başlığın ilk harfini büyük yap
                col.HeaderText = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);
            }

            // Satır yüksekliğini biraz büyütüp okunaklı hale getir
            grid.RowTemplate.Height = 28;
        }

        private void KaloriBtn_Click(object sender, EventArgs e)
        {
            ShowMemberListPanel();
        }

        private void ShowMemberListPanel()
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
                Text = "Üye Listesi",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 10),
                AutoSize = true
            };
            panel.Controls.Add(title);

            Guna2DataGridView memberGrid = new Guna2DataGridView
            {
                Location = new Point(20, 50),
                Size = new Size(800, 500),
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ThemeStyle = { AlternatingRowsStyle = { BackColor = Color.WhiteSmoke } }
            };
            panel.Controls.Add(memberGrid);

            LoadMemberGrid(memberGrid);
        }

        private void LoadMemberGrid(Guna2DataGridView grid)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string query = @"
            SELECT 
                id AS ""TC Kimlik"",
                name AS ""Ad Soyad"",
                mail AS ""E-posta"",
                phone AS ""Telefon"",
                weight AS ""Kilo"",
                height AS ""Boy"",
                birth_date AS ""Doğum Tarihi"",
                gender AS ""Cinsiyet""
            FROM member
            WHERE type IS DISTINCT FROM 1
            ORDER BY name;";

                var da = new NpgsqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                grid.DataSource = dt;
            }

            // Genel olarak tüm alanı doldur
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Kritik kolonlara özel genişlik ayarı
            grid.Columns["TC Kimlik"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grid.Columns["Ad Soyad"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grid.Columns["E-posta"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            grid.Columns["Telefon"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            // Header'ların ilk harfini büyük yap
            foreach (DataGridViewColumn col in grid.Columns)
            {
                col.HeaderText = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(col.HeaderText);
            }

            // Satır yüksekliğini biraz büyüt
            grid.RowTemplate.Height = 28;
        }



    }
}
