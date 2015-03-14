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
            var selectRect = this.DataContext as SelectRectangle;
            var canvas = selectRect.Owner.Canvas;

            double top = Canvas.GetTop(selectRect);
            double left = Canvas.GetLeft(selectRect);
            double newTop = top + e.VerticalChange;
            double newLeft = left + e.HorizontalChange;

            if (newTop < 0)
                newTop = 0;

            if (newTop > canvas.ActualHeight - selectRect.ActualHeight)
                newTop = canvas.ActualHeight - selectRect.ActualHeight;

            if (newLeft < 0)
                newLeft = 0;

            if (newLeft > canvas.ActualWidth - selectRect.ActualWidth)
                newLeft = canvas.ActualWidth - selectRect.ActualWidth;

            if (selectRect != null)
            {
                Canvas.SetLeft(selectRect, newLeft);
                Canvas.SetTop(selectRect, newTop);
            }
        }
    }
}
