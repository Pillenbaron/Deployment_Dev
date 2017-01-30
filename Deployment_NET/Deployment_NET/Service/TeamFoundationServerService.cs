using System;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using awinta.Deployment_NET.Interfaces;

namespace awinta.Deployment_NET.Service
{
    public sealed class TeamFoundationServerService : ITeamFoundationServerService
    {
        private const string TeamFoundationServerUrl = "http://tfs:8080/tfs/asys";
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

            Connect(new Uri(TeamFoundationServerUrl));

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

            var status = workSpace.Get(new[] { path }, VersionSpec.Latest, RecursionType.Full, GetOptions.GetAll | GetOptions.Overwrite);

           if (status.NoActionNeeded) { OutputService.WriteOutput($"<TFS>No Actions needed on: {path}"); }

            if (status.NumFailures > 0)
            {
                Failures = status.GetFailures();
            }
        }

        public void GetFileListOutput()
        {

            var items = vcServer.GetItems("$/", RecursionType.Full);
            foreach (var item in items.Items)
            {
                OutputService.WriteOutput(item.ServerItem);
            }

        }

    }
}
