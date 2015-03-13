using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AccurPdfCrop.WPF
{
    class MoveThumb : Thumb
    {
        public MoveThumb()
        {
            this.DragDelta += MoveThumb_DragDelta;
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Control selectRectangle = this.DataContext as Control;

            if (selectRectangle != null)
            {
                double left = Canvas.GetLeft(selectRectangle);
                double top = Canvas.GetTop(selectRectangle);

                Canvas.SetLeft(selectRectangle, left + e.HorizontalChange);
                Canvas.SetTop(selectRectangle, top + e.VerticalChange);
            }
        }
    }
}
