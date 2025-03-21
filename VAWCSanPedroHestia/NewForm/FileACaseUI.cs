using Google.Cloud.Firestore;
using System;
using System.Linq;
using System.Windows.Forms;
using VAWCSanPedroHestia.NewForm;

namespace VAWCSanPedroHestia
{
    public partial class FileACaseUI : Form
    {
        public FileACaseUI()
        {
            InitializeComponent();
            SetComplaintDateRestrictions();
            RAVioCase.SelectedIndexChanged += RAVioCase_SelectedIndexChanged;
        }
        private void SetComplaintDateRestrictions()
        {
            ComplaintDate.Value = DateTime.Today;
            ComplaintDate.MinDate = DateTime.Today; 
            ComplaintDate.MaxDate = DateTime.Today; 
            ComplaintDate.Enabled = false; 
        }

        private void FileACaseUI_Load(object sender, EventArgs e)
        {
            GenerateCaseNumber();
        }

        public async void GenerateCaseNumber()
        {
            string currentYear = DateTime.Now.Year.ToString();
            string prefix = $"{currentYear}-";

            var casesCollection = FirebaseInitialization.Database.Collection("caselist");
            var snapshot = await casesCollection
                .WhereGreaterThanOrEqualTo(FieldPath.DocumentId, prefix)
                .WhereLessThan(FieldPath.DocumentId, $"{int.Parse(currentYear) + 1}-")
                .GetSnapshotAsync();

            int newNumber = 1;

            if (snapshot.Documents.Any())
            {
                int maxExistingNumber = snapshot.Documents
                    .Select(doc => doc.Id.Replace(prefix, ""))
                    .Where(num => int.TryParse(num, out _))
                    .Select(int.Parse)
                    .Max();

                newNumber = maxExistingNumber + 1;
            }

            string newCaseId = $"{prefix}{newNumber:000}";

            Caseno.Text = newCaseId;
        }

        private async void save_casebtn_Click(object sender, EventArgs e)
        {
            await FileACaseSaveToDb.SaveCaseAsync(this);
            ClearForm(this); // ✅ Call ClearForm properly
        }

        // ✅ **Clears all TextBoxes, ComboBoxes (Properly!), and resets DateTimePickers**
        private void ClearForm(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is TextBox txt)
                    txt.Clear();

                if (ctrl is ComboBox cmb)
                {
                    cmb.SelectedIndex = -1; // ✅ Properly resets selection
                    cmb.Text = "";          // ✅ Clears any manually entered text
                }

                if (ctrl is DateTimePicker dtp)
                    dtp.Value = DateTime.Today; // Only sets the date portion, ignoring the time


                // **If the control contains more controls inside (like Panels, GroupBoxes)**
                if (ctrl.HasChildren)
                    ClearForm(ctrl); // Recursive call to clear controls inside Panels, GroupBoxes
            }

            // **Refresh Case Number after clearing**
            GenerateCaseNumber();
        }

        private void RAVioCase_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear existing items in the sub-case combo box
            RAVioSubCase.Items.Clear();

            // Get the selected value from RAVioCase
            string selectedCategory = RAVioCase.SelectedItem?.ToString();

            // Check which category is selected
            if (selectedCategory == "R.A. 9262: Anti Violence Against Women and their Children Act")
            {
                // Add specific sub-cases for R.A. 9262
                RAVioSubCase.Items.Add("Sexual Abuse");
                RAVioSubCase.Items.Add("Psychological");
                RAVioSubCase.Items.Add("Physical");
                RAVioSubCase.Items.Add("Economic");
            }
            else
            {
                // If it's any other category, only show "None"
                RAVioSubCase.Items.Add("None");
            }

            // ✅ Set default selection to "None"
            RAVioSubCase.SelectedIndex = 0;
        }
    }
}
