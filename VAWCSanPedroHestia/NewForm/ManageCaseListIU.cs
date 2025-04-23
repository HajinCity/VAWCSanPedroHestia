using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Firestore;
using Google.Protobuf.Collections;

namespace VAWCSanPedroHestia
{
    public partial class ManageCaseListIU : Form
    {
        private string fetchedCollectionName = "caselist"; // Default source

        public ManageCaseListIU()
        {
            InitializeComponent();
            casenoTxt.KeyDown += casenoTxt_KeyDown;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            await FetchCaseData();
        }

        private async void casenoTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                await FetchCaseData();
        }

        private async Task FetchCaseData()
        {
            string caseId = casenoTxt.Text.Trim();
            if (string.IsNullOrEmpty(caseId))
            {
                MessageBox.Show("Please enter a Case ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string[] collections = { "caselist", "onlinecaselist" };
            bool found = false;

            foreach (string collection in collections)
            {
                DocumentReference caseDoc = FirebaseInitialization.Database.Collection(collection).Document(caseId);
                DocumentSnapshot snapshot = await caseDoc.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    fetchedCollectionName = collection;
                    var data = snapshot.ToDictionary();

                    // Complainant
                    UcompLstName.Text = GetData(data, "Complainant", "LastName");
                    UcompFstName.Text = GetData(data, "Complainant", "FirstName");
                    UcompMddlName.Text = GetData(data, "Complainant", "MiddleName");
                    UcmbxSex.Text = GetData(data, "Complainant", "Sex");
                    UcompAge.Text = GetData(data, "Complainant", "Age");
                    SetDateTimePickerValue(UCompBdate, GetData(data, "Complainant", "Birthdate"));
                    UcompReligion.Text = GetData(data, "Complainant", "Religion");
                    UcompCellNo.Text = GetData(data, "Complainant", "CellNumber");
                    UcompCivilS.Text = GetData(data, "Complainant", "CivilStatus");
                    UcompPurok.Text = GetNestedData(data, "Complainant", "Address", "Purok");
                    UcompBarangay.Text = GetNestedData(data, "Complainant", "Address", "Barangay");
                    UcompMunicipal.Text = GetNestedData(data, "Complainant", "Address", "Municipality");
                    UcompProvince.Text = GetNestedData(data, "Complainant", "Address", "Province");
                    UcompRegion.Text = GetNestedData(data, "Complainant", "Address", "Region");
                    UCompNationality.Text = GetData(data, "Complainant", "Nationality");
                    UCompOccupation.Text = GetData(data, "Complainant", "Occupation");

                    // Respondent
                    UpRspLstName.Text = GetData(data, "Respondent", "LastName");
                    UpRspFrstName.Text = GetData(data, "Respondent", "FirstName");
                    UpRspMiddleName.Text = GetData(data, "Respondent", "MiddleName");
                    UpRspAllias.Text = GetData(data, "Respondent", "Alias");
                    UpRspSex.Text = GetData(data, "Respondent", "Sex");
                    UrAge.Text = GetData(data, "Respondent", "Age");
                    SetDateTimePickerValue(UResBdate, GetData(data, "Respondent", "Birthdate"));
                    UpRspReligion.Text = GetData(data, "Respondent", "Religion");
                    UpRspCellNo.Text = GetData(data, "Respondent", "CellNumber");
                    UpRspCivilS.Text = GetData(data, "Respondent", "CivilStatus");
                    UpRspPurok.Text = GetNestedData(data, "Respondent", "Address", "Purok");
                    UpRspBarangay.Text = GetNestedData(data, "Respondent", "Address", "Barangay");
                    UpRspMunicipal.Text = GetNestedData(data, "Respondent", "Address", "Municipality");
                    UpRspProvince.Text = GetNestedData(data, "Respondent", "Address", "Province");
                    UpRspRegion.Text = GetNestedData(data, "Respondent", "Address", "Region");
                    UResNationality.Text = GetData(data, "Respondent", "Nationality");
                    UResOccupation.Text = GetData(data, "Respondent", "Occupation");
                    UpRspRelationshiptoC.Text = GetData(data, "Respondent", "RelationshipToComplainant");

                    // Case Details
                    UpRAVio.Text = GetData(data, "CaseDetails", "VawcCase");
                    UpRASubVio.Text = GetData(data, "CaseDetails", "SubCase");
                    UpCaseStatus.Text = GetData(data, "CaseDetails", "CaseStatus");
                    UpRefTo.Text = GetData(data, "CaseDetails", "ReferredTo");
                    SetDateTimePickerValue(UpIncidentDate, GetData(data, "CaseDetails", "IncidentDate"));

                    // Special handling for IncidentDescription
                    string incidentDesc = GetData(data, "CaseDetails", "IncidentDescription");
                    Debug.WriteLine($"Incident Description from Firestore: {incidentDesc}");
                    Updesciption.Text = incidentDesc;

                    UpPlaceofIncident.Text = GetNestedData(data, "CaseDetails", "PlaceOfIncident", "Place");
                    UpPurok.Text = GetNestedData(data, "CaseDetails", "PlaceOfIncident", "Purok");
                    UpBarangay.Text = GetNestedData(data, "CaseDetails", "PlaceOfIncident", "Barangay");
                    UpMunicipal.Text = GetNestedData(data, "CaseDetails", "PlaceOfIncident", "Municipality");
                    UpProvince.Text = GetNestedData(data, "CaseDetails", "PlaceOfIncident", "Province");
                    UpRegion.Text = GetNestedData(data, "CaseDetails", "PlaceOfIncident", "Region");

                    found = true;
                    break;
                }
            }

            if (!found)
                MessageBox.Show("Case not found in both 'caselist' and 'onlinecaselist'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private string GetNestedData(Dictionary<string, object> data, string parentKey, string nestedMapKey, string fieldKey)
        {
            string[] parentKeys = { parentKey, char.ToLowerInvariant(parentKey[0]) + parentKey.Substring(1) };
            string[] nestedMapKeys = { nestedMapKey, char.ToLowerInvariant(nestedMapKey[0]) + nestedMapKey.Substring(1) };
            string[] fieldKeys = { fieldKey, char.ToLowerInvariant(fieldKey[0]) + fieldKey.Substring(1) };

            foreach (var pKey in parentKeys)
            {
                if (data.TryGetValue(pKey, out object parentObj) && parentObj is Dictionary<string, object> parentDict)
                {
                    foreach (var nKey in nestedMapKeys)
                    {
                        if (parentDict.TryGetValue(nKey, out object nestedObj) && nestedObj is Dictionary<string, object> nestedDict)
                        {
                            foreach (var fKey in fieldKeys)
                            {
                                if (nestedDict.TryGetValue(fKey, out object value))
                                    return value?.ToString() ?? "N/A";
                            }
                        }
                    }
                }
            }

            return "N/A";
        }


        private string GetData(Dictionary<string, object> data, string parentKey, string childKey)
        {
            // Try both PascalCase and camelCase for parent key
            string[] possibleParentKeys = {
        parentKey,
        char.ToLowerInvariant(parentKey[0]) + parentKey.Substring(1)
    };

            foreach (string effectiveParent in possibleParentKeys)
            {
                if (data.TryGetValue(effectiveParent, out object parentObj))
                {
                    if (parentObj is Dictionary<string, object> parentDict)
                    {
                        // Try both PascalCase and camelCase for child key
                        string[] possibleChildKeys = {
                    childKey,
                    char.ToLowerInvariant(childKey[0]) + childKey.Substring(1)
                };

                        foreach (var effectiveChild in possibleChildKeys)
                        {
                            if (parentDict.TryGetValue(effectiveChild, out object value))
                            {
                                if (value == null) return "N/A";

                                if (value is Timestamp ts)
                                    return ts.ToDateTime().ToString("yyyy-MM-dd");

                                return value.ToString();
                            }
                        }
                    }
                }
            }

            return "N/A";
        }

        private void SetDateTimePickerValue(DateTimePicker picker, string value)
        {
            // Handle null, empty, or "N/A" values first
            if (string.IsNullOrWhiteSpace(value) || value.Equals("N/A", StringComparison.OrdinalIgnoreCase))
            {
                picker.Value = DateTime.Now.Date;
                picker.CustomFormat = " ";
                picker.Format = DateTimePickerFormat.Custom;
                return;
            }

            // Try parsing as Firestore Timestamp first
            if (value.Contains("Timestamp") && value.Contains("seconds=") && value.Contains("nanoseconds="))
            {
                try
                {
                    var seconds = long.Parse(value.Split(new[] { "seconds=" }, StringSplitOptions.None)[1]
                        .Split(',')[0].Trim());
                    var date = DateTimeOffset.FromUnixTimeSeconds(seconds).DateTime;
                    picker.Value = date;
                    picker.Format = DateTimePickerFormat.Short;
                    return;
                }
                catch
                {
                    // Continue with normal date parsing if this fails
                }
            }

            // Try standard DateTime parsing
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                picker.Value = parsedDate;
                picker.Format = DateTimePickerFormat.Short;
                return;
            }

            // Try common date formats
            string[] formats = { "MM/dd/yyyy", "yyyy-MM-dd", "dd/MM/yyyy", "M/d/yyyy", "yyyy-MM-ddTHH:mm:ssZ" };
            if (DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                picker.Value = parsedDate;
                picker.Format = DateTimePickerFormat.Short;
                return;
            }

            // Fallback to current date if all parsing fails
            picker.Value = DateTime.Now.Date;
            picker.CustomFormat = " ";
            picker.Format = DateTimePickerFormat.Custom;
            Debug.WriteLine($"Failed to parse date: {value}");
        }

        private void ClearForm(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is TextBox txt)
                    txt.Clear();
                else if (ctrl is ComboBox cmb)
                {
                    cmb.SelectedIndex = -1;
                    cmb.Text = "";
                }
                else if (ctrl is DateTimePicker dtp)
                {
                    dtp.Value = DateTime.Now;
                    dtp.Format = DateTimePickerFormat.Short;
                }

                if (ctrl.HasChildren)
                    ClearForm(ctrl);
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
                var db = FirebaseInitialization.Database;
                DocumentReference caseDoc = db.Collection(fetchedCollectionName).Document(caseId);
                bool isOnline = fetchedCollectionName == "onlinecaselist";

                Dictionary<string, object> updatedData = new Dictionary<string, object>
        {
            {
                isOnline ? "complainant" : "Complainant", new Dictionary<string, object>
                {
                    { isOnline ? "lastName" : "LastName", UcompLstName.Text },
                    { isOnline ? "firstName" : "FirstName", UcompFstName.Text },
                    { isOnline ? "middleName" : "MiddleName", UcompMddlName.Text },
                    { isOnline ? "sex" : "Sex", UcmbxSex.Text },
                    { isOnline ? "age" : "Age", UcompAge.Text },
                    { isOnline ? "birthdate" : "Birthdate", UCompBdate.Format == DateTimePickerFormat.Custom ? "N/A" : UCompBdate.Value.ToString("yyyy-MM-dd") },
                    { isOnline ? "religion" : "Religion", UcompReligion.Text },
                    { isOnline ? "cellNumber" : "CellNumber", UcompCellNo.Text },
                    { isOnline ? "civilStatus" : "CivilStatus", UcompCivilS.Text },
                    { isOnline ? "nationality" : "Nationality", UCompNationality.Text },
                    { isOnline ? "occupation" : "Occupation", UCompOccupation.Text },
                    {
                        "address", new Dictionary<string, object>
                        {
                            { "purok", UcompPurok.Text },
                            { "barangay", UcompBarangay.Text },
                            { "municipality", UcompMunicipal.Text },
                            { "province", UcompProvince.Text },
                            { "region", UcompRegion.Text }
                        }
                    }
                }
            },
            {
                isOnline ? "respondent" : "Respondent", new Dictionary<string, object>
                {
                    { isOnline ? "lastName" : "LastName", UpRspLstName.Text },
                    { isOnline ? "firstName" : "FirstName", UpRspFrstName.Text },
                    { isOnline ? "middleName" : "MiddleName", UpRspMiddleName.Text },
                    { isOnline ? "alias" : "Alias", UpRspAllias.Text },
                    { isOnline ? "sex" : "Sex", UpRspSex.Text },
                    { isOnline ? "age" : "Age", UrAge.Text },
                    { isOnline ? "birthdate" : "Birthdate", UResBdate.Format == DateTimePickerFormat.Custom ? "N/A" : UResBdate.Value.ToString("yyyy-MM-dd") },
                    { isOnline ? "religion" : "Religion", UpRspReligion.Text },
                    { isOnline ? "cellNumber" : "CellNumber", UpRspCellNo.Text },
                    { isOnline ? "civilStatus" : "CivilStatus", UpRspCivilS.Text },
                    { isOnline ? "nationality" : "Nationality", UResNationality.Text },
                    { isOnline ? "occupation" : "Occupation", UResOccupation.Text },
                    { isOnline ? "relationshipToComplainant" : "RelationshipToComplainant", UpRspRelationshiptoC.Text },
                    {
                        "address", new Dictionary<string, object>
                        {
                            { "purok", UpRspPurok.Text },
                            { "barangay", UpRspBarangay.Text },
                            { "municipality", UpRspMunicipal.Text },
                            { "province", UpRspProvince.Text },
                            { "region", UpRspRegion.Text }
                        }
                    }
                }
            },
            {
                isOnline ? "caseDetails" : "CaseDetails", new Dictionary<string, object>
                {
                    { isOnline ? "vawcCase" : "VawcCase", UpRAVio.Text },
                    { isOnline ? "subCase" : "SubCase", UpRASubVio.Text },
                    { isOnline ? "caseStatus" : "CaseStatus", UpCaseStatus.Text },
                    { isOnline ? "referredTo" : "ReferredTo", UpRefTo.Text },
                    { isOnline ? "incidentDate" : "IncidentDate", UpIncidentDate.Format == DateTimePickerFormat.Custom ? "N/A" : UpIncidentDate.Value.ToString("yyyy-MM-dd") },
                    { isOnline ? "incidentDescription" : "IncidentDescription", string.IsNullOrWhiteSpace(Updesciption.Text) ? "N/A" : Updesciption.Text },
                    {
                        "placeOfIncident", new Dictionary<string, object>
                        {
                            { "place", UpPlaceofIncident.Text },
                            { "purok", UpPurok.Text },
                            { "barangay", UpBarangay.Text },
                            { "municipality", UpMunicipal.Text },
                            { "province", UpProvince.Text },
                            { "region", UpRegion.Text }
                        }
                    }
                }
            }
        };

                await caseDoc.SetAsync(updatedData, SetOptions.MergeAll);
                MessageBox.Show("Case updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm(this);
                fetchedCollectionName = "caselist"; // Reset default
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating case: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void UpRAVio_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear existing items in the SUB-case combo box (not the main one)
            UpRASubVio.Items.Clear();

            // Get the selected value from UpRAVio (the MAIN combo box)
            string selectedCategory = UpRAVio.SelectedItem?.ToString();

            // Check which category is selected
            if (selectedCategory == "R.A. 9262: Anti Violence Against Women and their Children Act")
            {
                // Add specific sub-cases for R.A. 9262
                UpRASubVio.Items.Add("Sexual Abuse");
                UpRASubVio.Items.Add("Psychological");
                UpRASubVio.Items.Add("Physical");
                UpRASubVio.Items.Add("Economic");
            }
            else
            {
                // If it's any other category, only show "None"
                UpRASubVio.Items.Add("None");
            }

            // Set default selection to first item if items exist
            if (UpRASubVio.Items.Count > 0)
            {
                UpRASubVio.SelectedIndex = 0;
            }
        }
    }
}