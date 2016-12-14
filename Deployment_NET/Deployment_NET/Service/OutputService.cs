using Microsoft.VisualStudio.Shell.Interop;

namespace awinta.Deployment_NET.Service
{

    public sealed class OutputService
    {

        private OutputService() { }

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
            IVsOutputWindowPane Service = IoC.ApplicationContainer.GetInstance<IVsOutputWindowPane>();

            Service.Clear();

        }


        public static void WriteOutput(string text)
        {

            IVsOutputWindowPane Service = IoC.ApplicationContainer.GetInstance<IVsOutputWindowPane>();

            if (Service != null)
            {

                Service.Activate();
                Service.OutputString(text + System.Environment.NewLine);
                
            }

        }

    }
}
