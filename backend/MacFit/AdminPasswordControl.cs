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
    public partial class AdminPasswordControl: UserControl
    {
        private string connString;

        public AdminPasswordControl(String connString)
        {
            this.connString = connString;
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AdminOldPassBox.Text) || string.IsNullOrWhiteSpace(AdminNewPassBox.Text))
            {
                MessageBox.Show("Please fill in all password fields.");
                return;
            }
            if (AdminOldPassBox.Text == AdminNewPassBox.Text)
            {
                MessageBox.Show("New password cannot be the same as old password.");
                return;
            }

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = "SELECT password_hash FROM admin_password";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        string hashedPassword = cmd.ExecuteScalar() as string;

                        // Ensure the fetched password is not null or invalid  
                        if (string.IsNullOrWhiteSpace(hashedPassword) || !hashedPassword.StartsWith("$2"))
                        {
                            MessageBox.Show("Stored password is invalid.");
                            return;
                        }

                        if (!BCrypt.Net.BCrypt.Verify(AdminOldPassBox.Text, hashedPassword))
                        {
                            MessageBox.Show("Old password is incorrect.");
                            return;
                        }
                    }

                    string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(AdminNewPassBox.Text);
                    string updateQuery = "UPDATE admin_password SET password_hash = @newHash";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@newHash", hashedNewPassword);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Password updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
