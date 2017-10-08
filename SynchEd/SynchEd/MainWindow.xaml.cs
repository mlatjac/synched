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
        // GUI-related
        private CollaboratorsDialog dlgCollabs;
        private SelectDocumentDialog dlgSelectDoc;
        private SaveAsDialog dlgSaveAs;

        // Model related
        private SynchedModel Model;

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
            // Add data to test views

            dlgCollabs.lvCollaborators.ItemsSource = Model.GetAllDocumentAccesses();

            dlgCollabs.lvUsers.ItemsSource = Model.GetAllUsers();

            dlgSelectDoc.lvMyDocs.ItemsSource = Model.GetAllUsersDocs();

            dlgSelectDoc.lvCollaboratorDocs.ItemsSource = Model.GetAllCollaborationDocs(false); // Don't limit to writeOnly Docs


            // Add test data
           dlgSaveAs.lvMyDocs.ItemsSource = Model.GetAllUsersDocs();

        }

        private void AbortStartupAndExit()
        {
            // FIXME Application.Current.Shutdown() doesn't work !
            Application.Current.Shutdown();
        }

        public MainWindow()
        {
            // Initialize GUI
            try
            {
                // Main Window Components
                InitializeComponent();

                // Dialogs
                dlgCollabs = new CollaboratorsDialog();
                dlgSelectDoc = new SelectDocumentDialog();
                dlgSaveAs = new SaveAsDialog();
            }
            catch (Exception exEx)
            {
                MessageBox.Show("There was a problem intializing the user interface " + exEx.Message);
                AbortStartupAndExit();
            }

            try
            {
                // Initialize Model

                // Fetch user information
                String strSynchedUser = "UUUU"; //FIXME: actually fetch user info from resources

                // Complete model initialization
                Model = SynchedModel.GetInstance(rtbDocumentEditor.Document, strSynchedUser);

            }
            catch (Exception exEx)
            {
                MessageBox.Show("There was a problem connecting to database " + exEx.Message);
                AbortStartupAndExit();
            }

            // FIXME Remove test data and dialog loop
            SetupTestData();
            LoopThroughDialogs();
        }
    }
}
