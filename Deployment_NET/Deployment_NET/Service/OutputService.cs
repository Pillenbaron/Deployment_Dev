using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

namespace awinta.Deployment_NET.Service
{

    public class OutputService
    {

        private IVsOutputWindow OutputWindow;

        public OutputService(IVsOutputWindow vsOutput)
        {

            OutputWindow= vsOutput
            Guid generalPaneGuid = VSConstants.GUID_OutWindowGeneralPane;

        }
        
        private void MenuItemCallback(object sender, EventArgs e)
        {
            try
            {
                WriteOutput(Microsoft.VisualStudio.VSConstants.OutputWindowPaneGuid.DebugPane_guid, "Hello World in Debug pane");
                WriteOutput(Microsoft.VisualStudio.VSConstants.OutputWindowPaneGuid.BuildOutputPane_guid, "Hello World in Build pane");
                WriteOutput(Microsoft.VisualStudio.VSConstants.OutputWindowPaneGuid.GeneralPane_guid, "Hello World in General pane");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }

        private void WriteOutput(Guid guidPane, string text)
        {
            const int VISIBLE = 1;
            const int DO_NOT_CLEAR_WITH_SOLUTION = 0;

            
            IVsOutputWindowPane outputWindowPane = null;
            int hr;

            // Get the output window
            outputWindow = base.GetService(typeof(SVsOutputWindow)) as IVsOutputWindow;

            // The General pane is not created by default. We must force its creation
            if (guidPane == Microsoft.VisualStudio.VSConstants.OutputWindowPaneGuid.GeneralPane_guid)
            {
                hr = outputWindow.CreatePane(guidPane, "General", VISIBLE, DO_NOT_CLEAR_WITH_SOLUTION);
                Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(hr);
            }

            // Get the pane
            hr = outputWindow.GetPane(guidPane, out outputWindowPane);
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(hr);

            // Output the text
            if (outputWindowPane != null)
            {
                outputWindowPane.Activate();
                outputWindowPane.OutputString(text);
            }


        }
    }
}
