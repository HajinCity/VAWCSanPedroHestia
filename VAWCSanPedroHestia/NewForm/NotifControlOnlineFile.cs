using System;
using System.Windows.Forms;

namespace VAWCSanPedroHestia.NewForm
{
    public partial class NotifControlOnlineFile : UserControl
    {
        public string CaseID { get; set; }

        public NotifControlOnlineFile()
        {
            InitializeComponent();
        }

        // Custom setup method for the notification card
        public void SetData(string caseId, string message, DateTime timestamp)
        {
            CaseID = caseId;

            // Uncommented and fixed the label2 assignment
            label2.Text = timestamp.ToString("MMMM dd, yyyy - hh:mm tt");

            // Optional: If you want to use the message parameter
            // lblMessage.Text = message;
        }

        private void btnViewComplaint_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Viewing details for Case ID: {CaseID}", "View Complaint");
        }
    }
}