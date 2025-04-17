using FirebaseAdmin.Messaging;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Forms;

namespace VAWCSanPedroHestia.NewForm
{
    public partial class ViewComplaintForm : Form
    {
        private string _caseId;

        public ViewComplaintForm()
        {
            InitializeComponent();
        }

        // ✅ Load full complaint data with incident details
        public void LoadComplaintData(
            string caseId,
            string firstName, string middleName, string lastName,
            string purok, string city,
            string sex, string contact,
            string age, string civilStatus, string religion, string nationality, string occupation,
            string respFirstName, string respMiddleName, string respLastName,
            string respAlias, string respSex, string respContact, string respAge,
            string respCivilStatus, string respReligion, string respNationality, string respOccupation,
            string relationship,
            DateTime complaintDate,
            DateTime incidentDate,
            string complaintDetails,
            string status,
            string place, string incidentPurok, string incidentBarangay,
            string incidentMunicipality, string incidentProvince, string incidentRegion
        )
        {
            _caseId = caseId;

            // 👩 Complainant
            CompFullName.Text = $"{firstName} {middleName} {lastName}";
            CompAge.Text = age;
            CompSex.Text = sex;
            CompSex.Text = civilStatus;
            CompReligion.Text = religion;
            CompNationality.Text = nationality;
            CompOccupation.Text = occupation;
            CompAddress.Text = $"{purok}, {city}";
            CompContact.Text = contact;

            // 👨 Respondent
            RespFullName.Text = $"{respFirstName} {respMiddleName} {respLastName}";
            RespAlias.Text = respAlias;
            RespSex.Text = respSex;
            RespAge.Text = respAge;
            RespCivilStatus.Text = respCivilStatus;
            RespReligion.Text = respReligion;
            RespNationality.Text = respNationality;
            RespOccupation.Text = respOccupation;
            RespContact.Text = respContact;
            RtoC.Text = relationship;

            // 📄 Complaint Info
            ComplaintID.Text = caseId;
            ComplaintDate.Text = complaintDate.ToString("MMMM dd, yyyy - hh:mm tt");
            IncidentDate.Text = incidentDate != DateTime.MinValue ? incidentDate.ToString("MMMM dd, yyyy") : "N/A";
            ComplaintDetails.Text = complaintDetails;
          //  ComplaintStatus.Text = status;

            // 📍 Incident Location
          //  PlaceOfIncident.Text = place;
            IncidentAddress.Text = $"{incidentPurok}, {incidentBarangay}, {incidentMunicipality}, {incidentProvince}, {incidentRegion}";

            // Optional color-code status
           // if (status == "Pending") ComplaintStatus.ForeColor = System.Drawing.Color.Orange;
           // else if (status == "Accepted") ComplaintStatus.ForeColor = System.Drawing.Color.Green;
           // else if (status == "Rejected") ComplaintStatus.ForeColor = System.Drawing.Color.Red;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            string originalCollection = "Complaints";
            string newCollection = "onlinecaselist";

            var db = FirebaseInitialization.Database;

            try
            {
                DocumentReference originalDocRef = db.Collection(originalCollection).Document(_caseId);
                DocumentSnapshot snapshot = await originalDocRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    Dictionary<string, object> complaintData = snapshot.ToDictionary();

                    DocumentReference newDocRef = db.Collection(newCollection).Document(_caseId);
                    await newDocRef.SetAsync(complaintData);

                    await originalDocRef.DeleteAsync();

                    MessageBox.Show("Complaint accepted and transferred.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Complaint not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var db = FirebaseInitialization.Database;

            try
            {
                DocumentReference docRef = db.Collection("Complaints").Document(_caseId);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                if (snapshot.Exists)
                {
                    await docRef.DeleteAsync();
                    MessageBox.Show("Complaint has been rejected and deleted.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Complaint not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error rejecting complaint: " + ex.Message);
            }
        }
    }
}
