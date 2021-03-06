﻿using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace awinta.Deployment_NET.Presentation.Converter
{
    internal class UriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var uri = value as Uri;

            return uri != null ? uri.LocalPath : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;
            Uri deployPath;

            if (string.IsNullOrWhiteSpace(path) && Uri.TryCreate(path, UriKind.Absolute, out deployPath) &&
                Directory.Exists(deployPath.LocalPath))
                return deployPath;
            return null;
        }
    }
}