using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VAWCSanPedroHestia
{
    public partial class FileACaseUI : Form
    {
        public FileACaseUI()
        {
            InitializeComponent();
            GenerateCaseNumber();
        }

        private void FileACaseUI_Load(object sender, EventArgs e)
        {
            GenerateCaseNumber();
        }


        private async void GenerateCaseNumber()
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


    }
}
