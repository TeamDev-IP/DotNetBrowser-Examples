using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using drawing = System.Drawing;

namespace Demo.WPF
{
    public class ImageButton : Label
    {
        public static readonly RoutedEvent OnClick;

        public drawing.Bitmap Icon
        {
            set
            {
                Image grayIcon;
                Image sourceIcon = CreateIcon(value, out grayIcon);
                this.Content = sourceIcon;
                this.MouseLeave += delegate
                {
                    this.Content = sourceIcon;
                };

                this.IsEnabledChanged += delegate
                {
                    Application.Current.Dispatcher.BeginInvoke(
                         DispatcherPriority.Input,
                         (ThreadStart)delegate
                         {
                             if (this.IsEnabled)
                                 this.Content = sourceIcon;
                             else
                             {
                                 this.Content = grayIcon;
                             }
                         });
                };
            }
        }

        public drawing.Bitmap RolloverIcon
        {
            set
            {
                Image sourceRolloverIcon = CreateIcon(value);
                this.MouseEnter += delegate
                {
                    this.Content = sourceRolloverIcon;
                };
            }
        }

        public drawing.Bitmap PressedIcon 
        {
            set
            {
                Image sourcePressedIcon = CreateIcon(value);
                this.Click += delegate
                {
                    this.Content = sourcePressedIcon;
                };
            }
        }

        public drawing.Bitmap DisableIcon
        {
            set
            {
                Image icon = CreateIcon(value);
            }
        }

        static ImageButton()
        {
            OnClick = ButtonBase.ClickEvent.AddOwner(typeof(ImageButton));
        }

        public ImageButton()
            : base()
        {
            this.Margin = new Thickness(0);
            ToolTipService.SetShowOnDisabled(this, true);
        }



        public event RoutedEventHandler Click
        {
            add { AddHandler(OnClick, value); }
            remove { RemoveHandler(OnClick, value); }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            CaptureMouse();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (IsMouseCaptured)
            {
                ReleaseMouseCapture();

                if (IsMouseOver)
                    RaiseEvent(new RoutedEventArgs(OnClick, this));
            }
        }

        private static Image CreateIcon(drawing.Bitmap image)
        {
            var icon = new Image();

            icon.Source = GetIconSource(image);

            return icon;
        }


        private static Image CreateIcon(drawing.Bitmap image, out Image grayIcon)
        {
            var icon = new Image();

            var iconSource = GetIconSource(image);
            icon.Source = iconSource;

            grayIcon = new Image();
            FormatConvertedBitmap grayBitmap = new FormatConvertedBitmap();
            grayBitmap.BeginInit();
            grayBitmap.Source = iconSource;
            grayBitmap.DestinationFormat = PixelFormats.Gray8;
            grayIcon.OpacityMask = new ImageBrush(iconSource); 
            grayBitmap.EndInit();
            // Set Source property of Image

            grayIcon.Source = grayBitmap;

            return icon;
        }

        private static BitmapImage GetIconSource(drawing.Bitmap image)
        {
            // ImageSource ...
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();

            // Save to a memory stream...
            image.Save(ms, drawing.Imaging.ImageFormat.Png);

            // Rewind the stream...
            ms.Seek(0, SeekOrigin.Begin);

            // Tell the WPF image to use this stream...
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }

    }
}
