using System;
using System.Collections.Generic;
using System.Linq;
using awinta.Deployment_NET.Application.Data;
using awinta.Deployment_NET.Application.Service;
using awinta.Deployment_NET.Common.DataContext;
using awinta.Deployment_NET.Common.Logging;

namespace awinta.Deployment_NET.Application.Factory
{
    public class DeployDataFactory : LoggingBase, IDeployDataFactory
    {

        private readonly DataContext dataBase;

        public DeployDataFactory()
        {
            dataBase = new DataContext();
        }

        public void SaveChanges()
        {

            dataBase.SaveChanges();

        }

        public void Add(DeployData value)
        {

            try
            {
                dataBase.Deploys.Add(value);
            }
            catch (Exception e)
            {
                OutputService.WriteOutputWithContext($"Error: {e.Message}");
                logger.Error(e, e.Message);
            }

        }

        public void AddRange(IEnumerable<DeployData> values)
        {

            try
            {
                dataBase.Deploys.AddRange(values);
            }
            catch (Exception e)
            {
                OutputService.WriteOutputWithContext($"Error: {e.Message}");
                logger.Error(e, e.Message);
            }

        }

        public DeployData GetInstance(Func<DeployData, bool> predicate)
        {
            return dataBase.Deploys.FirstOrDefault(predicate);
        }

        public IEnumerable<DeployData> GetInstances(Func<DeployData, bool> predicate)
        {
            var query = from deployData in dataBase.Deploys
                        where predicate(deployData)
                        orderby deployData.UserName
                        select deployData;

            return query.ToList();
        }

        public void Remove(DeployData value)
        {
            dataBase.Deploys.Remove(value);
        }

        public void RemoveRange(IEnumerable<DeployData> values)
        {
            dataBase.Deploys.RemoveRange(values);
        }
    }
}
