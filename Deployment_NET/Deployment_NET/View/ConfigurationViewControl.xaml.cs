//------------------------------------------------------------------------------
// <copyright file="ConfigurationViewControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Windows.Controls;
using awinta.Deployment_NET.Business.ViewModel;

namespace awinta.Deployment_NET.View
{
    /// <summary>
    ///     Interaction logic for ConfigurationViewControl.
    /// </summary>
    public partial class ConfigurationViewControl : UserControl
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ConfigurationViewControl" /> class.
        /// </summary>
        public ConfigurationViewControl()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}