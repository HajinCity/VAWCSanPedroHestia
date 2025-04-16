using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Firestore;

namespace VAWCSanPedroHestia
{
    public partial class ManageCaseListIU : Form
    {
        public ManageCaseListIU()
        {
            InitializeComponent();
            casenoTxt.KeyDown += casenoTxt_KeyDown; // Allow pressing Enter to fetch case data
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await FetchCaseData();
        }

        private async void casenoTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await FetchCaseData();
            }
        }


        private async Task FetchCaseData()
        {
            string caseId = casenoTxt.Text.Trim(); // Get Case ID input
            if (string.IsNullOrEmpty(caseId))
            {
                MessageBox.Show("Please enter a Case ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Reference the Firestore document
                DocumentReference caseDoc = FirebaseInitialization.Database.Collection("caselist").Document(caseId);
                DocumentSnapshot snapshot = await caseDoc.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    var data = snapshot.ToDictionary();

                    // Populate fields with Firestore data
                    UcompLstName.Text = GetData(data, "Complainant", "LastName");
                    UcompFstName.Text = GetData(data, "Complainant", "FirstName");
                    UcompMddlName.Text = GetData(data, "Complainant", "MiddleName");
                    UcmbxSex.Text = GetData(data, "Complainant", "Sex");
                    UcompAge.Text = GetData(data, "Complainant", "Age");
                    UCompBdate.Text = GetData(data,"Complainant", "Birthdate");
                    UcompReligion.Text = GetData(data, "Complainant", "Religion");
                    UcompCellNo.Text = GetData(data, "Complainant", "CellNumber");
                    UcompCivilS.Text = GetData(data, "Complainant", "CivilStatus");
                    UcompPurok.Text = GetData(data, "Complainant", "Purok");
                    UcompBarangay.Text = GetData(data, "Complainant", "Barangay");
                    UcompMunicipal.Text = GetData(data, "Complainant", "Municipality");
                    UcompProvince.Text = GetData(data, "Complainant", "Province");
                    UcompRegion.Text = GetData(data, "Complainant", "Region");
                    UCompNationality.Text = GetData(data, "Complainant", "Nationality");
                    UCompOccupation.Text = GetData(data, "Complainant", "Occupation");

                    UpRspLstName.Text = GetData(data, "Respondent", "LastName");
                    UpRspFrstName.Text = GetData(data, "Respondent", "FirstName");
                    UpRspMiddleName.Text = GetData(data, "Respondent", "MiddleName");
                    UpRspAllias.Text = GetData(data, "Respondent", "Alias");
                    UpRspSex.Text = GetData(data, "Respondent", "Sex");
                    UrAge.Text = GetData(data, "Respondent", "Age");
                    UResBdate.Text = GetData(data, "Respondent", "Birthdate");
                    UpRspReligion.Text = GetData(data, "Respondent", "Religion");
                    UpRspCellNo.Text = GetData(data, "Respondent", "CellNumber");
                    UpRspCivilS.Text = GetData(data, "Respondent", "CivilStatus");
                    UpRspPurok.Text = GetData(data, "Respondent", "Purok");
                    UpRspBarangay.Text = GetData(data, "Respondent", "Barangay");
                    UpRspMunicipal.Text = GetData(data, "Respondent", "Municipality");
                    UpRspProvince.Text = GetData(data, "Respondent", "Province");
                    UpRspRegion.Text = GetData(data, "Respondent", "Region");
                    UResNationality.Text = GetData(data, "Respondent", "Nationality");
                    UResOccupation.Text = GetData(data, "Respondent", "Occupation");
                    UpRspRelationshiptoC.Text = GetData(data, "Respondent", "RelationshipToComplainant");

                    UpRAVio.Text = GetData(data, "CaseDetails", "VAWCCase");
                    UpRASubVio.Text = GetData(data, "CaseDetails", "SubCase");
                    UpCaseStatus.Text = GetData(data, "CaseDetails", "CaseStatus");
                    UpRefTo.Text = GetData(data, "CaseDetails", "ReferredTo");
                    UpIncidentDate.Text = GetData(data, "CaseDetails", "IncidentDate");
                    Updesciption.Text = GetData(data, "CaseDetails", "IncidentDescription");
                    UpPlaceofIncident.Text = GetData(data, "CaseDetails", "IncidentPlace");
                    UpPurok.Text = GetData(data, "CaseDetails", "IncidentPurok");
                    UpBarangay.Text = GetData(data, "CaseDetails", "IncidentBarangay");
                    UpMunicipal.Text = GetData(data, "CaseDetails", "IncidentMunicipality");
                    UpProvince.Text = GetData(data, "CaseDetails", "IncidentProvince");
                    UpRegion.Text = GetData(data, "CaseDetails", "IncidentRegion");
                }
                else
                {
                    MessageBox.Show("Case not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching case: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetData(Dictionary<string, object> data, string parentKey, string childKey)
        {
            if (data.ContainsKey(parentKey) && data[parentKey] is Dictionary<string, object> parentDict && parentDict.ContainsKey(childKey))
            {
                return parentDict[childKey]?.ToString() ?? "N/A";
            }
            return "N/A";
        }

     
        private void ClearForm(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is TextBox txt)
                    txt.Clear();

                if (ctrl is ComboBox cmb)
                {
                    cmb.SelectedIndex = -1; // ✅ Clears dropdown selection
                    cmb.Text = "";          // ✅ Clears any manually entered text
                }

                if (ctrl is DateTimePicker dtp)
                    dtp.Value = DateTime.Now; // ✅ Resets to current date

                // **If the control contains more controls inside (like Panels, GroupBoxes)**
                if (ctrl.HasChildren)
                    ClearForm(ctrl); // ✅ Recursive call to clear controls inside Panels, GroupBoxes
            }
           
        }

        private async void save_casebtn_Click(object sender, EventArgs e)
        {
            string caseId = casenoTxt.Text.Trim();
            if (string.IsNullOrEmpty(caseId))
            {
                MessageBox.Show("No Case ID found. Please search for a case first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DocumentReference caseDoc = FirebaseInitialization.Database.Collection("caselist").Document(caseId);

                Dictionary<string, object> updatedData = new Dictionary<string, object>
        {
            {
                "Complainant", new Dictionary<string, object>
                {
                    { "LastName", UcompLstName.Text },
                    { "FirstName", UcompFstName.Text },
                    { "MiddleName", UcompMddlName.Text },
                    { "Sex", UcmbxSex.Text },
                    { "Age", UcompAge.Text },
                    { "Religion", UcompReligion.Text },
                    { "CellNumber", UcompCellNo.Text },
                    { "CivilStatus", UcompCivilS.Text },
                    {
                        "Address", new Dictionary<string, object>
                        {
                            { "Purok", UcompPurok.Text },
                            { "Barangay", UcompBarangay.Text },
                            { "Municipality", UcompMunicipal.Text },
                            { "Province", UcompProvince.Text },
                            { "Region", UcompRegion.Text }
                        }
                    }
                }
            },
            {
                "Respondent", new Dictionary<string, object>
                {
                    { "LastName", UpRspLstName.Text },
                    { "FirstName", UpRspFrstName.Text },
                    { "MiddleName", UpRspMiddleName.Text },
                    { "Alias", UpRspAllias.Text },
                    { "Sex", UpRspSex.Text },
                    { "Age", UrAge.Text },
                    { "Religion", UpRspReligion.Text },
                    { "CellNumber", UpRspCellNo.Text },
                    { "CivilStatus", UpRspCivilS.Text },
                    {
                        "Address", new Dictionary<string, object>
                        {
                            { "Purok", UpRspPurok.Text },
                            { "Barangay", UpRspBarangay.Text },
                            { "Municipality", UpRspMunicipal.Text },
                            { "Province", UpRspProvince.Text },
                            { "Region", UpRspRegion.Text }
                        }
                    },
                    { "RelationshipToComplainant", UpRspRelationshiptoC.Text }
                }
            },
            {
                "CaseDetails", new Dictionary<string, object>
                {
                    { "VAWCCase", UpRAVio.Text },
                    { "SubCase", UpRASubVio.Text },
                    { "CaseStatus", UpCaseStatus.Text },
                    { "ReferredTo", UpRefTo.Text },
                    { "IncidentDate", UpIncidentDate.Text },
                    { "IncidentDescription", Updesciption.Text },
                    { "IncidentPlace", UpPlaceofIncident.Text },
                    { "IncidentPurok", UpPurok.Text },
                    { "IncidentBarangay", UpBarangay.Text },
                    { "IncidentMunicipality", UpMunicipal.Text },
                    { "IncidentProvince", UpProvince.Text },
                    { "IncidentRegion", UpRegion.Text }
                }
            }
        };

                await caseDoc.SetAsync(updatedData, SetOptions.MergeAll);

                MessageBox.Show("Case updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm(this); // You can comment this if you want to review before clearing
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating case: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



    }
}
