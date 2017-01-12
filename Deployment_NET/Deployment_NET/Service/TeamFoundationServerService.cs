using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework;
using Microsoft.TeamFoundation.VersionControl.Client;
using awinta.Deployment_NET.Interfaces;

namespace awinta.Deployment_NET.Service
{
    public sealed class TeamFoundationServerService : ITeamFoundationServerService
    {
        private const string teamFoundationServerUrl = "http://tfs:8080/tfs/asys";
        private VersionControlServer vcServer;
        private WorkspaceInfo workSpaceInfo;
        private Workspace workSpace;

        public Failure[] Failures { get; private set; }

        public TeamFoundationServerService()
        {

            Connect();

        }

        public void Connect()
        {

            Connect(new Uri(teamFoundationServerUrl));

        }

        public void Connect(string path)
        {

            Connect(new Uri(path));

        }

        public void Connect(Uri path)
        {

            // Connect to the team project collection and the server that hosts the version-control repository. 
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(path);
            vcServer = tpc.GetService<VersionControlServer>();

            workSpaceInfo = Workstation.Current.GetLocalWorkspaceInfo(vcServer, Environment.MachineName, vcServer.AuthorizedUser);
            workSpace = vcServer.GetWorkspace(workSpaceInfo);

        }

        public void UpdateProject(string path)
        {

            GetStatus status = workSpace.Get(new string[] { path }, VersionSpec.Latest, RecursionType.Full, GetOptions.GetAll | GetOptions.Overwrite);

           if (status.NoActionNeeded) { Service.OutputService.WriteOutput($"<TFS>No Actions needed on: {path}"); }

            if (status.NumFailures > 0)
            {
                Failures = status.GetFailures();
            }
        }

        public void getFileListOutput()
        {

            ItemSet items = vcServer.GetItems("$/", RecursionType.Full);
            foreach (Item item in items.Items)
            {
                Service.OutputService.WriteOutput(item.ServerItem.ToString());
            }

        }

    }
}
