﻿//------------------------------------------------------------------------------
// <copyright file="TuneupCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Interop;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace VisualStudioTemplates
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class TuneupCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("3a5080ee-dc83-4781-a134-01111bc4a637");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Package package;

        /// <summary>
        /// Initializes a new instance of the <see cref="TuneupCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private TuneupCommand(Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static TuneupCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new TuneupCommand(package);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            DTE2 dte = (DTE2)ServiceProvider.GetService(typeof(DTE));

            var hwnd = new IntPtr(dte.MainWindow.HWnd);
            var window = (System.Windows.Window) HwndSource.FromHwnd(hwnd).RootVisual;

            string msg =
                "The following settings are going to be changed:\n\nAutomatically Populate Toolbox will be set to False\nWarn if no user code on launch will be unchecked\nEnable Edit and Continue will be unchecked\n\nWould you like to continue?";
            var answer = System.Windows.MessageBox.Show(window, msg, "Visual Studio Tuneup", MessageBoxButton.YesNo,MessageBoxImage.Question);

            if (answer == MessageBoxResult.Yes)
            {
                dte.Properties["WindowsFormsDesigner", "General"].Item("AutoToolboxPopulate").Value = false;
                dte.Properties["Debugging", "General"].Item("WarnIfNoUserCode").Value = false;
                dte.Properties["Debugging", "EditAndContinue"].Item("EnableEditAndContinue").Value = false;
            }


            //var p = vs.Properties["Debugging History", "General"];

            //var sb = new StringBuilder();
            //for (int i = 1; i <= p.Count; i++)
            //{
            //    sb.AppendLine(i + "-" + p.Item(i).Name);
            //}

            //MessageBox.Show(sb.ToString());

        }
    }
}
