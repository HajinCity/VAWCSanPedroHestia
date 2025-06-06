using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Firestore;

namespace VAWCSanPedroHestia.NewForm
{
    public partial class DataDashboardDesign : UserControl
    {
       

        public DataDashboardDesign()
        {
            InitializeComponent();
            SetupTableLayout();

            // Call GenerateReport when the form is initialized
            _ = GenerateReport();

            // Add event handlers to trigger report generation when the date pickers change
            dateTimePicker1.ValueChanged += DateTimePicker_ValueChanged;
            dateTimePicker2.ValueChanged += DateTimePicker_ValueChanged;
        }

        // Event handler for DateTimePickers to trigger report generation
        private async void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            await GenerateReport(); // Regenerate report when dates change
        }

        private async Task GenerateReport()
        {
            DateTime startDate = dateTimePicker1.Value.Date;
            DateTime endDate = dateTimePicker2.Value.Date.AddDays(1).AddTicks(-1);

            int totalComplainants = 0;
            Dictionary<string, int[]> caseData = new Dictionary<string, int[]>();
            HashSet<string> seenCaseIDs = new HashSet<string>();

            string[] collections = { "caselist", "onlinecaselist" };

            foreach (string collection in collections)
            {
                QuerySnapshot snapshot = await FirebaseInitialization.Database.Collection(collection).GetSnapshotAsync();

                foreach (DocumentSnapshot doc in snapshot.Documents)
                {
                    if (!doc.Exists) continue;

                    Dictionary<string, object> caseDetails = null;

                    // Extract caseDetails
                    if (collection == "caselist" && doc.ContainsField("CaseDetails"))
                        caseDetails = doc.GetValue<Dictionary<string, object>>("CaseDetails");

                    if (collection == "onlinecaselist" && doc.ContainsField("caseDetails"))
                        caseDetails = doc.GetValue<Dictionary<string, object>>("caseDetails");

                    if (caseDetails == null) continue;

                    string dateKey = collection == "caselist" ? "ComplaintDate" : "complaintDate";
                    if (!caseDetails.ContainsKey(dateKey)) continue;

                    if (!DateTime.TryParse(caseDetails[dateKey]?.ToString(), out DateTime complaintDate)) continue;
                    if (complaintDate < startDate || complaintDate > endDate) continue;

                    string caseID = doc.Id;
                    if (seenCaseIDs.Contains(caseID)) continue;
                    seenCaseIDs.Add(caseID);
                    totalComplainants++;

                    string rawCase = caseDetails.ContainsKey("VAWCCase") ? caseDetails["VAWCCase"]?.ToString() :
                                     caseDetails.ContainsKey("vawcCase") ? caseDetails["vawcCase"]?.ToString() : "";

                    string vawcCase = MapCase(rawCase);

                    string rawSub = caseDetails.ContainsKey("SubCase") ? caseDetails["SubCase"]?.ToString() :
                                    caseDetails.ContainsKey("subCase") ? caseDetails["subCase"]?.ToString() : "";

                    string subCase = MapSubCase(rawSub);

                    string referredTo = caseDetails.ContainsKey("ReferredTo") ? caseDetails["ReferredTo"]?.ToString()?.ToUpper() :
                                        caseDetails.ContainsKey("referredTo") ? caseDetails["referredTo"]?.ToString()?.ToUpper() : "";

                    int refIndex = GetReferralIndex(referredTo);

                    // Count main case
                    if (!string.IsNullOrEmpty(vawcCase))
                    {
                        if (!caseData.ContainsKey(vawcCase)) caseData[vawcCase] = new int[6];
                        caseData[vawcCase][0]++;

                        if (refIndex != -1)
                            caseData[vawcCase][refIndex]++;
                    }

                    // Count subcases for RA 9262
                    if (vawcCase == "RA 9262" && !string.IsNullOrEmpty(subCase))
                    {
                        if (!caseData.ContainsKey(subCase)) caseData[subCase] = new int[6];
                        caseData[subCase][0]++;

                        if (refIndex != -1)
                            caseData[subCase][refIndex]++;
                    }
                }
            }

            // Update labels
            label21.Text = totalComplainants.ToString();
            label22.Text = totalComplainants.ToString();
            label23.Text = totalComplainants.ToString();

            UpdateTable(caseData);
        }


        private string MapCase(string raw)
        {
            raw = raw?.Trim().ToUpper();
            if (string.IsNullOrEmpty(raw)) return "";

            if (raw.Contains("R.A. 9262")) return "RA 9262";
            if (raw.Contains("R.A. 8353")) return "RA 8353";
            if (raw.Contains("R.A. 9208") || raw.Contains("10364")) return "RA 9208/10364";
            if (raw.Contains("R.A. 7877")) return "RA 7877";
            return "";
        }

        private string MapSubCase(string raw)
        {
            raw = raw?.Trim().ToUpperInvariant();
            if (string.IsNullOrEmpty(raw)) return "";

            if (raw.Contains("SEXUAL")) return "Sexual";
            if (raw.Contains("PSYCHOLOGICAL")) return "Psychological";
            if (raw.Contains("PHYSICAL")) return "Physical";
            if (raw.Contains("ECONOMIC")) return "Economic";

            return "";
        }

        private int GetReferralIndex(string referredTo)
        {
            if (referredTo.Contains("CSWDO")) return 1;
            if (referredTo.Contains("PNP")) return 2;
            if (referredTo.Contains("COURT")) return 3;
            if (referredTo.Contains("HOSPITAL")) return 4;
            if (referredTo.Contains("NBI") || referredTo.Contains("PAO") || referredTo.Contains("OTHERS")) return 5;
            return -1;
        }


        private void UpdateTable(Dictionary<string, int[]> data)
        {
            // Loop through all rows in the table
            for (int row = 1; row < tableLayoutPanel1.RowCount; row++)
            {
                Label rowLabel = tableLayoutPanel1.GetControlFromPosition(0, row) as Label;
                if (rowLabel == null) continue;

                string caseKey = rowLabel.Text.Trim();

                if (data.ContainsKey(caseKey))
                {
                    int[] counts = data[caseKey];

                    // Update all columns for all rows
                    for (int col = 1; col <= 6; col++)
                    {
                        Label cell = tableLayoutPanel1.GetControlFromPosition(col, row) as Label;
                        if (cell != null)
                        {
                            cell.Text = counts[col - 1].ToString();
                        }
                    }
                }
                else
                {
                    // If no data found for this row, set all counts to 0
                    for (int col = 1; col <= 6; col++)
                    {
                        Label cell = tableLayoutPanel1.GetControlFromPosition(col, row) as Label;
                        if (cell != null)
                        {
                            cell.Text = "0";
                        }
                    }
                }
            }
        }

        private void SetupTableLayout()
        {
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tableLayoutPanel1.BorderStyle = BorderStyle.FixedSingle;
            tableLayoutPanel1.BackColor = Color.Black;

            string[] headers = {
        "NATURE OF CASE", "Total No. of Victims", "CSWDO", "PNP", "Court",
        "Hospital", "OTHERS (NBI/PAO),", "Total No.BPO's Issued", ""
    };

            // Set header row
            for (int col = 0; col < headers.Length; col++)
            {
                Label header = new Label
                {
                    Text = headers[col],
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    BackColor = Color.LightGray,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding(0)
                };
                tableLayoutPanel1.Controls.Add(header, col, 0);
            }

            // Set data rows
            string[,] rows = new string[,]
            {
        { "RA 9262", "", "", "", "", "", "", "" },
        { "Sexual", "", "", "", "", "", "", "" },
        { "Psychological", "", "", "", "", "", "", "" },
        { "Physical", "", "", "", "", "", "", "" },
        { "Economic", "", "", "", "", "", "", "" },
        { "RA 8353", "", "", "", "", "", "", "" },
        { "RA 9208/10364", "", "", "", "", "", "", "" },
        { "RA 7877", "", "", "", "", "", "", "" }
            };

            // Populate the table with rows
            for (int row = 0; row < rows.GetLength(0); row++)
            {
                for (int col = 0; col < rows.GetLength(1); col++)
                {
                    Label cell = new Label
                    {
                        Text = rows[row, col],
                        TextAlign = ContentAlignment.MiddleCenter,
                        Dock = DockStyle.Fill,
                        BackColor = Color.White,
                        Margin = new Padding(0)
                    };

                    // Left-align first column
                    if (col == 0)
                    {
                        cell.TextAlign = ContentAlignment.MiddleLeft;
                        cell.Padding = new Padding(10, 0, 0, 0);
                    }

                    tableLayoutPanel1.Controls.Add(cell, col, row + 1);
                }
            }

            // Configure column styles
            tableLayoutPanel1.ColumnStyles.Clear();
            for (int i = 0; i < headers.Length; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            }
        }

    }
}
