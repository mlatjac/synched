using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Microsoft.XmlDiffPatch;
using System.Data.SqlClient;
using System.IO;

namespace POC1
{

    public class SynchEdDB
    {
        static string connString = @"Data Source=localhost;Initial Catalog=SynchedPOC1;User ID=sa;Password=root";
        SqlConnection conn;

        public SynchEdDB()
        {
            // Open and store connection
            conn = new SqlConnection();
            conn.ConnectionString = connString;
            conn.Open();
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
        public void InsertNewChange(string diff)
        {
            // Insert
            SqlCommand insertCommand = new SqlCommand("INSERT INTO Changes (ChangeInstructions) VALUES (@diffContent)", conn);
            insertCommand.Parameters.Add(new SqlParameter("diffContent", diff));
            insertCommand.ExecuteNonQuery();

        }
        public string GetDocChange(int Id)
        {
            string changeContent = "";
            // Select
            SqlCommand command = new SqlCommand("SELECT * FROM Changes WHERE ID=@Id", conn);
            command.Parameters.Add(new SqlParameter("Id", Id));


            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    changeContent += reader["ChangeInstructions"];
                }
            }
            return changeContent;
        }

    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer tmUpdate;
        private SynchEdDB DBSynched;

        string PreviousVersionOfContent = ""; // We'll perform the diff against this value


        private bool IsListening = true; // Boolean to indicate role of instance. Defaults to true. false means updating

        public string GenerateDiffGram(string originalversion, string changedversion)
        {
            // Make xml readers out of strings
            XmlReader srOriginalVersion = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(originalversion)));
            XmlReader srChangedVersion = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(changedversion)));

            MemoryStream msDiffOutput = new MemoryStream();
            XmlWriter diffGramWriter = XmlWriter.Create(msDiffOutput);

            string diffGram = "";

            XmlDiff xmldiff = new XmlDiff(XmlDiffOptions.IgnoreNamespaces |
                                             XmlDiffOptions.IgnorePrefixes);
            bool bIdentical = xmldiff.Compare(srOriginalVersion, srChangedVersion, diffGramWriter);
            diffGramWriter.Close();

            // Get a string from stream
            diffGram = Encoding.UTF8.GetString(msDiffOutput.ToArray());
            return diffGram;
        }

        public string Patch(string originalversion, string diffContent)
        {
            string newVersion = "";

            XmlDocument sourceDoc = new XmlDocument(new NameTable());
            XmlReader srOriginalVersion = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(originalversion)));
            XmlReader srDiffContent = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(diffContent)));

            sourceDoc.Load(srOriginalVersion);

            XmlPatch myPatcher = new XmlPatch();
            myPatcher.Patch(sourceDoc, srDiffContent);

            MemoryStream msNewVersionOutput = new MemoryStream();
            XmlWriter newVersionWriter = XmlWriter.Create(msNewVersionOutput);

            sourceDoc.Save(newVersionWriter);
            newVersionWriter.Close();

            newVersion = Encoding.UTF8.GetString(msNewVersionOutput.ToArray());

            return newVersion;
        }


        public MainWindow()
        {
            InitializeComponent();

            DBSynched = new SynchEdDB();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            /*
            // Set flag to false to indicate that we're the updating instance
            IsListening = false;

            // Setup update loop and timers
            tmUpdate = new Timer(3000); // Startwith every 3 seconds to keep things manageable
            tmUpdate.Elapsed += TmUpdate_Elapsed;
            tmUpdate.Start(); */
            TextRange range = new TextRange(rtbMainDoc.Document.ContentStart, rtbMainDoc.Document.ContentEnd);
            MemoryStream stream = new MemoryStream();
            range.Save(stream, DataFormats.Xaml); // Format might be Xaml
            string xamlText = Encoding.UTF8.GetString(stream.ToArray());
            DBSynched.SetDocXamlContent(1, xamlText);
        }

        private void TmUpdate_Elapsed(object sender, ElapsedEventArgs e)
        {
            System.Console.WriteLine("Timer invoked");
            if (IsListening == true)
            {
                GetUpdates();
            }
            else
            {
                GenerateUpdates();
            }

        }
        private void GetUpdates()
        {
            System.Console.WriteLine("Getting updates");
            return;
        }

        private void GenerateUpdates()
        {
            System.Console.WriteLine("Sending updates");
            return;
        }

        private void btnListen_Click(object sender, RoutedEventArgs e)
        {
            string XamlContent = DBSynched.GetDocXamlContent(1);

            MemoryStream stream = new MemoryStream();
            TextRange range = new TextRange(rtbMainDoc.Document.ContentStart, rtbMainDoc.Document.ContentEnd);
            stream = new MemoryStream(Encoding.UTF8.GetBytes(XamlContent));
            range.Load(stream, System.Windows.DataFormats.Xaml);

            stream.Close();

            // Also keep a copy
            range = new TextRange(rtbMainDoc.Document.ContentStart, rtbMainDoc.Document.ContentEnd);
            stream = new MemoryStream();
            range.Save(stream, DataFormats.Xaml); // Format might be Xaml
            PreviousVersionOfContent = Encoding.UTF8.GetString(stream.ToArray());
            stream.Close();


        }

        private void btnSendChange_Click(object sender, RoutedEventArgs e)
        {
            // Get content as a string
            TextRange range = new TextRange(rtbMainDoc.Document.ContentStart, rtbMainDoc.Document.ContentEnd);
            MemoryStream stream = new MemoryStream();
            range.Save(stream, DataFormats.Xaml); // Format might be Xaml
            string newVersion = Encoding.UTF8.GetString(stream.ToArray());

            string strDiff = GenerateDiffGram(PreviousVersionOfContent, newVersion);

            DBSynched.InsertNewChange(strDiff);

        }

        private void btnGetChanges_Click(object sender, RoutedEventArgs e)
        {
            string newVersion = Patch(PreviousVersionOfContent, DBSynched.GetDocChange(3));

            MemoryStream stream = new MemoryStream();
            TextRange range = new TextRange(rtbMainDoc.Document.ContentStart, rtbMainDoc.Document.ContentEnd);
            stream = new MemoryStream(Encoding.UTF8.GetBytes(newVersion));
            range.Load(stream, System.Windows.DataFormats.Xaml);
            stream.Close();

        }
    }
}
