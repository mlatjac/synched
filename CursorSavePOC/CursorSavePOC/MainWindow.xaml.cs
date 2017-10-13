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

using System.Reflection;
using System.Globalization;
using System.Windows.Documents;

namespace CursorSavePOC
{
    [Flags]
    public enum FindFlags
    {
        FindInReverse = 2,
        FindWholeWordsOnly = 4,
        MatchAlefHamza = 0x20,
        MatchCase = 1,
        MatchDiacritics = 8,
        MatchKashida = 0x10,
        None = 0
    }

    public static class DocumentHelper
    {
        private static MethodInfo findMethod = null;

        public static TextRange FindText(TextPointer findContainerStartPosition, TextPointer findContainerEndPosition, String input, FindFlags flags, CultureInfo cultureInfo)
        {
            TextRange textRange = null;
            if (findContainerStartPosition.CompareTo(findContainerEndPosition) < 0)
            {
                try
                {
                    if (findMethod == null)
                    {
                        findMethod = typeof(FrameworkElement).Assembly.GetType("System.Windows.Documents.TextFindEngine").
                               GetMethod("Find", BindingFlags.Static | BindingFlags.Public);
                    }
                    Object result = findMethod.Invoke(null, new Object[] { findContainerStartPosition,
                    findContainerEndPosition,
                    input, flags, CultureInfo.CurrentCulture });
                    textRange = result as TextRange;
                }
                catch (ApplicationException)
                {
                    textRange = null;
                }
            }

            return textRange;
        }
    }

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
            TextRange trStartMarker;

            trStartMarker = new TextRange(tpSelectionStart, tpSelectionStart);
            trStartMarker.Text = "StartMarker";

            /*
            Span spStartMarker = new Span(tpSelectionStart, tpSelectionStart);

            spStartMarker.Inlines.Add("SelectionStart");
            
            spStartMarker.Name = "SelectionStart"; // Name it so we can find it in code -> Name isn't exported
            spStartMarker.Tag = "SelectionStart"; // Tag isn't exported*/

           
            TextPointer tpSelectionEnd = rtbRichTextBox1.Selection.End;
            TextRange trEndMarker;

            trEndMarker = new TextRange(tpSelectionEnd, tpSelectionEnd);
            trEndMarker.Text = "EndMarker";


            //Span spEndMarker = new Span(tpSelectionEnd, tpSelectionEnd); 

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

            TextRange trFindStartMarkerRange = DocumentHelper.FindText(rtbRichTextBox2.Document.ContentStart, rtbRichTextBox2.Document.ContentEnd, "StartMarker", FindFlags.None, CultureInfo.CurrentCulture);
            TextRange trFindEndMarkerRange = DocumentHelper.FindText(rtbRichTextBox2.Document.ContentStart, rtbRichTextBox2.Document.ContentEnd, "EndMarker", FindFlags.None, CultureInfo.CurrentCulture);


            trFindStartMarkerRange.Text = ""; // Remove marker
            trFindEndMarkerRange.Text = "";

            rtbRichTextBox2.Selection.Select(trFindStartMarkerRange.Start, trFindEndMarkerRange.Start);

            rtbRichTextBox2.Focus();

            // Find the span element
            range = new TextRange(rtbRichTextBox2.Document.ContentStart, rtbRichTextBox2.Document.ContentEnd);
            IEnumerable children = LogicalTreeHelper.GetChildren(rtbRichTextBox2.Document);
            
        }
    }
}
