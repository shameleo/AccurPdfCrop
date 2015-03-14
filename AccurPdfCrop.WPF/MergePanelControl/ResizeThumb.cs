using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace AccurPdfCrop.WPF
{
    class ResizeThumb : Thumb
    {
        public ResizeThumb()
        {
            this.DragDelta += this.ResizeThumb_DragDelta;
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var selectRect = this.DataContext as SelectRectangle;
            var canvas = selectRect.Owner.Canvas;

            if (selectRect != null)
            {
                double newHeight;
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        newHeight = selectRect.ActualHeight - e.VerticalChange;
                        if (newHeight < selectRect.MinHeight)
                           newHeight = selectRect.MinHeight;
                        if (newHeight > Canvas.GetTop(selectRect) + selectRect.ActualHeight)
                            newHeight = Canvas.GetTop(selectRect) + selectRect.ActualHeight;
                        var newTop = Canvas.GetTop(selectRect) + selectRect.ActualHeight - newHeight;
                        Canvas.SetTop(selectRect, newTop);
                        selectRect.Height = newHeight;
                        break;
                    case VerticalAlignment.Bottom:
                        newHeight = selectRect.ActualHeight + e.VerticalChange;
                        if (newHeight < selectRect.MinHeight)
                            newHeight = selectRect.MinHeight;
                        if (newHeight > canvas.ActualHeight - Canvas.GetTop(selectRect))
                            newHeight = canvas.ActualHeight - Canvas.GetTop(selectRect);
                        selectRect.Height = newHeight;
                        break;
                    default:
                        break;
                }

                double newWidth;
                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        newWidth = selectRect.ActualWidth - e.HorizontalChange;
                        if (newWidth < selectRect.MinWidth)
                            newWidth = selectRect.MinWidth;
                        if (newWidth > Canvas.GetLeft(selectRect) + selectRect.ActualWidth)
                            newWidth = Canvas.GetLeft(selectRect) + selectRect.ActualWidth;
                        var newLeft = Canvas.GetLeft(selectRect) + selectRect.ActualWidth - newWidth;
                        Canvas.SetLeft(selectRect, newLeft);
                        selectRect.Width = newWidth;
                        break;
                    case HorizontalAlignment.Right:
                        newWidth = selectRect.ActualWidth + e.HorizontalChange;
                        if (newWidth < selectRect.MinWidth)
                            newWidth = selectRect.MinWidth;
                        if (newWidth > canvas.ActualWidth - Canvas.GetLeft(selectRect))
                            newWidth = canvas.ActualWidth - Canvas.GetLeft(selectRect);
                        selectRect.Width = newWidth;
                        break;
                    default:
                        break;
                }
            }

            e.Handled = true;
        }
    }
}
