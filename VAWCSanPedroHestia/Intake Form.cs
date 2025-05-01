using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Firestore;

namespace VAWCSanPedroHestia
{
    public partial class Intake_Form : UserControl
    {
        private readonly FirestoreDb _db;

        public Intake_Form()
        {

            InitializeComponent();

            _db = FirebaseInitialization.Database;
        }

        private void Intake_Form_Load(object sender, EventArgs e)
        {

        }

        private async void CaseTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) // Fetch when user presses Enter
            {
                string caseNumber = CaseTextBox.Text.Trim();

                if (!string.IsNullOrEmpty(caseNumber))
                {
                    await FetchAndPopulateCaseInfo(caseNumber);
                }
            }
        }

        private async Task FetchAndPopulateCaseInfo(string caseNumber)
        {
            try
            {
                string[] collections = { "onlinecaselist", "caselist" };
                bool found = false;
                string collectionFound = "";
                DocumentSnapshot snapshot = null;

                foreach (string collection in collections)
                {
                    DocumentReference caseDoc = FirebaseInitialization.Database.Collection(collection).Document(caseNumber);
                    snapshot = await caseDoc.GetSnapshotAsync();

                    if (snapshot.Exists)
                    {
                        found = true;
                        collectionFound = collection;
                        break;
                    }
                }

                if (!found || snapshot == null)
                {
                    MessageBox.Show($"No record found for case number: {caseNumber}.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Document exists, now parse
                Dictionary<string, object> data = snapshot.ToDictionary();
                if (data == null)
                {
                    MessageBox.Show("Case data is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool isOnline = collectionFound == "onlinecaselist";

                string CaseDetailsKey = isOnline ? "caseDetails" : "CaseDetails";
                string ComplainantKey = isOnline ? "complainant" : "Complainant";
                string RespondentKey = isOnline ? "respondent" : "Respondent";

                // --- Handle CaseDetails
                if (data.TryGetValue(CaseDetailsKey, out object caseDetailsObj) && caseDetailsObj is Dictionary<string, object> caseDetails)
                {
                    ComplaintDate.Text = caseDetails.TryGetValue(isOnline ? "complaintDate" : "ComplaintDate", out var complaintDate) ? complaintDate?.ToString() ?? "" : "";
                    CaseTextBox.Text = caseDetails.TryGetValue(isOnline ? "caseNumber" : "CaseNumber", out var caseNumberValue) ? caseNumberValue?.ToString() ?? "" : "";
                }

                // --- Handle Complainant
                if (data.TryGetValue(ComplainantKey, out object complainantObj) && complainantObj is Dictionary<string, object> complainant)
                {
                    string lastName = complainant.TryGetValue(isOnline ? "lastName" : "LastName", out var cln) ? cln?.ToString() ?? "" : "";
                    string firstName = complainant.TryGetValue(isOnline ? "firstName" : "FirstName", out var cfn) ? cfn?.ToString() ?? "" : "";
                    string middleName = complainant.TryGetValue(isOnline ? "middleName" : "MiddleName", out var cmn) ? cmn?.ToString() ?? "" : "";

                    ComplainantFullName.Text = $"{firstName} {middleName} {lastName}".Trim();
                    ComplainantContactNo.Text = complainant.TryGetValue(isOnline ? "cellNumber" : "CellNumber", out var ccno) ? ccno?.ToString() ?? "" : "";
                    ComplainantSex.Text = complainant.TryGetValue(isOnline ? "sexIdentification" : "SexIdentification", out var csex) ? csex?.ToString() ?? "" : "";
                    ComplainantAge.Text = complainant.TryGetValue(isOnline ? "age" : "Age", out var cage) ? cage?.ToString() ?? "" : "";
                    ComplainantCivilStatus.Text = complainant.TryGetValue(isOnline ? "civilStatus" : "CivilStatus", out var ccivs) ? ccivs?.ToString() ?? "" : "";
                    ComplainantReligion.Text = complainant.TryGetValue(isOnline ? "religion" : "Religion", out var creligion) ? creligion?.ToString() ?? "" : "";
                    ComplainantNationality.Text = complainant.TryGetValue(isOnline ? "nationality" : "Nationality", out var cnationality) ? cnationality?.ToString() ?? "" : "";
                    ComplainantOccupation.Text = complainant.TryGetValue(isOnline ? "occupation" : "Occupation", out var coccupation) ? coccupation?.ToString() ?? "" : "";

                    if (complainant.TryGetValue(isOnline ? "address" : "Address", out var caddressObj) && caddressObj is Dictionary<string, object> caddress)
                    {
                        string purok = caddress.TryGetValue("purok", out var cpurok) ? cpurok?.ToString() ?? "" : "";
                        string barangay = caddress.TryGetValue("barangay", out var cbarangay) ? cbarangay?.ToString() ?? "" : "";
                        string municipality = caddress.TryGetValue("municipality", out var cmunicipality) ? cmunicipality?.ToString() ?? "" : "";
                        string province = caddress.TryGetValue("province", out var cprovince) ? cprovince?.ToString() ?? "" : "";
                        string region = caddress.TryGetValue("region", out var cregion) ? cregion?.ToString() ?? "" : "";

                        ComplainantAddress.Text = $"{purok}, {barangay}, {municipality}, {province}, {region}".Replace(" ,", "").Trim(' ', ',');
                    }
                }

                // --- Handle Respondent
                if (data.TryGetValue(RespondentKey, out object respondentObj) && respondentObj is Dictionary<string, object> respondent)
                {
                    string lastName = respondent.TryGetValue(isOnline ? "lastName" : "LastName", out var rln) ? rln?.ToString() ?? "" : "";
                    string firstName = respondent.TryGetValue(isOnline ? "firstName" : "FirstName", out var rfn) ? rfn?.ToString() ?? "" : "";
                    string middleName = respondent.TryGetValue(isOnline ? "middleName" : "MiddleName", out var rmn) ? rmn?.ToString() ?? "" : "";

                    RespondentFullName.Text = $"{firstName} {middleName} {lastName}".Trim();
                    RespondentContactNo.Text = respondent.TryGetValue(isOnline ? "cellNumber" : "CellNumber", out var rcno) ? rcno?.ToString() ?? "" : "";
                    RespondentSex.Text = respondent.TryGetValue(isOnline ? "sexIdentification" : "SexIdentification", out var rsex) ? rsex?.ToString() ?? "" : "";
                    RespondentAge.Text = respondent.TryGetValue(isOnline ? "age" : "Age", out var rage) ? rage?.ToString() ?? "" : "";
                    RespondentCivilStatus.Text = respondent.TryGetValue(isOnline ? "civilStatus" : "CivilStatus", out var rcivs) ? rcivs?.ToString() ?? "" : "";
                    RespondentReligion.Text = respondent.TryGetValue(isOnline ? "religion" : "Religion", out var rreligion) ? rreligion?.ToString() ?? "" : "";
                    RespondentNationality.Text = respondent.TryGetValue(isOnline ? "nationality" : "Nationality", out var rnationality) ? rnationality?.ToString() ?? "" : "";
                    RespondentOccupation.Text = respondent.TryGetValue(isOnline ? "occupation" : "Occupation", out var roccupation) ? roccupation?.ToString() ?? "" : "";
                    RespondentRelationshipstoVictim.Text = respondent.TryGetValue(isOnline ? "relationshipToComplainant" : "RelationshipToComplainant", out var rrelationship) ? rrelationship?.ToString() ?? "" : "";

                    if (respondent.TryGetValue(isOnline ? "address" : "Address", out var raddressObj) && raddressObj is Dictionary<string, object> raddress)
                    {
                        string purok = raddress.TryGetValue("purok", out var rpurok) ? rpurok?.ToString() ?? "" : "";
                        string barangay = raddress.TryGetValue("barangay", out var rbarangay) ? rbarangay?.ToString() ?? "" : "";
                        string municipality = raddress.TryGetValue("municipality", out var rmunicipality) ? rmunicipality?.ToString() ?? "" : "";
                        string province = raddress.TryGetValue("province", out var rprovince) ? rprovince?.ToString() ?? "" : "";
                        string region = raddress.TryGetValue("region", out var rregion) ? rregion?.ToString() ?? "" : "";

                        RespondentAddress.Text = $"{purok}, {barangay}, {municipality}, {province}, {region}".Replace(" ,", "").Trim(' ', ',');
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching case info: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}