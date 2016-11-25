//------------------------------------------------------------------------------
// <copyright file="ConfigView.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace awinta.Deployment_NET.View
{
    using Microsoft.VisualStudio.Shell;
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("ac75cb62-5e37-4769-82a4-0fa0289cf2af")]
    public class ConfigView : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigView"/> class.
        /// </summary>
        public ConfigView() : base(null)
        {
            this.Caption = "ConfigView";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new ConfigViewControl();
        }

        public void setService(EnvDTE.DTE Service)
        {

            var Control = this.Content as ConfigViewControl;

            if (Control != null)
            {

                Control.Service = Service;

            }

        }

    }
}
