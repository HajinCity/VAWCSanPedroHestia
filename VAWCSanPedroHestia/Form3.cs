using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VAWCSanPedroHestia
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
          //  MessageBox.Show("Connected to Firestore!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            await LoginUser(username, password);
        }

        private async Task LoginUser(string username, string password)
        {
            try
            {
            
                DocumentReference docRef = FirebaseInitialization.Database.Collection("users").Document(username);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists) 
                {
                    Dictionary<string, object> userData = snapshot.ToDictionary();
                    string storedPassword = userData.ContainsKey("password") ? userData["password"].ToString() : "";

                    if (storedPassword == password) 
                    {
                        MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Form2 dashboard = new Form2(username);
                        this.Hide();
                        dashboard.Show();
                    }
                    else
                    {
                        MessageBox.Show("Invalid password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("User not found in Firestore!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during login: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    
    }
}
