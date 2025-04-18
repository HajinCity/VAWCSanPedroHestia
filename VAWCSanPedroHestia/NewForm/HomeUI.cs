using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Drawing;
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
        private readonly Timer _refreshTimer = new Timer();

        public HomeUI()
        {
            InitializeComponent();
            RA9262_Button_Click(null, EventArgs.Empty);

            _refreshTimer.Interval = 10000; // 10 seconds
            _refreshTimer.Tick += async (s, e) => await LoadNotificationsAsync();
            _refreshTimer.Start();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await LoadNotificationsAsync();
        }

        private async Task LoadNotificationsAsync()
        {
            try
            {
                string url = $"https://firestore.googleapis.com/v1/projects/{ProjectId}/databases/(default)/documents/Complaints?key={ApiKey}";
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                JObject data = JObject.Parse(json);

                flowLayoutPanel1.Controls.Clear();

                var documents = data["documents"] as JArray;
                if (documents == null || documents.Count == 0)
                {
                    Label noDataLabel = new Label
                    {
                        Text = "No complaint notifications available.",
                        AutoSize = true,
                        Font = new Font("Segoe UI", 12, FontStyle.Italic),
                        ForeColor = Color.Gray,
                        Margin = new Padding(10)
                    };
                    flowLayoutPanel1.Controls.Add(noDataLabel);
                    return;
                }

                foreach (var document in documents)
                {
                    try
                    {
                        var fields = document["fields"] as JObject;
                        if (fields == null) continue;

                        var complainant = fields["complainant"]?["mapValue"]?["fields"] as JObject;
                        var respondent = fields["respondent"]?["mapValue"]?["fields"] as JObject;
                        var details = fields["caseDetails"]?["mapValue"]?["fields"] as JObject;
                        if (complainant == null || respondent == null || details == null) continue;

                        // Complainant Info
                        string firstName = complainant["firstName"]?["stringValue"]?.ToString() ?? "";
                        string middleName = complainant["middleName"]?["stringValue"]?.ToString() ?? "";
                        string lastName = complainant["lastName"]?["stringValue"]?.ToString() ?? "";
                        string age = complainant["age"]?["stringValue"]?.ToString() ?? "";
                        string sex = complainant["sexIdentification"]?["stringValue"]?.ToString() ?? "";
                        string civilStatus = complainant["civilStatus"]?["stringValue"]?.ToString() ?? "";
                        string religion = complainant["religion"]?["stringValue"]?.ToString() ?? "";
                        string nationality = complainant["nationality"]?["stringValue"]?.ToString() ?? "";
                        string occupation = complainant["occupation"]?["stringValue"]?.ToString() ?? "";
                        string contact = complainant["cellNumber"]?["stringValue"]?.ToString() ?? "";

                        var address = complainant["address"]?["mapValue"]?["fields"] as JObject;
                        string purok = address?["purok"]?["stringValue"]?.ToString() ?? "";
                        string city = address?["municipality"]?["stringValue"]?.ToString() ?? "";

                        // Respondent Info
                        string respFirstName = respondent["firstName"]?["stringValue"]?.ToString() ?? "";
                        string respMiddleName = respondent["middleName"]?["stringValue"]?.ToString() ?? "";
                        string respLastName = respondent["lastName"]?["stringValue"]?.ToString() ?? "";
                        string respAlias = respondent["alias"]?["stringValue"]?.ToString() ?? "";
                        string respSex = respondent["sexIdentification"]?["stringValue"]?.ToString() ?? "";
                        string respContact = respondent["cellNumber"]?["stringValue"]?.ToString() ?? "";
                        string respAge = respondent["age"]?["stringValue"]?.ToString() ?? "";
                        string respCivilStatus = respondent["civilStatus"]?["stringValue"]?.ToString() ?? "";
                        string respReligion = respondent["religion"]?["stringValue"]?.ToString() ?? "";
                        string respNationality = respondent["nationality"]?["stringValue"]?.ToString() ?? "";
                        string respOccupation = respondent["occupation"]?["stringValue"]?.ToString() ?? "";
                        string relationship = respondent["relationshipToComplainant"]?["stringValue"]?.ToString() ?? "";

                        // Case Details
                        string caseId = details["caseNumber"]?["stringValue"]?.ToString() ?? "";
                        string incidentDescription = details["incidentDescription"]?["stringValue"]?.ToString() ?? "";
                        string status = details["caseStatus"]?["stringValue"]?.ToString() ?? "Pending";

                        // Updated date handling - checks both timestampValue and stringValue
                        DateTime complaintDate;
                        var timestampValue = details["complaintDate"]?["timestampValue"]?.ToString();
                        var stringValue = details["complaintDate"]?["stringValue"]?.ToString();

                        if (!string.IsNullOrEmpty(timestampValue))
                        {
                            // Handle Firestore timestamp format
                            complaintDate = DateTime.TryParse(timestampValue, out var parsed) ? parsed.ToLocalTime() : DateTime.MinValue;
                        }
                        else if (!string.IsNullOrEmpty(stringValue))
                        {
                            // Handle string format (like in your image: "2025-04-16")
                            complaintDate = DateTime.TryParse(stringValue, out var parsed) ? parsed : DateTime.MinValue;
                        }
                        else
                        {
                            complaintDate = DateTime.MinValue;
                        }

                        string incidentDateRaw = details["incidentDate"]?["stringValue"]?.ToString() ?? "";
                        DateTime incidentDate = DateTime.TryParse(incidentDateRaw, out var incidentParsed)
                            ? incidentParsed
                            : DateTime.MinValue;

                        var placeOfIncident = details["placeOfIncident"]?["mapValue"]?["fields"] as JObject;
                        string place = placeOfIncident?["place"]?["stringValue"]?.ToString() ?? "";
                        string incidentPurok = placeOfIncident?["purok"]?["stringValue"]?.ToString() ?? "";
                        string incidentBarangay = placeOfIncident?["barangay"]?["stringValue"]?.ToString() ?? "";
                        string incidentMunicipality = placeOfIncident?["municipality"]?["stringValue"]?.ToString() ?? "";
                        string incidentProvince = placeOfIncident?["province"]?["stringValue"]?.ToString() ?? "";
                        string incidentRegion = placeOfIncident?["region"]?["stringValue"]?.ToString() ?? "";

                        var notif = new NotifControlOnlineFile();
                        notif.SetData(
                            caseId,
                            firstName, middleName, lastName,
                            age, sex, civilStatus, religion, nationality, occupation,
                            purok, city, contact,
                            respFirstName, respMiddleName, respLastName,
                            respAlias, respSex, respContact, respAge,
                            respCivilStatus, respReligion, respNationality, respOccupation,
                            relationship,
                            complaintDate,
                            incidentDate,
                            incidentDescription,
                            status,
                            place, incidentPurok, incidentBarangay, incidentMunicipality, incidentProvince, incidentRegion
                        );

                        flowLayoutPanel1.Controls.Add(notif);
                    }
                    catch (Exception innerEx)
                    {
                        Debug.WriteLine("❌ Error processing document: " + innerEx.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("⚠️ Error loading notifications: " + ex.Message);
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
            // Implementation for RA7610 button click
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