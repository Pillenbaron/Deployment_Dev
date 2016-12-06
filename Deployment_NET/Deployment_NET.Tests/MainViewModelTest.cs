// <copyright file="MainViewModelTest.cs" company="awinta GmbH">Copyright © 2016 awinta GmbH</copyright>
using System;
using System.Collections.ObjectModel;
using EnvDTE;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using awinta.Deployment_NET.Solution.Model;
using awinta.Deployment_NET.ViewModel;

namespace awinta.Deployment_NET.ViewModel.Tests
{
    /// <summary>Diese Klasse enthält parametrisierte Komponententests für MainViewModel.</summary>
    [PexClass(typeof(MainViewModel))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class MainViewModelTest
    {
        /// <summary>Test-Stub für BuildSolution()</summary>
        [PexMethod]
        internal void BuildSolutionTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.BuildSolution();
            // TODO: Assertionen zu Methode MainViewModelTest.BuildSolutionTest(MainViewModel) hinzufügen
        }

        /// <summary>Test-Stub für get_Configuration()</summary>
        [PexMethod]
        internal ConfigData ConfigurationGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            ConfigData result = target.Configuration;
            return result;
            // TODO: Assertionen zu Methode MainViewModelTest.ConfigurationGetTest(MainViewModel) hinzufügen
        }

        /// <summary>Test-Stub für set_Configuration(ConfigData)</summary>
        [PexMethod]
        internal void ConfigurationSetTest([PexAssumeUnderTest]MainViewModel target, ConfigData value)
        {
            target.Configuration = value;
            // TODO: Assertionen zu Methode MainViewModelTest.ConfigurationSetTest(MainViewModel, ConfigData) hinzufügen
        }

        /// <summary>Test-Stub für .ctor()</summary>
        [PexMethod]
        internal MainViewModel ConstructorTest()
        {
            MainViewModel target = new MainViewModel();
            return target;
            // TODO: Assertionen zu Methode MainViewModelTest.ConstructorTest() hinzufügen
        }

        /// <summary>Test-Stub für get_Data()</summary>
        [PexMethod]
        internal ObservableCollection<ProjectData> DataGetTest([PexAssumeUnderTest]MainViewModel target)
        {
            ObservableCollection<ProjectData> result = target.Data;
            return result;
            // TODO: Assertionen zu Methode MainViewModelTest.DataGetTest(MainViewModel) hinzufügen
        }

        /// <summary>Test-Stub für set_Data(ObservableCollection`1&lt;ProjectData&gt;)</summary>
        [PexMethod]
        internal void DataSetTest(
            [PexAssumeUnderTest]MainViewModel target,
            ObservableCollection<ProjectData> value
        )
        {
            target.Data = value;
            // TODO: Assertionen zu Methode MainViewModelTest.DataSetTest(MainViewModel, ObservableCollection`1<ProjectData>) hinzufügen
        }

        /// <summary>Test-Stub für Deploy()</summary>
        [PexMethod]
        internal void DeployTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.Deploy();
            // TODO: Assertionen zu Methode MainViewModelTest.DeployTest(MainViewModel) hinzufügen
        }

        /// <summary>Test-Stub für Load()</summary>
        [PexMethod]
        internal void LoadTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.Load();
            // TODO: Assertionen zu Methode MainViewModelTest.LoadTest(MainViewModel) hinzufügen
        }

        /// <summary>Test-Stub für Save()</summary>
        [PexMethod]
        internal void SaveTest([PexAssumeUnderTest]MainViewModel target)
        {
            target.Save();
            // TODO: Assertionen zu Methode MainViewModelTest.SaveTest(MainViewModel) hinzufügen
        }

        /// <summary>Test-Stub für _BuildDone(vsBuildScope, vsBuildAction)</summary>
        [PexMethod]
        internal void _BuildDoneTest(
            [PexAssumeUnderTest]MainViewModel target,
            vsBuildScope scope,
            vsBuildAction action
        )
        {
            target._BuildDone(scope, action);
            // TODO: Assertionen zu Methode MainViewModelTest._BuildDoneTest(MainViewModel, vsBuildScope, vsBuildAction) hinzufügen
        }
    }
}
