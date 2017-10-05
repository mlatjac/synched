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
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class wSaveAsDialog : Window
    {
        public wSaveAsDialog()
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

        }
    }
}
