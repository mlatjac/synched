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
using System.Windows.Shapes;

namespace MultiWindowTest
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class SelectDocumentDialog : Window
    {
        public SelectDocumentDialog()
        {
            InitializeComponent();

            // Add test data
            List<SynchedDocument> lstMyDocs = new List<SynchedDocument>();
            lstMyDocs.Add(new SynchedDocument { Name = "Monday Menu" });
            lstMyDocs.Add(new SynchedDocument { Name = "CV" });
            lstMyDocs.Add(new SynchedDocument { Name = "Shopping List" });
            lstMyDocs.Add(new SynchedDocument { Name = "My Novel -- Chapter 1" });
            lstMyDocs.Add(new SynchedDocument { Name = "July English Assignment" });
            lvMyDocs.ItemsSource = lstMyDocs;

            List<SynchedDocument> lstCollabDocs = new List<SynchedDocument>();
            lstCollabDocs.Add(new SynchedDocument { Name = "School Project 1", OwnerName = "Mary" });
            lstCollabDocs.Add(new SynchedDocument { Name = "Wedding Plans", OwnerName = "Suzan" });
            lstCollabDocs.Add(new SynchedDocument { Name = "Daily Scrum", OwnerName = "Sandy" });
            lstCollabDocs.Add(new SynchedDocument { Name = "IT Course Outline", OwnerName = "Dave" });
            lvCollaboratorDocs.ItemsSource = lstCollabDocs;
        }
    }
}
