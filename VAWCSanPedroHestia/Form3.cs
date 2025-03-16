using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VAWCSanPedroHestia
{
    public partial class Form3 : Form
    {
        private FirestoreDb firestoreDb;
        public Form3()
        {
            InitializeComponent();
            InitializeFirebase();
        }
        private void InitializeFirebase()
        {
            try
            {
                string path = @"C:\Users\WINDOWS 10\source\repos\HajinCity\VAWCSanPedroHestia\VAWCSanPedroHestia\FirebaseJSONFile\vawc-hestiaxisanpedro2025-firebase-adminsdk-fbsvc-89e0f144fb.json";
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                firestoreDb = FirestoreDb.Create("vawc-hestiaxisanpedro2025");
                MessageBox.Show("Connected to Firestore!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Firebase Init Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void button1_Click(object sender, EventArgs e) // Login Button Click
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
                DocumentReference docRef = firestoreDb.Collection("users").Document(username);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    Dictionary<string, object> userData = snapshot.ToDictionary();
                    string storedPassword = userData["password"].ToString();

                    if (storedPassword == password) // 🔹 Password check (Consider hashing in real apps)
                    {
                        MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Open Form2 (Main Application)
                        Form2 dashboard = new Form2();
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
                    MessageBox.Show("User not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during login: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
