using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace SynchEd
{
    public class SynchedDB
    {
        private static string connString = @"Data Source = mlatjac.database.windows.net; Initial Catalog = TestDB; User ID = serveradmin; Password=rootR00t";
        private SqlConnection conn;
        private static SynchedDB DBUniqueInstance;

        public static SynchedDB GetInstance()
        {
            if (DBUniqueInstance == null)
                DBUniqueInstance = new SynchedDB();
            return DBUniqueInstance;
        }

        private SynchedDB()
        {
            // Open and store connection
            conn = new SqlConnection();
            conn.ConnectionString = connString;
            conn.Open();
        }

        // Fetch a user by key
        public SynchedUser GetUserByKey(String strKey)
        {
            return new SynchedUser { Name = "Suzan" }; //FIXME: actually get a user from DB
        }
    }

    public class SynchedModel
    {
        private static SynchedModel ModelUniqueInstance;

        private SynchedDB ModelDB;
        private System.Windows.Documents.FlowDocument Doc;
        private SynchedDocument SDCurrentDoc; // Current SynchedDocument being edited
        private SynchedUser currentUser; // User for whom we're editing and managing documents

        // Get an instance of the model. Pass it the RichTextBox's Flow Document and a synched user key
        public static SynchedModel GetInstance(FlowDocument fdDoc, String strSynchedUser)
        {
            if (ModelUniqueInstance == null)
                ModelUniqueInstance = new SynchedModel(fdDoc, strSynchedUser);

            return ModelUniqueInstance;
        }

        private SynchedModel(FlowDocument fdDoc, String strUser)
        {
            ModelDB = SynchedDB.GetInstance();
            Doc = fdDoc;

            // Get a user object for the current user
            currentUser = ModelDB.GetUserByKey(strUser);
        }

        // Return all system users
        public List<SynchedUser> GetAllUsers()
        {
            List<SynchedUser> lstAllUsers = new List<SynchedUser>();
            lstAllUsers.Add(new SynchedUser { Name = "Henry" });
            lstAllUsers.Add(new SynchedUser { Name = "Frank" });
            lstAllUsers.Add(new SynchedUser { Name = "Mary" });
            lstAllUsers.Add(new SynchedUser { Name = "Barry" });
            lstAllUsers.Add(new SynchedUser { Name = "Fred" });
            lstAllUsers.Add(new SynchedUser { Name = "Erika" });
            lstAllUsers.Add(new SynchedUser { Name = "Suzan" });
            lstAllUsers.Add(new SynchedUser { Name = "William" });
            lstAllUsers.Add(new SynchedUser { Name = "Arthur" });
            return lstAllUsers;
        }

        // Return all user accesses for current document
        public List<SynchedUserAccess> GetAllDocumentAccesses()
        {
            List<SynchedUserAccess> lstUserAccess = new List<SynchedUserAccess>();

            lstUserAccess.Add(new SynchedUserAccess { Name = "Frank", CanWrite = true });
            lstUserAccess.Add(new SynchedUserAccess { Name = "Mary", CanWrite = true });
            lstUserAccess.Add(new SynchedUserAccess { Name = "Barry", CanWrite = false });

            return lstUserAccess;

        }

        // Return all a user's documents
        public List<SynchedDocument> GetAllUsersDocs()
        {
            // FIXME actually get docs based on currentUser
            List<SynchedDocument> lstMyDocs = new List<SynchedDocument>();
            lstMyDocs.Add(new SynchedDocument { Name = "Monday Menu" });
            lstMyDocs.Add(new SynchedDocument { Name = "CV" });
            lstMyDocs.Add(new SynchedDocument { Name = "Shopping List" });
            lstMyDocs.Add(new SynchedDocument { Name = "My Novel -- Chapter 1" });
            lstMyDocs.Add(new SynchedDocument { Name = "July English Assignment" });

            return lstMyDocs;

        }

        // Return all docs where current user is a collaborator. If bOnlyWriteable is true, return only document with a write access
        public List<SynchedDocument> GetAllCollaborationDocs(bool bOnlyWriteable)
        {
            // FIXME Actually return documents a person is collaborating on
            List<SynchedDocument> lstCollabDocs = new List<SynchedDocument>();
            lstCollabDocs.Add(new SynchedDocument { Name = "School Project 1", OwnerName = "Mary" });
            lstCollabDocs.Add(new SynchedDocument { Name = "Wedding Plans", OwnerName = "Suzan" });
            lstCollabDocs.Add(new SynchedDocument { Name = "Daily Scrum", OwnerName = "Sandy" });
            lstCollabDocs.Add(new SynchedDocument { Name = "IT Course Outline", OwnerName = "Dave" });

            return lstCollabDocs;
        }

    }
}
