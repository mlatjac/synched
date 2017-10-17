using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace POCFullUpdateCycle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static FlowDocument fdCurrentOrigDocContent = new FlowDocument();
        public static Timer tTimer;

        public static RichTextBox rtb1;
        public static RichTextBox rtb2;

        public static bool bRtb1IsBlocked = false;
        

        public static void TimerCallBack(Object o)
        {
            tTimer.Dispose();
            rtb1.BeginChange();
            CopyDoc(rtb1.Document, fdCurrentOrigDocContent);
        }

        public static void Step1()
        {
            rtb1.IsReadOnly = true;
            //rtb1.BeginChange();
            //CopyDoc(rtb1.Document, fdCurrentOrigDocContent);
            bRtb1IsBlocked = true;

        }

        public static void Step2()
        {
            //CopyDoc(fdCurrentOrigDocContent, rtb1.Document);

            rtb1.IsReadOnly = false;
            bRtb1IsBlocked = false;

        }

        public static void TimerCallBack2(Object o)
        {
            if (bRtb1IsBlocked == false)
            {
                rtb1.Dispatcher.Invoke(() =>
                {
                    Step1();
                });

            }
            else
            {
                rtb1.Dispatcher.Invoke(() =>
                {
                    Step2();
                });

            }
        }


        void Setup()
    {
            // Block changes with begin change
            //rtbClient1.BeginChange();
            //rtbClient2.BeginChange();
            rtbClient1.IsReadOnly = true;
            rtbClient2.IsReadOnly = false;
    }

        static void CopyDoc(FlowDocument src, FlowDocument dest)
        {
            TextRange trOriginal = new TextRange(src.ContentStart, src.ContentEnd);
            MemoryStream stream = new MemoryStream();
            trOriginal.Save(stream, DataFormats.Xaml); // Format might be Xaml

            TextRange trRichTextBox = new TextRange(dest.ContentStart, dest.ContentEnd);
            trRichTextBox.Load(stream, System.Windows.DataFormats.Xaml);
            stream.Close();

        }

        void UpdateCycleWithWriteAccess(RichTextBox rtb)
    {
            // Changes are blocked from setup or previous cycle

            // Attempt to get write access. It succeeds !

            // Update the document from the DB if not yet up to date
            CopyDoc(fdCurrentOrigDocContent, rtb.Document);

            // EndChnage block -- Allow GUI to make changes to RTB (Some changes may have been cache)
            // rtb.EndChange();
            rtb.IsReadOnly = false;

            // After a set time Block changes. Do we even need a timer here?
            //Thread.Sleep(1000);
            tTimer = new Timer(TimerCallBack, rtb1, 0, 50);
            // rtb.BeginChange(); // Begin will occur in timer task

            // Get a copy of the document and send it to DB
            //CopyDoc(rtb.Document, fdCurrentOrigDocContent);

        }

        void UpdateCycleWithoutWriteAccess(RichTextBox rtb)
        {
            // Changes are blocked from setup of previous cycle

            // Attempt to get write access. It fails !

            // Update the document from the DB
            CopyDoc(fdCurrentOrigDocContent, rtb.Document);

            // Try write access again

        }

        public MainWindow()
        {
            InitializeComponent();

            rtb1 = rtbClient1;
            //rtb2 = rtbClient2;

            //tTimer = new Timer(TimerCallBack2, null, 100, 100);


            //Setup();

            //do
            //{
            //UpdateCycleWithoutWriteAccess(rtbClient1);
                //UpdateCycleWithoutWriteAccess(rtbClient2);
                //UpdateCycleWithWriteAccess(rtbClient1);
            //rtbClient1.EndChange();
                //UpdateCycleWithoutWriteAccess(rtbClient2);
                //UpdateCycleWithWriteAccess(rtbClient2);
                //Thread.Sleep(1000);

            //} while (true); 
        }

        private void rtbClient1_TextChanged(object sender, TextChangedEventArgs e)
        {
           Console.WriteLine("Changes:");
            MemoryStream stream = new MemoryStream();


            foreach ( TextChange tc in e.Changes)
            {
                TextRange range = new TextRange(rtbClient1.Document.ContentStart.GetPositionAtOffset(tc.Offset), rtbClient1.Document.ContentStart.GetPositionAtOffset((tc.Offset+tc.AddedLength)));
                range.Save(stream, DataFormats.Xaml); // Format might be Xaml

                TextRange range2 = new TextRange(rtbClient2.Document.ContentStart.GetPositionAtOffset(tc.Offset), rtbClient2.Document.ContentStart.GetPositionAtOffset(tc.Offset));

                range2.Load(stream, DataFormats.Xaml);
            }

            //TextRangerange range = new TextRange(rtb1.Document.ContentStart.GetPositionAtOffset(e.Changes.FirstOrDefault<TextChange>., rtbMainDoc.Document.ContentEnd);
            //range.Save(Console.Out, DataFormats.Xaml); // Format might be Xaml
            Console.WriteLine("\nEnd of changes");
            return;

          
        }
    }
}
