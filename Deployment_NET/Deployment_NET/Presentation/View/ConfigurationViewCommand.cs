﻿//------------------------------------------------------------------------------
// <copyright file="ConfigurationViewCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using awinta.Deployment_NET.Common;
using awinta.Deployment_NET.Common.Interfaces;
using awinta.Deployment_NET.Common.Service;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace awinta.Deployment_NET.Presentation.View
{
    /// <summary>
    ///     Command handler
    /// </summary>
    internal sealed class ConfigurationViewCommand
    {
        /// <summary>
        ///     Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        ///     Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("1e437a60-a91c-4782-ab86-0b3b59a91ac0");

        /// <summary>
        ///     VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConfigurationViewCommand" /> class.
        ///     Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private ConfigurationViewCommand(Package package)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));

            this.package = package;

            ApplicationProvider.Register<DTE, DTE>((DTE) ServiceProvider.GetService(typeof(DTE)));
            ApplicationProvider.Register<IVsOutputWindowPane, IVsOutputWindowPane>(
                (IVsOutputWindowPane) ServiceProvider.GetService(typeof(SVsGeneralOutputWindowPane)));
            ApplicationProvider.Register<ITeamFoundationServerService, TeamFoundationServerService>();
            ApplicationProvider.Register<IVsStatusbar, IVsStatusbar>(
                (IVsStatusbar) ServiceProvider.GetService(typeof(SVsStatusbar)));

            var commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService != null)
            {
                var menuCommandId = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(ShowToolWindow, menuCommandId);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        ///     Gets the instance of the command.
        /// </summary>
        public static ConfigurationViewCommand Instance { get; private set; }

        /// <summary>
        ///     Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider => package;

        /// <summary>
        ///     Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new ConfigurationViewCommand(package);
        }

        /// <summary>
        ///     Shows the tool window when the menu item is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            var window = package.FindToolWindow(typeof(ConfigurationView), 0, true);
            if (window?.Frame == null)
                throw new NotSupportedException("Cannot create tool window");

            var windowFrame = (IVsWindowFrame) window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }
    }
}