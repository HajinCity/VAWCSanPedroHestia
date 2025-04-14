using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using VAWCSanPedroHestia.NewForm;

namespace VAWCSanPedroHestia
{
    public partial class HomeUI : Form
    {
        private readonly HttpClient httpClient = new HttpClient();
        private const string ProjectId = "vawc-hestiaxisanpedro2025";
        private const string ApiKey = "AIzaSyDtRBmbhj36SeQ6e1Oj6h4hiGB7uqtE8Vo";

        public HomeUI()
        {
            InitializeComponent();
            RA9262_Button_Click(null, EventArgs.Empty);
        }

        // Load notifications when the form loads
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadNotificationsAsync();
        }

        private async Task LoadNotificationsAsync()
        {
            try
            {
                string collectionName = "Complaints";
                string url = $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/{collectionName}?key={ApiKey}";

                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                JObject data = JObject.Parse(json);

                flowLayoutPanel1.Controls.Clear();

                foreach (var document in data["documents"])
                {
                    string caseId = document["fields"]?["caseID"]?["stringValue"]?.ToString() ?? "N/A";
                    string message = document["fields"]?["complaintDetails"]?["stringValue"]?.ToString() ?? "No details provided";

                    // Get the raw date string from Firestore
                    string rawDate = document["fields"]?["complaintDate"]?["stringValue"]?.ToString();

                    // Debug output to verify what we're receiving
                    Debug.WriteLine($"Raw date from Firestore: {rawDate}");

                    DateTime parsedDate;
                    if (!string.IsNullOrEmpty(rawDate))
                    {
                        // First try parsing as Firestore timestamp format
                        if (DateTime.TryParseExact(rawDate, "MMMM dd, yyyy 'at' h:mm:ss tt 'UTC'z",
                                                CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                        {
                            // Successfully parsed
                        }
                        else if (DateTime.TryParse(rawDate, out parsedDate))
                        {
                            // Fallback to regular parsing
                        }
                        else
                        {
                            // Final fallback - show error but use current time
                            Debug.WriteLine($"Failed to parse date: {rawDate}");
                            parsedDate = DateTime.Now;
                        }
                    }
                    else
                    {
                        parsedDate = DateTime.Now;
                    }

                    var notif = new NotifControlOnlineFile();
                    notif.SetData(caseId, message, parsedDate);
                    flowLayoutPanel1.Controls.Add(notif);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading notifications: " + ex.Message);
            }
        }

        private void RA9262_Button_Click(object sender, EventArgs e)
        {
            
            panel1.Controls.Clear(); 
            RA_9262 uc = new RA_9262();
            uc.Dock = DockStyle.Fill; 
            panel1.Controls.Add(uc);
        }

        private void RA8353_Button_Click(object sender, EventArgs e)
        {
            // Your code here
            panel1.Controls.Clear(); 
            RA_8353 uc = new RA_8353();
            uc.Dock = DockStyle.Fill;
            panel1.Controls.Add(uc);
        }

        private void RA7877_Button_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear(); 
            RA_7877 uc = new RA_7877(); 
            uc.Dock = DockStyle.Fill;
            panel1.Controls.Add(uc); 
        }

        private void RA7610_Button_Click(object sender, EventArgs e)
        {
           
        }

        private void RA9208_Button_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            RA_9208 uc = new RA_9208(); 
            uc.Dock = DockStyle.Fill; 
            panel1.Controls.Add(uc);
        }
    }
}
