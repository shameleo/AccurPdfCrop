using System.Windows;
using System.Windows.Controls;

namespace AccurPdfCrop.WPF
{
    class SelectRectangle : ContentControl
    {
        public MergePanel Owner { get; private set; }
        internal const double DefaultMinHeight = 50;
        internal const double DefaultMinWidth = 50;
        private const string TemplateKey = "SelectorTemplate";


        public SelectRectangle(MergePanel owner)
        {
            this.Owner = owner;
            this.Template = Application.Current.FindResource("SelectorTemplate") as ControlTemplate;
            this.MinHeight = DefaultMinHeight;
            this.MinWidth = DefaultMinWidth;
        }
    }
}
