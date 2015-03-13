using System;
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
            Control selectRectangle = this.DataContext as Control;

            if (selectRectangle != null)
            {
                double deltaVertical, deltaHorizontal;

                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange, selectRectangle.ActualHeight - selectRectangle.MinHeight);
                        selectRectangle.Height = selectRectangle.ActualHeight - deltaVertical;
                        break;
                    case VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange, selectRectangle.ActualHeight - selectRectangle.MinHeight);
                        Canvas.SetTop(selectRectangle, Canvas.GetTop(selectRectangle) + deltaVertical);
                        selectRectangle.Height = selectRectangle.ActualHeight - deltaVertical;
                        break;
                    default:
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange, selectRectangle.ActualWidth - selectRectangle.MinWidth);
                        Canvas.SetLeft(selectRectangle, Canvas.GetLeft(selectRectangle) + deltaHorizontal);
                        selectRectangle.Width = selectRectangle.ActualWidth - deltaHorizontal;
                        break;
                    case HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange, selectRectangle.ActualWidth - selectRectangle.MinWidth);
                        selectRectangle.Width = selectRectangle.ActualWidth - deltaHorizontal;
                        break;
                    default:
                        break;
                }
            }

            e.Handled = true;
        }
    }
}
