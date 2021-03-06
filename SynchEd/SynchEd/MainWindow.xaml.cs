﻿using System;
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

// For ClickOnce web config
using System.Deployment.Application;
using System.Web;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;

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
        private PrintDialog dlgPrint;

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

        private void SetupSpellCheckLanguageSelector()
        {
            List<LanguageSelector> lstSpellCheckLanguages = new List<LanguageSelector>();
            lstSpellCheckLanguages.Add(new LanguageSelector { IETFCode = "en-US", OriginalLanguageName = "English", EnglishNameOfLanguage = "English" });
            lstSpellCheckLanguages.Add(new LanguageSelector { IETFCode = "fr-FR", OriginalLanguageName = "Français", EnglishNameOfLanguage = "French" });
            lstSpellCheckLanguages.Add(new LanguageSelector { IETFCode = "es-ES", OriginalLanguageName = "Español", EnglishNameOfLanguage = "Spanish" });
            lstSpellCheckLanguages.Add(new LanguageSelector { IETFCode = "de-DE", OriginalLanguageName = "Deutsche", EnglishNameOfLanguage = "German" });

            // Set combo box source to our list. XAML data bindings will take care of the rest
            cbLanguage.ItemsSource = lstSpellCheckLanguages;
            cbLanguage.SelectedIndex = 0;
        }

        private void SetupUserKeyFromInstallURL()
        {
            // Setup User Key from instllation URL when we're first run
            NameValueCollection nameValueTable = new NameValueCollection();
            String strUserKey;

            try
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                {

                    string queryString = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;

                    if (String.IsNullOrEmpty(queryString) == true)
                    {
                        return;
                    }

                    nameValueTable = HttpUtility.ParseQueryString(queryString);
                    strUserKey = nameValueTable.Get("UserKey");
                    if (string.IsNullOrEmpty(strUserKey) != true)
                    {
                        Properties.Settings.Default.SynchedUserKey = strUserKey;
                        Properties.Settings.Default.Save();
                    }
                }
            }
            catch (Exception exEx)
            {
                return; // Fail silently. We'll take care of not having a user key elsewhere in the initialization
            }
        }



        public MainWindow()
        {
            // Initialize GUI
            try
            {
                // Main Window Components
                InitializeComponent();
                // Font menus
                cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
                cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };


                // Dialogs
                dlgCollabs = new CollaboratorsDialog();
                dlgSelectDoc = new SelectDocumentDialog();
                dlgSaveAs = new SaveAsDialog();
                dlgPrint = new PrintDialog();

                // Language Selector
                SetupSpellCheckLanguageSelector();
            }
            catch (Exception exEx)
            {
                MessageBox.Show("There was a problem intializing the user interface " + exEx.Message);
                AbortStartupAndExit();
            }

            try
            {
                // Initialize Model
                // Fetch information from installation
                SetupUserKeyFromInstallURL();

                // Fetch user information
                String strSynchedUser = Properties.Settings.Default.SynchedUserKey; // Get User key from application settings

                if (String.IsNullOrEmpty(strSynchedUser) == true)
                {
                    // Use message below in production
                    //MessageBox.Show("This application requires a user account on the SynchEd website. Create an account and install the application directly from your profile page.");
                    //AbortStartupAndExit();

                    // For demo purposes, allow a default user
                    MessageBox.Show("This demo is best when using a configured installation of SynchEd. You may be seing this message because you installed from Chrome. Try installing from Windows Explorer instead.");
                    strSynchedUser = "UUUZ";
                }
                // Complete model initialization
                Model = SynchedModel.GetInstance(rtbDocumentEditor.Document, strSynchedUser);

                // Set some info in status bar
                lblUser.Text = "Connected as: " + Model.UserName;


            }
            catch (Exception exEx)
            {
                MessageBox.Show("There was a problem connecting to database " + exEx.Message);
                AbortStartupAndExit();
            }

            // FIXME Remove test data and dialog loop
            SetupTestData();
            //LoopThroughDialogs();
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
                 rtbDocumentEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);

        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            double dblFontSize;

            if (Double.TryParse(cmbFontSize.Text, out dblFontSize) == true)
                rtbDocumentEditor.Selection.ApplyPropertyValue(Inline.FontSizeProperty, dblFontSize);

        }

        private void rtbDocumentEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbDocumentEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = rtbDocumentEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = rtbDocumentEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbDocumentEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp;
            temp = rtbDocumentEditor.Selection.GetPropertyValue(Inline.FontSizeProperty);
            double dblFontSize;
            if (Double.TryParse(temp.ToString(), out dblFontSize) == true)
                cmbFontSize.Text = dblFontSize.ToString();
            else
                cmbFontSize.Text = "";
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            // Need to clone document otherwise Rich Text Box text dissapears after printing
            FlowDocument fdPrintClone = SynchedModel.Clone(rtbDocumentEditor.Document);


            if (dlgPrint.ShowDialog() == true)
            {
                // We need to set a column width, otherwise printing defaults to 2 columns (half of page size)
                fdPrintClone.IsColumnWidthFlexible = false;
                fdPrintClone.PageWidth = dlgPrint.PrintableAreaWidth;
                fdPrintClone.ColumnWidth = dlgPrint.PrintableAreaWidth;

                IDocumentPaginatorSource idpSource = fdPrintClone;

                dlgPrint.PrintDocument(idpSource.DocumentPaginator, "SynchEdPrinting");
            }
        }

        private void miOpen_Click(object sender, RoutedEventArgs e)
        {
            Model.RetrieveContent();
            Model.LoadDocContent();
        }

        private void miSave_Click(object sender, RoutedEventArgs e)
        {
            Model.UpdateDocContent();
            Model.SaveContent();
        }

        private void rtbDocumentEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            Console.WriteLine("Changes:");

            

            foreach (TextChange tc in e.Changes)
            {
                MemoryStream stream = new MemoryStream();

                Console.WriteLine("Removed : " + tc.RemovedLength.ToString() + "Added : " + tc.AddedLength.ToString());
                TextRange range = new TextRange(rtbDocumentEditor.Document.ContentStart.GetPositionAtOffset(tc.Offset), rtbDocumentEditor.Document.ContentStart.GetPositionAtOffset((tc.Offset + tc.AddedLength)));
                range.Save(stream, DataFormats.Xaml, true); // Format might be Xaml
                Console.Write(Encoding.UTF8.GetString(stream.ToArray()));

                stream.Close();
            }

            Console.WriteLine("\nEnd of changes");
            return;
        }
    }
}
