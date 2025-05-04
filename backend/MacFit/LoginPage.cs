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
    public partial class LoginPage : Form
    {
        private string connString;

        public LoginPage(string connString)
        {
            this.connString = connString;
            InitializeComponent();
            passwordBox.PasswordChar = '●';
            passwordBox.UseSystemPasswordChar = true;
            LoginPasswordBox.PasswordChar = '●';
            LoginPasswordBox.UseSystemPasswordChar = true;
            AdminPassBox.PasswordChar = '●';
            AdminPassBox.UseSystemPasswordChar = true;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private bool userExists(string text)
        {
            // Ensure the connection string is properly passed
            string connectionString = this.connString;

            // Use a standard using block instead of using declarations for compatibility with C# 7.3
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                // Use the provided text parameter instead of hardcoding the username
                using (var cmd = new NpgsqlCommand("SELECT user_exists(@username)", conn))
                {
                    cmd.Parameters.AddWithValue("username", text);

                    // Ensure a return value is provided
                    bool exists = (bool)cmd.ExecuteScalar();
                    Console.WriteLine(exists ? "User exists." : "User does not exist.");
                    return exists;
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Validate input fields  
            if (string.IsNullOrEmpty(nameBox.Text) || string.IsNullOrEmpty(mailBox.Text) || string.IsNullOrEmpty(phoneBox.Text) || 
                string.IsNullOrEmpty(idBox.Text) || string.IsNullOrEmpty(birthdatePicker.Text) || 
                string.IsNullOrEmpty(weightBox.Text) || string.IsNullOrEmpty(heightBox.Text))
            {
                MessageBox.Show("Please fill all the fields");
                return;
            }
            if (weightBox.Value <= 0 || heightBox.Value <= 0)
            {
                MessageBox.Show("Please enter valid weight and height.");
                return;
            }
            if (mailBox.Text.Length > 50 || phoneBox.Text.Length > 50)
            {
                MessageBox.Show("Email and phone number must be less than 50 characters.");
                return;
            }
            if (phoneBox.Text.Length != 11)
            {
                MessageBox.Show("Phone number must be 11 digits.");
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(phoneBox.Text, @"^\d+$"))
            {
                MessageBox.Show("Phone number must contain only digits.");
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(mailBox.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Invalid email format.");
                return;
            }
            if (weightBox.Value < 30 || weightBox.Value > 300)
            {
                MessageBox.Show("Weight must be between 30 and 300 kg.");
                return;
            }
            if (heightBox.Value < 100 || heightBox.Value > 250)
            {
                MessageBox.Show("Height must be between 100 and 250 cm.");
                return;
            }
            if (phoneBox.Text[0] != '0')
            {
                MessageBox.Show("Phone number must start with 0.");
                return;
            }

            // Check if the user already exists  
            if (this.userExists(idBox.Text))
            {
                MessageBox.Show("User already exists");
                return;
            }

            // Check birthdate  
            DateTime birthdate = birthdatePicker.Value;
            if (birthdate != null)
            {
                DateTime today = DateTime.Today;
                int age = today.Year - birthdate.Year;
                if (birthdate > today.AddYears(-age)) age--;
                if (age < 18)
                {
                    MessageBox.Show("You must be at least 18 years old to register.");
                    return;
                }
            }

            // Hash the password  
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordBox.Text);

            try
            {
                using (NpgsqlConnection conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    string query = "INSERT INTO Member (id, name, mail, phone, birth_date, weight, height, gender, password, type) " +
                                   "VALUES (@id, @name, @mail, @phone, @birthdate, @weight, @height, @gender, @password, 0)";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idBox.Text);
                        cmd.Parameters.AddWithValue("@name", nameBox.Text);
                        cmd.Parameters.AddWithValue("@mail", mailBox.Text);
                        cmd.Parameters.AddWithValue("@phone", phoneBox.Text);
                        cmd.Parameters.AddWithValue("@birthdate", birthdate);
                        cmd.Parameters.AddWithValue("@weight", weightBox.Value);
                        cmd.Parameters.AddWithValue("@height", heightBox.Value);
                        cmd.Parameters.AddWithValue("@gender", genderBox.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("User registered successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // login control  
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    // 1. Query the hashed password for the given username  
                    string query = "SELECT password FROM Member WHERE id = @userId";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", LoginIdBox.Text);

                        object result = cmd.ExecuteScalar();

                        // 2. Check if user exists  
                        if (result == null)
                        {
                            MessageBox.Show("User not found.");
                            return;
                        }

                        string storedHash = result.ToString();

                        // 3. Verify the input password against the stored hash  
                        bool isMatch = BCrypt.Net.BCrypt.Verify(LoginPasswordBox.Text, storedHash);

                        if (isMatch)
                        {
                            MessageBox.Show("Login successful.");

                            var mainForm = new UserForm(LoginIdBox.Text);

                            // Hide current form and open main form
                            this.Hide();

                            mainForm.FormClosed += (s, args) => this.Close(); // closes login form when main closes
                            mainForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Invalid password.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            // validate admin password (hashed)
            try
            {
                using (var conn = new NpgsqlConnection(connString))
                {
                    conn.Open();

                    // 1. Query the hashed password for the given username  
                    string query = "SELECT password_hash FROM admin_password";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        string storedHash = result.ToString();

                        // 3. Verify the input password against the stored hash  
                        bool isMatch = BCrypt.Net.BCrypt.Verify(AdminPassBox.Text, storedHash);

                        if (isMatch)
                        {
                            MessageBox.Show("Admin login successful.");

                            var mainForm = new AdminForm();

                            // Hide current form and open main form
                            this.Hide();

                            mainForm.FormClosed += (s, args) => this.Close(); // closes login form when main closes
                            mainForm.Show();
                        }
                        else
                        {
                            MessageBox.Show("Invalid password.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void nameBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            // Close the application
            Application.Exit();
        }
    }
}
