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

        public MainWindow()
        {
            InitializeComponent();

            // Initialize Dialogs
            dlgCollabs = new CollaboratorsDialog();
            dlgSelectDoc = new SelectDocumentDialog();
            dlgSaveAs = new SaveAsDialog();


            // FIXME: Remove loop through dialogs
            dlgCollabs.ShowDialog();
            dlgSelectDoc.ShowDialog();
            dlgSaveAs.ShowDialog();

        }
    }
}
