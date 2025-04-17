using System;
using System.Drawing;
using System.Windows.Forms;

namespace VAWCSanPedroHestia.NewForm
{
    public partial class NotifControlOnlineFile : UserControl
    {
        // 📄 Complaint Info
        public string CaseID { get; set; }
        public DateTime ComplaintDate { get; set; }
        public DateTime IncidentDate { get; set; }
        public string ComplaintDetails { get; set; }
        public string Status { get; set; }

        // 👩 Complainant Info
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string CivilStatus { get; set; }
        public string Religion { get; set; }
        public string Nationality { get; set; }
        public string Occupation { get; set; }
        public string Purok { get; set; }
        public string City { get; set; }
        public string Contact { get; set; }

        // 👨 Respondent Info
        public string RespFirstName { get; set; }
        public string RespMiddleName { get; set; }
        public string RespLastName { get; set; }
        public string RespAlias { get; set; }
        public string RespSex { get; set; }
        public string RespContact { get; set; }
        public string RespAge { get; set; }
        public string RespCivilStatus { get; set; }
        public string RespReligion { get; set; }
        public string RespNationality { get; set; }
        public string RespOccupation { get; set; }
        public string Relationship { get; set; }

        // 📍 Incident Location Info
        public string Place { get; set; }
        public string IncidentPurok { get; set; }
        public string IncidentBarangay { get; set; }
        public string IncidentMunicipality { get; set; }
        public string IncidentProvince { get; set; }
        public string IncidentRegion { get; set; }

        public NotifControlOnlineFile()
        {
            InitializeComponent();
        }

        public void SetData(
            string caseId,
            string firstName, string middleName, string lastName,
            string age, string sex, string civilStatus, string religion, string nationality, string occupation,
            string purok, string city, string contact,
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
            CaseID = caseId;
            ComplaintDate = complaintDate;
            IncidentDate = incidentDate;
            ComplaintDetails = complaintDetails;
            Status = status;

            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Age = age;
            Sex = sex;
            CivilStatus = civilStatus;
            Religion = religion;
            Nationality = nationality;
            Occupation = occupation;
            Purok = purok;
            City = city;
            Contact = contact;

            RespFirstName = respFirstName;
            RespMiddleName = respMiddleName;
            RespLastName = respLastName;
            RespAlias = respAlias;
            RespSex = respSex;
            RespContact = respContact;
            RespAge = respAge;
            RespCivilStatus = respCivilStatus;
            RespReligion = respReligion;
            RespNationality = respNationality;
            RespOccupation = respOccupation;
            Relationship = relationship;

            Place = place;
            IncidentPurok = incidentPurok;
            IncidentBarangay = incidentBarangay;
            IncidentMunicipality = incidentMunicipality;
            IncidentProvince = incidentProvince;
            IncidentRegion = incidentRegion;

            label2.Text = complaintDate.ToString("MMMM dd, yyyy - hh:mm tt");
        }

        private void btnViewComplaint_Click(object sender, EventArgs e)
        {
            ViewComplaintForm viewForm = new ViewComplaintForm();
            viewForm.Text = $"Complaint Details - Case ID: {CaseID}";

            viewForm.LoadComplaintData(
                CaseID,
                FirstName, MiddleName, LastName,
                Purok, City,
                Sex, Contact,
                Age, CivilStatus, Religion, Nationality, Occupation,
                RespFirstName, RespMiddleName, RespLastName,
                RespAlias, RespSex, RespContact, RespAge,
                RespCivilStatus, RespReligion, RespNationality, RespOccupation,
                Relationship,
                ComplaintDate,
                IncidentDate,
                ComplaintDetails,
                Status,
                Place, IncidentPurok, IncidentBarangay,
                IncidentMunicipality, IncidentProvince, IncidentRegion
            );

            viewForm.ShowDialog();
        }
    }
}
