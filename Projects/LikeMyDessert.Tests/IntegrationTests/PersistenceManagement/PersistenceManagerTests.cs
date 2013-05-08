using NUnit.Framework;

using LikeMyDessert.Repositories;
using LikeMyDessert.Repositories.Interfaces;
using LikeMyDessert.Tests.DependencyInjection;
using LikeMyDessert.Domain;

namespace LikeMyDessert.Tests.IntegrationTests.PersistenceManagement
{
	[TestFixture]
	public class When_PersistenceManager_Has_Been_Initialized : IntegrationTestConcerns
	{
		private IPictureRepository _pictureRepo;

		[SetUp]
		public void Context()
		{
			_pictureRepo = DependencyResolverHelper.GetRealDependency<IPictureRepository>();
		}

		[Test]
		public void A_Test_Database_Can_Be_Created()
		{
			PersistenceManager.CreateDatabase();
		}
	
		[Test]
		public void A_Domain_Object_Can_Be_Persisted()
		{
			var pic = new Picture
			          	{
			          		Alt = "Test Pic"
							, ImageType = "image/jpeg"
			          	};

			_pictureRepo.Save(pic);
			PersistenceManager.CommitCachedObjects();

			var savedPic = _pictureRepo.GetByID(pic.ID);

			Assert.AreEqual(pic, savedPic);
		}
	}
}
