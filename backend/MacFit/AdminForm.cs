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
            ClearPanels(); // Önceki paneli temizle

            Guna2Panel trainerPanel = new Guna2Panel
            {
                Size = new Size(800, 600),
                Location = new Point(300, 10)
            };
            this.Controls.Add(trainerPanel);

            // Label, Textbox ve Save butonları
            Label lblTitle = new Label { Text = "Yeni Antrenör Kaydı", Font = new Font("Segoe UI", 14, FontStyle.Bold), Location = new Point(20, 20), AutoSize = true };
            trainerPanel.Controls.Add(lblTitle);

            Label lblName = new Label { Text = "Ad Soyad:", Location = new Point(20, 70), AutoSize = true };
            trainerPanel.Controls.Add(lblName);
            TextBox txtName = new TextBox { Location = new Point(100, 65), Size = new Size(250, 30) };
            trainerPanel.Controls.Add(txtName);

            Label lblPhone = new Label { Text = "Telefon:", Location = new Point(20, 110), AutoSize = true };
            trainerPanel.Controls.Add(lblPhone);
            TextBox txtPhone = new TextBox { Location = new Point(100, 105), Size = new Size(250, 30) };
            trainerPanel.Controls.Add(txtPhone);

            Button btnSave = new Button { Text = "Kaydet", Location = new Point(100, 150), Size = new Size(100, 30) };
            trainerPanel.Controls.Add(btnSave);

            Button btnUpdate = new Button { Text = "Güncelle", Location = new Point(210, 150), Size = new Size(100, 30), Enabled = false };
            trainerPanel.Controls.Add(btnUpdate);

            Button btnDelete = new Button { Text = "Sil", Location = new Point(320, 150), Size = new Size(100, 30), Enabled = false };
            trainerPanel.Controls.Add(btnDelete);

            Label lblStatus = new Label { ForeColor = Color.DarkGreen, Location = new Point(20, 190), AutoSize = true };
            trainerPanel.Controls.Add(lblStatus);

            DataGridView dgvTrainers = new DataGridView
            {
                Location = new Point(20, 230),
                Size = new Size(750, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            trainerPanel.Controls.Add(dgvTrainers);

            LoadTrainerList(dgvTrainers); // Listeyi yükle

            int selectedTrainerId = -1;

            dgvTrainers.CellClick += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    selectedTrainerId = Convert.ToInt32(dgvTrainers.Rows[e.RowIndex].Cells["Id"].Value);
                    txtName.Text = dgvTrainers.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                    txtPhone.Text = dgvTrainers.Rows[e.RowIndex].Cells["Phone"].Value.ToString();
                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                }
            };

            btnSave.Click += (s, e) =>
            {
                string name = txtName.Text.Trim();
                string phone = txtPhone.Text.Trim();

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone))
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
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Antrenör başarıyla kaydedildi.";
                            txtName.Clear();
                            txtPhone.Clear();
                            LoadTrainerList(dgvTrainers);
                        }
                        catch (Exception ex)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Hata: " + ex.Message;
                        }
                    }
                }
            };

            btnUpdate.Click += (s, e) =>
            {
                if (selectedTrainerId == -1) return;

                string updatedName = txtName.Text.Trim();
                string updatedPhone = txtPhone.Text.Trim();

                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = "UPDATE trainer SET name = @name, phone = @phone WHERE id = @id";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedTrainerId);
                        cmd.Parameters.AddWithValue("@name", updatedName);
                        cmd.Parameters.AddWithValue("@phone", updatedPhone);
                        try
                        {
                            cmd.ExecuteNonQuery();
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Text = "Antrenör güncellendi.";
                            LoadTrainerList(dgvTrainers);
                            txtName.Clear();
                            txtPhone.Clear();
                            btnUpdate.Enabled = false;
                            btnDelete.Enabled = false;
                            selectedTrainerId = -1;
                        }
                        catch (Exception ex)
                        {
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Text = "Hata: " + ex.Message;
                        }
                    }
                }
            };

            btnDelete.Click += (s, e) =>
            {
                if (selectedTrainerId == -1) return;

                var result = MessageBox.Show("Bu antrenörü silmek istediğinizden emin misiniz?", "Onay", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    using (var conn = new NpgsqlConnection(connString))
                    {
                        conn.Open();
                        string query = "DELETE FROM trainer WHERE id = @id";
                        using (var cmd = new NpgsqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", selectedTrainerId);
                            try
                            {
                                cmd.ExecuteNonQuery();
                                lblStatus.ForeColor = Color.Green;
                                lblStatus.Text = "Antrenör silindi.";
                                LoadTrainerList(dgvTrainers);
                                txtName.Clear();
                                txtPhone.Clear();
                                btnUpdate.Enabled = false;
                                btnDelete.Enabled = false;
                                selectedTrainerId = -1;
                            }
                            catch (Exception ex)
                            {
                                lblStatus.ForeColor = Color.Red;
                                lblStatus.Text = "Hata: " + ex.Message;
                            }
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









    }
}
