using System;
using awinta.Deployment_NET.Application.Interfaces;
using awinta.Deployment_NET.Common.Logging;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace awinta.Deployment_NET.Application.Service
{
    public sealed class TeamFoundationServerService : LoggingBase, ITeamFoundationServerService
    {
        private const string TeamFoundationServerUrl = "http://tfs:8080/tfs/asys";
        private VersionControlServer vcServer;
        private Workspace workSpace;
        private WorkspaceInfo workSpaceInfo;

        public TeamFoundationServerService()
        {
            Connect();
        }

        public bool IsConnected { get; private set; }

        public Failure[] Failures { get; private set; }

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
            try
            {
                // Connect to the team project collection and the server that hosts the version-control repository. 
                var tpc = new TfsTeamProjectCollection(path);
                vcServer = tpc.GetService<VersionControlServer>();

                workSpaceInfo = Workstation.Current.GetLocalWorkspaceInfo(vcServer, Environment.MachineName,
                    vcServer.AuthorizedUser);
                workSpace = vcServer.GetWorkspace(workSpaceInfo);

                IsConnected = true;
            }
            catch (Exception exception)
            {
                IsConnected = false;
                logger.Error(exception, $"couldn't connect to Server: {TeamFoundationServerUrl}");
                OutputService.WriteOutput(
                    $"couldn't connect to Server: {TeamFoundationServerUrl} Error:{exception.Message}");
            }
        }

        public void UpdateProject(string path)
        {
            var status = workSpace.Get(new[] {path}, VersionSpec.Latest, RecursionType.Full,
                GetOptions.GetAll | GetOptions.Overwrite);

            if (status.NoActionNeeded) OutputService.WriteOutput($"<TFS>No Actions needed on: {path}");

            if (status.NumFailures > 0)
                Failures = status.GetFailures();
        }

        public void GetFileListOutput()
        {
            var items = vcServer.GetItems("$/", RecursionType.Full);
            foreach (var item in items.Items)
                OutputService.WriteOutput(item.ServerItem);
        }
    }
}