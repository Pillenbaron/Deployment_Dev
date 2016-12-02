//------------------------------------------------------------------------------
// <copyright file="ConfigurationViewControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace awinta.Deployment_NET.View
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ConfigurationViewControl.
    /// </summary>
    public partial class ConfigurationViewControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationViewControl"/> class.
        /// </summary>
        public ConfigurationViewControl()
        {
            this.InitializeComponent();

            this.DataContext = new ViewModel.MainViewModel();

        }
         
    }
}