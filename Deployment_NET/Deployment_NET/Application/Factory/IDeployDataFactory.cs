using System;
using System.Collections.Generic;
using awinta.Deployment_NET.Application.Data;

namespace awinta.Deployment_NET.Application.Factory
{
    public interface IDeployDataFactory
    {
        void Add(DeployData value);
        void AddRange(IEnumerable<DeployData> values);
        DeployData GetInstance(Func<DeployData, bool> predicate);
        IEnumerable<DeployData> GetInstances(Func<DeployData, bool> predicate);
        void Remove(DeployData value);
        void RemoveRange(IEnumerable<DeployData> values);
        void SaveChanges();
    }
}