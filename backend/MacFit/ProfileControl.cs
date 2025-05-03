using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace MacFit
{
    public partial class ProfileControl: UserControl
    {
        private string userId, mail, phone, name;
        private int weight, height;
        private NpgsqlTypes.NpgsqlDate birthdate;
        private double points;
        private string password;
        private string connString;

        public ProfileControl(String userId, String connString)
        {
            this.userId = userId;
            this.connString = connString;

            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Load user (member) data from the database
            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    string query = "SELECT * FROM Member WHERE id = @userId";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", this.userId);
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                this.mail = reader.GetString(1);
                                this.phone = reader.GetString(2);
                                this.name = reader.GetString(3);
                                this.weight = reader.GetInt32(4);
                                this.height = reader.GetInt32(5);
                                this.birthdate = reader.GetFieldValue<NpgsqlTypes.NpgsqlDate>(6);
                                this.points = reader.GetDouble(7);
                                this.password = reader.GetString(8);
                                Console.WriteLine("mail: " + this.mail);

                                EmailBox.Text = this.mail;
                                PhoneBox.Text = this.phone;
                                WBox.Value = this.weight;
                                HBox.Value = this.height;
                                OldPassBox.Text = "";
                                NewPassBox.Text = "";
                                IdLbl.Text = this.userId;
                                NameLbl.Text = this.name;
                                BirthLbl.Text = this.birthdate.ToString();
                                PointsLbl.Text = this.points.ToString();
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // update password button
            // Update password in OldPassBox and NewPassBox, reject if same
            if (string.IsNullOrWhiteSpace(OldPassBox.Text) || string.IsNullOrWhiteSpace(NewPassBox.Text))
            {
                MessageBox.Show("Please fill in all password fields.");
                return;
            }
            if (OldPassBox.Text == NewPassBox.Text)
            {
                MessageBox.Show("New password cannot be the same as old password.");
                return;
            }

            // Plain text password check, TODO: change later on
            if (OldPassBox.Text != this.password)
            {
                MessageBox.Show("Old password is incorrect.");
                return;
            }

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = "UPDATE Member SET password = @newPassword WHERE id = @userId AND password = @oldPassword";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", this.userId);
                        cmd.Parameters.AddWithValue("@oldPassword", OldPassBox.Text);
                        cmd.Parameters.AddWithValue("@newPassword", NewPassBox.Text);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Password updated successfully.");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void WBox_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // update profile button
            // Update the profile information in the database
            if (string.IsNullOrWhiteSpace(EmailBox.Text) || string.IsNullOrWhiteSpace(PhoneBox.Text))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }
            if (WBox.Value <= 0 || HBox.Value <= 0)
            {
                MessageBox.Show("Please enter valid weight and height.");
                return;
            }
            if (EmailBox.Text.Length > 50 || PhoneBox.Text.Length > 50)
            {
                MessageBox.Show("Email and phone number must be less than 50 characters.");
                return;
            }
            if (PhoneBox.Text.Length != 11)
            {
                MessageBox.Show("Phone number must be 11 digits.");
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(PhoneBox.Text, @"^\d+$"))
            {
                MessageBox.Show("Phone number must contain only digits.");
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(EmailBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Invalid email format.");
                return;
            }
            if (WBox.Value < 30 || WBox.Value > 300)
            {
                MessageBox.Show("Weight must be between 30 and 300 kg.");
                return;
            }
            if (HBox.Value < 100 || HBox.Value > 250)
            {
                MessageBox.Show("Height must be between 100 and 250 cm.");
                return;
            }
            if (PhoneBox.Text[0] != '0')
            {
                MessageBox.Show("Phone number must start with 0.");
                return;
            }

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = "UPDATE Member SET mail = @email, phone = @phone, weight = @weight, height = @height WHERE id = @userId";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", this.userId);
                        cmd.Parameters.AddWithValue("@email", EmailBox.Text);
                        cmd.Parameters.AddWithValue("@phone", PhoneBox.Text);
                        cmd.Parameters.AddWithValue("@weight", WBox.Value);
                        cmd.Parameters.AddWithValue("@height", HBox.Value);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Profile updated successfully.");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void EmailBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
