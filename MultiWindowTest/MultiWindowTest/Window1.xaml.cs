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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CollaboratorsDialog : Window
    {
        public CollaboratorsDialog()
        {
            InitializeComponent();

            // Add data to test list view
            List<SynchedUserAccess> lstUserAccess = new List<SynchedUserAccess>();

            lstUserAccess.Add(new SynchedUserAccess { Name = "Frank", CanWrite = true });
            lstUserAccess.Add(new SynchedUserAccess { Name = "Mary", CanWrite = true });
            lstUserAccess.Add(new SynchedUserAccess { Name = "Barry", CanWrite = false });

            lvCollaborators.ItemsSource = lstUserAccess;

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
            lvUsers.ItemsSource = lstAllUsers;
        }
    }
}
