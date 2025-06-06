using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Firestore;

namespace VAWCSanPedroHestia
{
    public partial class BarangayProtectionOrderForm : UserControl
    {
        public BarangayProtectionOrderForm()
        {
            InitializeComponent();
            textBox1.KeyDown += textBox1_KeyDown;
        }

        private async void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent system beep

                string caseNumberInput = textBox1.Text.Trim();

                if (string.IsNullOrWhiteSpace(caseNumberInput))
                {
                    MessageBox.Show("Please enter a valid case number.");
                    return;
                }

                var db = FirebaseInitialization.Database;

                // Try caselist (PascalCase)
                DocumentSnapshot foundDoc = await FindCaseByNumberAsync(db, "caselist", "CaseDetails.CaseNumber", caseNumberInput);

                // Try onlinecaselist (camelCase)
                if (foundDoc == null)
                    foundDoc = await FindCaseByNumberAsync(db, "onlinecaselist", "caseDetails.caseNumber", caseNumberInput);

                if (foundDoc == null)
                {
                    MessageBox.Show("Case not found.");
                    return;
                }

                var data = foundDoc.ToDictionary();

                // Safely cast to Dictionary<string, object>
                if (data.ContainsKey("Complainant") && data["Complainant"] is Dictionary<string, object> complainant)
                {
                    var complainantFullName = $"{complainant["FirstName"]} {complainant["MiddleName"]} {complainant["LastName"]}";
                    var complainantAge = complainant["Age"].ToString();
                    var complainantStatus = complainant["CivilStatus"].ToString();

                    // Set complainant's full name in label8
                    label8.Text = complainantFullName;

                    // Extract complainant's address
                    if (complainant.ContainsKey("Address") && complainant["Address"] is Dictionary<string, object> address)
                    {
                        var complainantPurok = address.ContainsKey("Purok") ? address["Purok"].ToString() : "";
                        var complainantBarangay = address.ContainsKey("Barangay") ? address["Barangay"].ToString() : "";
                        var complainantMunicipality = address.ContainsKey("Municipality") ? address["Municipality"].ToString() : "";

                        // Set complainant's address in label9
                        label9.Text = $"Purok: {complainantPurok}, Barangay: {complainantBarangay}, Municipality: {complainantMunicipality}";
                    }

                    // Respondent data
                    if (data.ContainsKey("Respondent") && data["Respondent"] is Dictionary<string, object> respondent)
                    {
                        var respondentFullName = $"{respondent["FirstName"]} {respondent["LastName"]}";
                        var respondentAge = respondent["Age"].ToString();
                        var respondentRelationship = respondent["RelationshipToComplainant"].ToString();

                        // Case details
                        if (data.ContainsKey("CaseDetails") && data["CaseDetails"] is Dictionary<string, object> caseDetails)
                        {
                            // Get Philippine Time (UTC+8)
                            TimeZoneInfo philippinesTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Manila");
                            DateTime philippinesTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, philippinesTimeZone);

                            // Format date and time as desired
                            string complaintDate = philippinesTime.ToString("MMMM dd, yyyy hh:mm tt");

                            // Construct the result string
                            string result = $"{complainantFullName}, {complainantAge} years old, {complainantStatus}, applied for a BPO on {complaintDate}, " +
                                            $"under the oath stating that her {respondentRelationship.ToLower()}, {respondentFullName.ToUpper()}, {respondentAge} years old, " +
                                            $"physically/emotionally/economically/psychologically abused and threatened her that caused her to file a case against him.";

                            // Display the result in label22
                            label22.Text = result;
                        }
                    }
                }
            }
        }




        private async Task<DocumentSnapshot> FindCaseByNumberAsync(FirestoreDb db, string collectionName, string caseNumberFieldPath, string caseNumber)
        {
            CollectionReference colRef = db.Collection(collectionName);
            Query query = colRef.WhereEqualTo(caseNumberFieldPath, caseNumber);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();

            return snapshot.Documents.FirstOrDefault();
        }
    }
}
