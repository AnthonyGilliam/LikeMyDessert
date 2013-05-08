using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

using HyperQueryNH.Core;

using LikeMyDessert.Repositories;
using LikeMyDessert.Tests.DependencyInjection;

namespace LikeMyDessert.Tests.IntegrationTests
{
	[TestFixture]
	public abstract class IntegrationTestConcerns
	{
	    protected IUnitOfWork<Guid> UnitOfWork = DependencyResolverHelper.GetDependency<IUnitOfWork<Guid>>();

		public void FixtureSetUp()
		{
			//TODO:  Add migration methods here...
		}

		[SetUp]
		public void InitializeTest()
		{
			PersistenceManager.Initilize();
			PersistenceManager.DropDatabase();
			PersistenceManager.CreateDatabase();
		}

		[TearDown]
		public void CleanUp()
		{
            PersistenceManager.DropDatabase();
            PersistenceManager.CreateDatabase();
		}
	}
}
