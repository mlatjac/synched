using System;
using System.Collections;
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

namespace CursorSavePOC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSaveToCode_Click(object sender, RoutedEventArgs e)
        {
            
            // Try to insert span element at selection start and selection end
            TextPointer tpSelectionStart = rtbRichTextBox1.Selection.Start;

            Span spStartMarker = new Span(tpSelectionStart, tpSelectionStart);

            spStartMarker.Inlines.Add("SelectionStart");
            
            /*spStartMarker.Name = "SelectionStart"; // Name it so we can find it in code -> Name isn't exported
            spStartMarker.Tag = "SelectionStart"; // Tag isn't exported*/
            

            /*
            TextPointer tpSelectionEnd = rtbRichTextBox1.Selection.End;
            Span spEndMarker = new Span(tpSelectionEnd, tpSelectionEnd); */

            TextRange range = new TextRange(rtbRichTextBox1.Document.ContentStart, rtbRichTextBox1.Document.ContentEnd);
            MemoryStream stream = new MemoryStream();
            range.Save(stream, DataFormats.Xaml); // Format might be Xaml
            string xamlText = Encoding.UTF8.GetString(stream.ToArray());
            tbMarkup.Text = xamlText;

            IEnumerable children = LogicalTreeHelper.GetChildren(rtbRichTextBox1.Document);

        }

        private void btnSaveFromCode_Click(object sender, RoutedEventArgs e)
        {
            MemoryStream stream = new MemoryStream();
            TextRange range = new TextRange(rtbRichTextBox2.Document.ContentStart, rtbRichTextBox2.Document.ContentEnd);
            stream = new MemoryStream(Encoding.UTF8.GetBytes(tbMarkup.Text));
            range.Load(stream, System.Windows.DataFormats.Xaml);
            stream.Close();

            // Find the span element
            range = new TextRange(rtbRichTextBox2.Document.ContentStart, rtbRichTextBox2.Document.ContentEnd);
            IEnumerable children = LogicalTreeHelper.GetChildren(rtbRichTextBox2.Document);
        }
    }
}
