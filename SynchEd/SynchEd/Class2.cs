using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            // Select
            SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[User] WHERE UserKey=@UserKey", conn);
            command.Parameters.Add(new SqlParameter("UserKey", strKey));

            using (SqlDataReader reader = command.ExecuteReader())
            {
                SynchedUser suUserForKey = null;
                while (reader.Read())
                {
                    // Create a Person object
                    suUserForKey = new SynchedUser { Id = (int)reader["Id"], Name = (string)reader["Name"], UserKey = (string)reader["UserKey"] };
                }

                return suUserForKey;
            }
            // FIXME Remove test data stub below
            //return new SynchedUser { Name = "Suzan" }; //FIXME: actually get a user from DB
        }
        public string GetDocXamlContent(int Id)
        {
            string XamlContent = "";
            // Select
            SqlCommand command = new SqlCommand("SELECT * FROM Document WHERE ID=@DocId", conn);
            command.Parameters.Add(new SqlParameter("DocId", Id));


            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    XamlContent += reader["XAMLContent"];
                }
            }
            return XamlContent;
        }
        public void SetDocXamlContent(int Id, string XamlContent)
        {
            // Update
            SqlCommand updateCommand = new SqlCommand("UPDATE Document Set XAMLContent = @NewXaml", conn);
            updateCommand.Parameters.Add(new SqlParameter("NewXaml", XamlContent));
            updateCommand.ExecuteNonQuery();
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

            if (currentUser == null)
            {
                throw new Exception("No user found for user key " + strUser);
            }

            // Create an object for the current document
            SDCurrentDoc = new SynchedDocument { Id = 1, Owner = currentUser }; // FIXME Document ID is hardcoded
        }


        // Method to clone a flow document. Used to return a copy to print
        public static FlowDocument Clone(FlowDocument fDoc)
        {
            FlowDocument fdClone = new FlowDocument();

            TextRange trOriginal = new TextRange(fDoc.ContentStart, fDoc.ContentEnd);
            MemoryStream stream = new MemoryStream();
            trOriginal.Save(stream, DataFormats.Xaml); // Format might be Xaml

            //string xamlText = Encoding.UTF8.GetString(stream.ToArray());
            //tbXAMLPreview.Text = xamlText;

            //MemoryStream stream = new MemoryStream();
            TextRange trClone = new TextRange(fdClone.ContentStart, fdClone.ContentEnd);
            //stream = new MemoryStream(Encoding.UTF8.GetBytes(tbXAMLPreview.Text));
            trClone.Load(stream, System.Windows.DataFormats.Xaml);
            stream.Close();

            return fdClone;
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
            lstCollabDocs.Add(new SynchedDocument { Name = "School Project 1", Owner = (new SynchedUser { Name = "Mary" }) });
            lstCollabDocs.Add(new SynchedDocument { Name = "Wedding Plans", Owner = (new SynchedUser { Name = "Suzan" }) });
            lstCollabDocs.Add(new SynchedDocument { Name = "Daily Scrum", Owner = (new SynchedUser { Name = "Sandy" }) });
            lstCollabDocs.Add(new SynchedDocument { Name = "IT Course Outline", Owner = (new SynchedUser { Name = "Dave" }) });

            return lstCollabDocs;
        }

        // Return the user's name
        public String UserName
        {
            get
            {
                return currentUser.Name;
            }
        }

        // Get Content from DB
        public void RetrieveContent()
        {
            SDCurrentDoc.XamlContent = ModelDB.GetDocXamlContent(SDCurrentDoc.Id);
        }
        // Save content to DB
        public void SaveContent()
        {
            ModelDB.SetDocXamlContent(SDCurrentDoc.Id, SDCurrentDoc.XamlContent);
        }

        // Load content into Editor Flow Doc
        public void LoadDocContent()
        {
            MemoryStream stream = new MemoryStream();
            TextRange range = new TextRange(Doc.ContentStart, Doc.ContentEnd);
            stream = new MemoryStream(Encoding.UTF8.GetBytes(SDCurrentDoc.XamlContent));
            range.Load(stream, System.Windows.DataFormats.Xaml);
            stream.Close();
        }
        // Update Doc content from Editor
        public void UpdateDocContent()
        {
            TextRange range = new TextRange(Doc.ContentStart, Doc.ContentEnd);
            MemoryStream stream = new MemoryStream();
            range.Save(stream, DataFormats.Xaml);
            SDCurrentDoc.XamlContent = Encoding.UTF8.GetString(stream.ToArray());
        }
        // Create Document in DB
        public void CreateDocument()
        {

        }
    }
}
