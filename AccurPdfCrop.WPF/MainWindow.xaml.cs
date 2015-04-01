using AccurPdfCrop.GUISpecific;
using Microsoft.Win32;
using System.Windows;

namespace AccurPdfCrop.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GUIWorkflow workflow;

        public MainWindow()
        {
            InitializeComponent();
            workflow = new GUIWorkflow();
        }

        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            var diallog = new OpenFileDialog();
            diallog.Filter = "PDF documents (*.pdf)|*.pdf|All Files (*.*)|*.*";

            if(diallog.ShowDialog() == true)
            {
                workflow.LoadDocument(diallog.FileName);
            }

            workflow.CreateMerges();
        }
    }
}
