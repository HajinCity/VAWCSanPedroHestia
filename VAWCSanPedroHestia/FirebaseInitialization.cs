using System;
using Google.Cloud.Firestore;

namespace VAWCSanPedroHestia
{
    public static class FirebaseInitialization
    {
        private static readonly FirestoreDb _firestoreDb;

        static FirebaseInitialization()
        {
            // ✔️ Confirm this EXACT path points to your credentials file.
            string credentialsPath = @"C:\Users\WINDOWS 10\source\repos\HajinCity\VAWCSanPedroHestia\VAWCSanPedroHestia\FirebaseJSONFile\vawc-hestiaxisanpedro2025-firebase-adminsdk-fbsvc-89e0f144fb.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);

            // ✔️ Ensure correct Firebase project ID here:
            string projectId = "vawc-hestiaxisanpedro2025";
            _firestoreDb = FirestoreDb.Create(projectId);
        }

        public static FirestoreDb Database => _firestoreDb;
    }
}
