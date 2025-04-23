using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Firestore;

namespace VAWCSanPedroHestia
{
    public partial class Case_List : Form
    {
        private List<object[]> caseDataList = new List<object[]>();
        private List<object[]> onlineCaseList = new List<object[]>();

        public Case_List()
        {
            InitializeComponent();
            LoadCaseList();
            LoadOnlineCaseList();
            searchtxtb.TextChanged += Searchtxtb_TextChanged;
        }

        private async void LoadCaseList()
        {
            try
            {
                CollectionReference casesCollection = FirebaseInitialization.Database.Collection("caselist");
                QuerySnapshot snapshot = await casesCollection.GetSnapshotAsync();

                dataGridView1.Rows.Clear();
                caseDataList.Clear();

                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    var data = document.ToDictionary();

                    string caseID = document.Id;
                    string complaintDate = GetData(data, "CaseDetails", "ComplaintDate");
                    string complainant = GetFullName(data, "Complainant");
                    string respondent = GetFullName(data, "Respondent");
                    string caseViolation = GetData(data, "CaseDetails", "VAWCCase");
                    string caseSubViolation = GetData(data, "CaseDetails", "SubCase");
                    string violationOccurred = GetData(data, "CaseDetails", "IncidentDate");
                    string respondentsRelations = GetData(data, "Respondent", "RelationshipToComplainant");
                    string narrativeDescription = GetData(data, "CaseDetails", "IncidentDescription");

                    object[] rowData = {
                        caseID,complaintDate,complainant,respondent,caseViolation,caseSubViolation,violationOccurred,respondentsRelations,narrativeDescription
                    };

                    caseDataList.Add(rowData);
                    dataGridView1.Rows.Add(rowData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cases: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void LoadOnlineCaseList()
        {
            try
            {
                CollectionReference onlineCaseCollection = FirebaseInitialization.Database.Collection("onlinecaselist");
                QuerySnapshot snapshot = await onlineCaseCollection.GetSnapshotAsync();

                dataGridView2.Rows.Clear();
                onlineCaseList.Clear();

                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    var data = document.ToDictionary();

                    string caseID = document.Id;
                    string complaintDate = GetData(data, "caseDetails", "complaintDate");
                    string complainant = GetFullName(data, "complainant");
                    string respondent = GetFullName(data, "respondent");
                    string caseViolation = GetData(data, "caseDetails", "vawcCase");
                    string caseSubViolation = GetData(data, "caseDetails", "subCase");
                    string violationOccurred = GetData(data, "caseDetails", "incidentDate");
                    string respondentsRelations = GetData(data, "respondent", "relationshipToComplainant");
                    string narrativeDescription = GetData(data, "caseDetails", "incidentDescription");

                    object[] rowData = {
                        caseID,complaintDate,complainant,respondent,caseViolation,caseSubViolation,violationOccurred,respondentsRelations,narrativeDescription
                    };

                    onlineCaseList.Add(rowData);
                    dataGridView2.Rows.Add(rowData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading online cases: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetData(Dictionary<string, object> data, string parentKey, string childKey)
        {
            if (string.IsNullOrEmpty(parentKey))
            {
                return data.ContainsKey(childKey) ? data[childKey]?.ToString() ?? "N/A" : "N/A";
            }

            if (data.ContainsKey(parentKey) && data[parentKey] is Dictionary<string, object> parentDict && parentDict.ContainsKey(childKey))
            {
                return parentDict[childKey]?.ToString() ?? "N/A";
            }

            return "N/A";
        }

        private string GetFullName(Dictionary<string, object> data, string personKey)
        {
            if (data.ContainsKey(personKey) && data[personKey] is Dictionary<string, object> personDict)
            {
                // Try both capitalized and lowercase keys
                string lastName = personDict.ContainsKey("LastName") ? personDict["LastName"].ToString() :
                                  personDict.ContainsKey("lastName") ? personDict["lastName"].ToString() : "";

                string firstName = personDict.ContainsKey("FirstName") ? personDict["FirstName"].ToString() :
                                   personDict.ContainsKey("firstName") ? personDict["firstName"].ToString() : "";

                string middleName = personDict.ContainsKey("MiddleName") ? personDict["MiddleName"].ToString() :
                                    personDict.ContainsKey("middleName") ? personDict["middleName"].ToString() : "";

                return $"{lastName}, {firstName} {middleName}".Trim();
            }

            return "N/A";
        }

        private void Searchtxtb_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchtxtb.Text.Trim().ToLower();

            // === Filter DataGridView1 (caselist) ===
            dataGridView1.Rows.Clear();

            if (string.IsNullOrEmpty(searchText))
            {
                foreach (var rowData in caseDataList)
                {
                    dataGridView1.Rows.Add(rowData);
                }
            }
            else
            {
                var filteredCaseData = caseDataList.Where(row =>
                    row[0].ToString().ToLower().Contains(searchText) || // Case ID
                    row[2].ToString().ToLower().Contains(searchText) || // Complainant
                    row[3].ToString().ToLower().Contains(searchText)    // Respondent
                ).ToList();

                foreach (var rowData in filteredCaseData)
                {
                    dataGridView1.Rows.Add(rowData);
                }
            }

            // === Filter DataGridView2 (onlinecaselist) ===
            dataGridView2.Rows.Clear();

            if (string.IsNullOrEmpty(searchText))
            {
                foreach (var rowData in onlineCaseList)
                {
                    dataGridView2.Rows.Add(rowData);
                }
            }
            else
            {
                var filteredOnlineCaseData = onlineCaseList.Where(row =>
                    row[0].ToString().ToLower().Contains(searchText) || // Case ID
                    row[2].ToString().ToLower().Contains(searchText) || // Complainant
                    row[3].ToString().ToLower().Contains(searchText)    // Respondent
                ).ToList();

                foreach (var rowData in filteredOnlineCaseData)
                {
                    dataGridView2.Rows.Add(rowData);
                }
            }
        }

    }
}