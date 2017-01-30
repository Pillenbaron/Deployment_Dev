﻿using System;
using awinta.Deployment_NET.IoC;
using Microsoft.VisualStudio.Shell.Interop;

namespace awinta.Deployment_NET.Service
{
    public static class OutputService
    {

        //private void MenuItemCallback(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        WriteOutput(VSConstants.OutputWindowPaneGuid.DebugPane_guid, "Hello World in Debug pane");
        //        WriteOutput(VSConstants.OutputWindowPaneGuid.BuildOutputPane_guid, "Hello World in Build pane");
        //        WriteOutput(VSConstants.OutputWindowPaneGuid.GeneralPane_guid, "Hello World in General pane");
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ex.ToString());
        //    }
        //}

        public static void ClearOutput()
        {
            var service = ApplicationContainer.GetInstance<IVsOutputWindowPane>();

            service.Clear();
        }

        public static void WriteOutput(string text)
        {
            var service = ApplicationContainer.GetInstance<IVsOutputWindowPane>();

            if (service == null) return;
            service.Activate();
            service.OutputStringThreadSafe(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ": " + text +
                                           Environment.NewLine);
        }
    }
}