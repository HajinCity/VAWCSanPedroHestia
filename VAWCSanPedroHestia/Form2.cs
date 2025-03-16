using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Firestore;

namespace VAWCSanPedroHestia
{
    public partial class Form2 : Form
    {
        private FirestoreDb firestoreDb;
        private string loggedInUsername; // Store logged-in username

        public Form2(string username) // Receive username from Form3
        {
            InitializeComponent();
            loggedInUsername = username;
            InitializeFirebase();
            LoadUserData();
        }

        private void InitializeFirebase()
        {
            try
            {
                string path = @"C:\Users\WINDOWS 10\source\repos\HajinCity\VAWCSanPedroHestia\VAWCSanPedroHestia\FirebaseJSONFile\vawc-hestiaxisanpedro2025-firebase-adminsdk-fbsvc-89e0f144fb.json";
                Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
                firestoreDb = FirestoreDb.Create("vawc-hestiaxisanpedro2025");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Firebase Init Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadUserData()
        {
            try
            {
                // 🔹 Check if the user document exists
                DocumentReference docRef = firestoreDb.Collection("users").Document(loggedInUsername);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists) // ✅ Check if user exists
                {
                    Dictionary<string, object> userData = snapshot.ToDictionary();

                    string firstName = userData.ContainsKey("Firstname") ? userData["Firstname"].ToString() :
                                       userData.ContainsKey("firstname") ? userData["firstname"].ToString() : "Unknown";

                    string lastName = userData.ContainsKey("Lastname") ? userData["Lastname"].ToString() :
                                      userData.ContainsKey("lastname") ? userData["lastname"].ToString() : "Unknown";

                    string fullName = $"{firstName} {lastName}";

                    label1.Text = fullName; // ✅ Display full name in label1
                }
                else
                {
                    MessageBox.Show("User data not found in Firestore!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching user data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
