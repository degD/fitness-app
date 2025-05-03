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
        public LoginPage()
        {
            InitializeComponent();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // get name, password, email, phone, id, birthdate, weight, height, gender
            // and validate them
            // if they are valid, insert them into the database

            // validate them
            if (string.IsNullOrEmpty(nameBox.Text) || string.IsNullOrEmpty(mailBox.Text) || string.IsNullOrEmpty(phoneBox.Text) || string.IsNullOrEmpty(idBox.Text) || string.IsNullOrEmpty(birthdatePicker.Text) || string.IsNullOrEmpty(weightBox.Text) || string.IsNullOrEmpty(heightBox.Text))
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

            // check if the user already exists
            if (Program.userExists(idBox.Text))
            {
                MessageBox.Show("User already exists");
                return;
            }

            // check birthdate
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

            // insert the user into the database


        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
