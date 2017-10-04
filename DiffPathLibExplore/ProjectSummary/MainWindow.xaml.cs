using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.XmlDiffPatch;
using System.Xml;

namespace ProjectSummary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            callDiff();
            callPatch();
        }

        private void rtbMain_TextChanged(object sender, TextChangedEventArgs e)
        {

            //rtbMain.BeginChange();
            //tbXAMLPreview.Text = rtbMain.Document.
            TextRange range = new TextRange(rtbMain.Document.ContentStart, rtbMain.Document.ContentEnd);
            MemoryStream stream = new MemoryStream();
            range.Save(stream, DataFormats.Xaml); // Format might be Xaml
            string xamlText = Encoding.UTF8.GetString(stream.ToArray());
            tbXAMLPreview.Text = xamlText;
        }

        public void GenerateDiffGram(string originalFile, string finalFile,
                                    XmlWriter diffGramWriter)
        {
            XmlDiff xmldiff = new XmlDiff(XmlDiffOptions.IgnoreNamespaces |
                                             XmlDiffOptions.IgnorePrefixes);
            bool bIdentical = xmldiff.Compare(originalFile, finalFile, false, diffGramWriter);
            diffGramWriter.Close();
        }

        public void callDiff()
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Encoding = new UTF8Encoding(false, false);

            XmlWriter dwDiffs1 = XmlWriter.Create(@"C:\Users\8547102\Desktop\diff1.xml");
            GenerateDiffGram(@"C:\Users\8547102\Desktop\startingtext.txt", @"C:\Users\8547102\Desktop\firstparachanges.txt", dwDiffs1);

            XmlWriter dwDiffs2 = XmlWriter.Create(@"C:\Users\8547102\Desktop\diff2.xml");
            GenerateDiffGram(@"C:\Users\8547102\Desktop\startingtext.txt", @"C:\Users\8547102\Desktop\secondparachanges.txt", dwDiffs2);
        }

        private void btnLoadFromXML_Click(object sender, RoutedEventArgs e)
        {

            MemoryStream stream = new MemoryStream();
            TextRange range = new TextRange(rtbMain.Document.ContentStart, rtbMain.Document.ContentEnd);
            stream = new MemoryStream(Encoding.UTF8.GetBytes(tbXAMLPreview.Text));
            range.Load(stream, System.Windows.DataFormats.Xaml);
            stream.Close();

        }
        public void callPatch()
        {
            //PatchUp(@"C:\Users\8547102\Desktop\startingtext.txt", @"C:\Users\8547102\Desktop\diff1.xml", @"C:\Users\8547102\Desktop\patched1.xml");
            //PatchUp(@"C:\Users\8547102\Desktop\startingtext.txt", @"C:\Users\8547102\Desktop\diff2.xml", @"C:\Users\8547102\Desktop\patched2.xml");
            PatchUp(@"C:\Users\8547102\Desktop\startingtext.txt", @"C:\Users\8547102\Desktop\diffcombo.txt", @"C:\Users\8547102\Desktop\patchedcombo.xml");


            /* Order in which patches are applied is important 
            PatchUp(@"C:\Users\8547102\Desktop\startingtext.txt", @"C:\Users\8547102\Desktop\diff2.xml", @"C:\Users\8547102\Desktop\patched3.xml");
            PatchUp(@"C:\Users\8547102\Desktop\patched3.xml", @"C:\Users\8547102\Desktop\diff1.xml", @"C:\Users\8547102\Desktop\patched4.xml"); */

        }
        public static void PatchUp(string originalFile, String diffGramFile, String OutputFile)
        {
            XmlDocument sourceDoc = new XmlDocument(new NameTable());
            sourceDoc.Load(originalFile);

            XmlTextReader diffgramReader = new XmlTextReader(diffGramFile);

            XmlPatch myPatcher = new XmlPatch();
            myPatcher.Patch(sourceDoc, diffgramReader); /* If diff is from a different source the following exception is triggered:
            System.Exception: 'The XDL diffgram is not applicable to this XML document; the srcDocHash value does not match.'
            */

            XmlTextWriter output = new XmlTextWriter(OutputFile, Encoding.UTF8);
            sourceDoc.Save(output);
            output.Close();
        }
    }
}