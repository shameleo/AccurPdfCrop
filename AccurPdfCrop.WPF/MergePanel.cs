﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AccurPdfCrop.WPF
{
    class MouseDragHandler
    {
        public MouseDragHandler(Point pointStarted)
        {
            this.PointStarted = pointStarted;
        }

        public Point PointStarted { get; private set; }
        public ContentControl SelectRectangle { get; set; }
    }

    class MergePanel : ContentControl
    {
        private MouseDragHandler DragHandler;

        public MergePanel()
        {
            this.MouseLeftButtonDown += MergePanel_MouseLeftButtonDown;
            this.MouseMove += MergePanel_MouseMove;
            this.MouseLeftButtonUp += MergePanel_MouseLeftButtonUp;
            this.MinHeight = 100;
            this.MinWidth = 100;
        }

        public SelectRectangle CreateSelectRectangle(double left, double top, double width, double height)
        {
            var selectRect = new SelectRectangle(this);
            selectRect.Width = width;
            selectRect.Height = height;
            Canvas.SetTop(selectRect, top);
            Canvas.SetLeft(selectRect, left);
            (Content as Canvas).Children.Add(selectRect);

            return selectRect;
        }

        public override void EndInit()
        {
            base.EndInit();

            if(Content == null)
            {
                Content = new Canvas();
            }
            else
            {
                if ((Content as Canvas) == null)
                    throw new Exception("MergePanel Content property must contain Canvas");
            }
        }

        private void MergePanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragHandler = new MouseDragHandler(e.GetPosition(sender as IInputElement));
        }

        private void MergePanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (DragHandler == null)
                return;

            var canvas = sender as Canvas;
            var currPos = e.GetPosition(sender as IInputElement);

            if(DragHandler.SelectRectangle == null)
            {
                if(Math.Abs(DragHandler.PointStarted.X - currPos.X) >= SelectRectangle.DefaultMinWidth &&
                   Math.Abs(DragHandler.PointStarted.Y - currPos.Y) >= SelectRectangle.DefaultMinHeight)
                {
                    DragHandler.SelectRectangle = CreateSelectRectangle(
                            Math.Min(DragHandler.PointStarted.X, currPos.X),
                            Math.Min(DragHandler.PointStarted.Y, currPos.Y),
                            Math.Abs(DragHandler.PointStarted.X - currPos.X),
                            Math.Abs(DragHandler.PointStarted.Y - currPos.Y)); 
                }
            }
            else
            {
                var rect = DragHandler.SelectRectangle;
                Canvas.SetLeft(rect, Math.Min(DragHandler.PointStarted.X, currPos.X));
                Canvas.SetTop(rect, Math.Min(DragHandler.PointStarted.Y, currPos.Y));
                rect.Width = Math.Abs(DragHandler.PointStarted.X - currPos.X);
                rect.Height = Math.Abs(DragHandler.PointStarted.Y - currPos.Y);
            }
        }

        private void MergePanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DragHandler = null;
        }
    }
}
