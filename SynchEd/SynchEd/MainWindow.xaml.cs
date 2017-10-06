using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SynchEd
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CollaboratorsDialog dlgCollabs;
        private SelectDocumentDialog dlgSelectDoc;
        private SaveAsDialog dlgSaveAs;

        // FIXME Remove the next procedure/ test
        private void LoopThroughDialogs()
        {
            dlgCollabs.ShowDialog();
            dlgSelectDoc.ShowDialog();
            dlgSaveAs.ShowDialog();
        }
        // FIXME Remove test data procedure
        private void SetupTestData()
        {
            // Add data to test list view
            List<SynchedUserAccess> lstUserAccess = new List<SynchedUserAccess>();

            lstUserAccess.Add(new SynchedUserAccess { Name = "Frank", CanWrite = true });
            lstUserAccess.Add(new SynchedUserAccess { Name = "Mary", CanWrite = true });
            lstUserAccess.Add(new SynchedUserAccess { Name = "Barry", CanWrite = false });

            dlgCollabs.lvCollaborators.ItemsSource = lstUserAccess;

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
            dlgCollabs.lvUsers.ItemsSource = lstAllUsers;

            // Add test data
            List<SynchedDocument> lstMyDocs = new List<SynchedDocument>();
            lstMyDocs.Add(new SynchedDocument { Name = "Monday Menu" });
            lstMyDocs.Add(new SynchedDocument { Name = "CV" });
            lstMyDocs.Add(new SynchedDocument { Name = "Shopping List" });
            lstMyDocs.Add(new SynchedDocument { Name = "My Novel -- Chapter 1" });
            lstMyDocs.Add(new SynchedDocument { Name = "July English Assignment" });
            dlgSelectDoc.lvMyDocs.ItemsSource = lstMyDocs;

            List<SynchedDocument> lstCollabDocs = new List<SynchedDocument>();
            lstCollabDocs.Add(new SynchedDocument { Name = "School Project 1", OwnerName = "Mary" });
            lstCollabDocs.Add(new SynchedDocument { Name = "Wedding Plans", OwnerName = "Suzan" });
            lstCollabDocs.Add(new SynchedDocument { Name = "Daily Scrum", OwnerName = "Sandy" });
            lstCollabDocs.Add(new SynchedDocument { Name = "IT Course Outline", OwnerName = "Dave" });
            dlgSelectDoc.lvCollaboratorDocs.ItemsSource = lstCollabDocs;

            // Add test data
            List<SynchedDocument> lstMyDocsSaveAs = new List<SynchedDocument>();
            lstMyDocs.Add(new SynchedDocument { Name = "Monday Menu" });
            lstMyDocs.Add(new SynchedDocument { Name = "CV" });
            lstMyDocs.Add(new SynchedDocument { Name = "Shopping List" });
            lstMyDocs.Add(new SynchedDocument { Name = "My Novel -- Chapter 1" });
            lstMyDocs.Add(new SynchedDocument { Name = "July English Assignment" });
            dlgSaveAs.lvMyDocs.ItemsSource = lstMyDocsSaveAs;

        }

        public MainWindow()
        {
            InitializeComponent();

            // Initialize Dialogs
            dlgCollabs = new CollaboratorsDialog();
            dlgSelectDoc = new SelectDocumentDialog();
            dlgSaveAs = new SaveAsDialog();

            // FIXME Remove test data and dialog loop
            SetupTestData();
            LoopThroughDialogs();
        }
    }
}
