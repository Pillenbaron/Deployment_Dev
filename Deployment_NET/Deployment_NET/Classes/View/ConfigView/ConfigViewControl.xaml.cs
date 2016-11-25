//------------------------------------------------------------------------------
// <copyright file="ConfigViewControl.xaml.cs" company="awinta GmbH">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace awinta.Deployment_NET.View
{
    using EnvDTE;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ConfigViewControl.
    /// </summary>
    public partial class ConfigViewControl : UserControl
    {

        private ViewModel.MainViewModel viewModel;

        public DTE Service
        {
            get { return viewModel.Service; }
            set { viewModel.Service = value; }
        }  

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigViewControl"/> class.
        /// </summary>
        public ConfigViewControl()
        {
            this.InitializeComponent();
            viewModel = new ViewModel.MainViewModel();
            this.DataContext = viewModel;

        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "ConfigView");
        }
    }
}