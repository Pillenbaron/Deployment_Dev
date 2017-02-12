﻿using System.Data.Entity;
using awinta.Deployment_NET.Application.Data;

namespace awinta.Deployment_NET.Common.DataContext
{
    public class DataContext : DbContext
    {

        public DataContext() : base(Properties.Settings.Default.Deployment_NETConnectionString) { }

        //public void init()
        //{

        //    DbConnection connection = new EntityConnection();

        //}


        public DbSet<ProjectData> Projects { get; set; }

        public DbSet<AssemblyData> Assemblys { get; set; }

        public DbSet<VersionData> Versions { get; set; }

        public DbSet<ConfigData> Configs { get; set; }

        public DbSet<DeployData> Deploys { get; set; }

    }
}
