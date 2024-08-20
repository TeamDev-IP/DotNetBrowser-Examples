#region Copyright

// Copyright 2024, TeamDev. All rights reserved.
// 
// Redistribution and use in source and/or binary forms, with or without
// modification, must retain the above copyright notice and the following
// disclaimer.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DotNetBrowser.Ui;

namespace Demo.Wpf.Extensions
{
    [ValueConversion(typeof(Bitmap), typeof(BitmapSource))]
    public class BitmapConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType,
                              object parameter, CultureInfo culture)
            => value == null ? null : ToBitmapSource((Bitmap)value);

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
            => throw new NotSupportedException();


        internal static BitmapSource ToBitmapSource(Bitmap bitmap)
        {
            PixelFormat pf = PixelFormats.Pbgra32;
            int width = (int)bitmap.Size.Width;
            int height = (int)bitmap.Size.Height;
            int rawStride = (width * pf.BitsPerPixel + 7) / 8;

            BitmapSource bitmapSource = BitmapSource.Create(width, height,
                                                            96, 96, pf, null,
                                                            (byte[])bitmap.Pixels, rawStride);

            return bitmapSource;
        }

        #endregion
    }
}
