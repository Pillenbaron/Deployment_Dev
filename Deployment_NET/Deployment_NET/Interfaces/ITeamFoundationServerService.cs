using Microsoft.TeamFoundation.VersionControl.Client;
using System;

namespace awinta.Deployment_NET.Interfaces
{
    public interface ITeamFoundationServerService
    {
        Failure[] Failures { get; }

        void Connect();
        void Connect(Uri path);
        void Connect(string path);
        void GetFileListOutput();
        void UpdateProject(string path);
    }
}