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
        private List<object[]> caseDataList = new List<object[]>(); // Stores all cases before filtering

        public Case_List()
        {
            InitializeComponent();
            LoadCaseList(); // ✅ Load data when form opens
            searchtxtb.TextChanged += Searchtxtb_TextChanged; // ✅ Attach event to search box
        }

        private async void LoadCaseList()
        {
            try
            {
                // Reference Firestore collection
                CollectionReference casesCollection = FirebaseInitialization.Database.Collection("caselist");
                QuerySnapshot snapshot = await casesCollection.GetSnapshotAsync();

                dataGridView1.Rows.Clear();
                caseDataList.Clear(); // Clear old data before reloading

                // Loop through each document in Firestore
                foreach (DocumentSnapshot document in snapshot.Documents)
                {
                    var data = document.ToDictionary();

                    // Extract necessary fields
                    string caseID = document.Id;
                    string complaintDate = GetData(data, "CaseDetails", "ComplaintDate");
                    string complainant = GetFullName(data, "Complainant");
                    string respondent = GetFullName(data, "Respondent");
                    string caseViolation = GetData(data, "CaseDetails", "VAWCCase");
                    string caseSubViolation = GetData(data, "CaseDetails", "SubCase");
                    string violationOccurred = GetData(data, "CaseDetails", "IncidentDate");
                    string respondentsRelations = GetData(data, "Respondent", "RelationshipToComplainant");
                    string narrativeDescription = GetData(data, "CaseDetails", "IncidentDescription");

                    // Store data in a list (for filtering later)
                    object[] rowData = { caseID, complaintDate, complainant, respondent, caseViolation, caseSubViolation, violationOccurred, respondentsRelations, narrativeDescription };
                    caseDataList.Add(rowData);

                    // Add to DataGridView
                    dataGridView1.Rows.Add(rowData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cases: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Searchtxtb_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchtxtb.Text.Trim().ToLower();

            // If search box is empty, reset DataGridView
            if (string.IsNullOrEmpty(searchText))
            {
                dataGridView1.Rows.Clear();
                foreach (var rowData in caseDataList)
                {
                    dataGridView1.Rows.Add(rowData);
                }
                return;
            }

            // Filter data
            var filteredData = caseDataList.Where(row =>
                row[0].ToString().ToLower().Contains(searchText) || // Case ID
                row[2].ToString().ToLower().Contains(searchText) || // Complainant Name
                row[3].ToString().ToLower().Contains(searchText)    // Respondent Name
            ).ToList();

            // Update DataGridView
            dataGridView1.Rows.Clear();
            foreach (var rowData in filteredData)
            {
                dataGridView1.Rows.Add(rowData);
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

        private string GetFullName(Dictionary<string, object> data, string personKey)
        {
            if (data.ContainsKey(personKey) && data[personKey] is Dictionary<string, object> personDict)
            {
                string lastName = personDict.ContainsKey("LastName") ? personDict["LastName"].ToString() : "";
                string firstName = personDict.ContainsKey("FirstName") ? personDict["FirstName"].ToString() : "";
                string middleName = personDict.ContainsKey("MiddleName") ? personDict["MiddleName"].ToString() : "";

                return $"{lastName}, {firstName} {middleName}".Trim();
            }
            return "N/A";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
