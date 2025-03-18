using System;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using System.Windows.Forms;

namespace VAWCSanPedroHestia.NewForm
{
    public static class FileACaseSaveToDb
    {
        public static async Task SaveCaseAsync(FileACaseUI form)
        {
            try
            {
                CollectionReference casesCollection = FirebaseInitialization.Database.Collection("caselist");

                var caseData = new
                {
                    Complainant = new
                    {
                        LastName = form.Ctxtlname.Text ?? "",
                        FirstName = form.CtxtFname.Text ?? "",
                        MiddleName = form.CtxtMname.Text ?? "",
                        SexIdentification = form.ComboCsex.Text ?? "",
                        CivilStatus = form.CompCivlStatus.Text ?? "",  // ✅ Added Complainant Civil Status
                        Age = form.CompAge.Text ?? "",
                        Religion = form.CtextRegion.Text ?? "",
                        CellNumber = form.Cnumno.Text ?? "",
                        Address = new
                        {
                            Purok = form.CtxtPurok.Text ?? "",
                            Barangay = form.Ctxtbrgy.Text ?? "",
                            Municipality = form.Ctxtmncp.Text ?? "",
                            Province = form.Ctxtprvnc.Text ?? "",
                            Region = form.Ctxtrgn.Text ?? ""
                        }
                    },
                    Respondent = new
                    {
                        LastName = form.RtxtLname.Text ?? "",
                        FirstName = form.RtextFname.Text ?? "",
                        MiddleName = form.RtxtMname.Text ?? "",
                        Alias = form.RtxtAllias.Text ?? "",
                        SexIdentification = form.RCmboSex.Text ?? "",
                        CivilStatus = form.ResCivilStatus.Text ?? "",  // ✅ Added Respondent Civil Status
                        Age = form.ResAge.Text ?? "",
                        Religion = form.Rtextrgn.Text ?? "",
                        CellNumber = form.Rnumcntct.Text ?? "",
                        RelationshipToComplainant = form.Rcmborltn.Text ?? "",
                        Address = new
                        {
                            Purok = form.Rtxtprk.Text ?? "",
                            Barangay = form.Rtxtbrgy.Text ?? "",
                            Municipality = form.Rtxtmncp.Text ?? "",
                            Province = form.Rtxtprvnc.Text ?? "",
                            Region = form.Rtxtrgn.Text ?? ""
                        }
                    },
                    CaseDetails = new
                    {
                        CaseNumber = form.Caseno.Text,
                        ComplaintDate = form.ComplaintDate.Value.ToString("yyyy-MM-dd"),
                        VAWCCase = form.RAVioCase.Text ?? "",
                        SubCase = form.RAVioSubCase.Text ?? "",
                        CaseStatus = form.CaseStatus.Text ?? "",
                        ReferredTo = form.RefTo.Text ?? "",
                        IncidentDate = form.IncidentDate.Value.ToString("yyyy-MM-dd"),
                        PlaceOfIncident = new
                        {
                            Place = form.PlaceOfIncident.Text ?? "",
                            Purok = form.piPurok.Text ?? "",
                            Barangay = form.piBarangay.Text ?? "",
                            Municipality = form.piMunicpal.Text ?? "",
                            Province = form.piProvince.Text ?? "",
                            Region = form.piRegion.Text ?? ""
                        },
                        IncidentDescription = form.DescriptionIncident.Text ?? ""
                    }
                };

                string documentId = form.Caseno.Text;

                await casesCollection.Document(documentId).SetAsync(caseData);

                MessageBox.Show("Case successfully saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving case: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
