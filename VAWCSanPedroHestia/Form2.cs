using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Firestore;
using Newtonsoft.Json.Linq;
using VAWCSanPedroHestia.NewForm;

namespace VAWCSanPedroHestia
{
    public partial class Form2 : Form
    {
        private readonly string loggedInUsername;
        private Form activeForm;

        public Form2(string username)
        {
            InitializeComponent();
            loggedInUsername = username;
            LoadUserData();
            home_btn_Click(null, EventArgs.Empty);
        }

        private async void LoadUserData()
        {
            try
            {
                DocumentReference docRef = FirebaseInitialization.Database
                    .Collection("users")
                    .Document(loggedInUsername);

                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    Dictionary<string, object> userData = snapshot.ToDictionary();

                    string firstName = userData.ContainsKey("Firstname") ? userData["Firstname"].ToString() :
                                       userData.ContainsKey("firstname") ? userData["firstname"].ToString() : "Unknown";

                    string lastName = userData.ContainsKey("Lastname") ? userData["Lastname"].ToString() :
                                      userData.ContainsKey("lastname") ? userData["lastname"].ToString() : "Unknown";

                    label1.Text = $"{firstName} {lastName}";
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

        private void openingForm(Form childForm)
        {
            if (activeForm != null)

                activeForm.Close();

            activeForm = childForm;

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            panel3.Controls.Add(childForm);
            panel3.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }



        private void File_btn_Click(object sender, EventArgs e)
        {
            openingForm(new FileACaseUI());
        }

        private void home_btn_Click(object sender, EventArgs e)
        {
            openingForm(new HomeUI());
        }

        private void case_btn_Click(object sender, EventArgs e)
        {
            openingForm(new Case_List());
        }

        private void managecase_btn_Click(object sender, EventArgs e)
        {
            openingForm(new ManageCaseListIU());
        }

        private void report_btn_Click(object sender, EventArgs e)
        {
            openingForm(new QuarterlyAccomplishmentReport());
        }

        private void management_btn_Click(object sender, EventArgs e)
        {
            openingForm(new SystemManage());
        }

        private  void call_btn_Click(object sender, EventArgs e)
        {

            openingForm(new QuarterlyAccomplishmentReport()); 
        }



    }


}
