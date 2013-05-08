using System.Configuration;
using System.Linq;

using NUnit.Framework;

using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;
using LikeMyDessert.Repositories;
using LikeMyDessert.Web.ViewModelManagers.Interfaces;
using LikeMyDessert.Tests.DependencyInjection;

namespace LikeMyDessert.Tests.IntegrationTests.HomePage
{
	[TestFixture]
	public class When_A_List_Of_Pictures_Has_Been_Requested : IntegrationTestConcerns
	{
		private Picture _testPic;
		private IHomePageViewModelManager _viewModelManager;

		[SetUp]
		public void Context()
		{
			_testPic = new Picture
			{
				Alt = "Test Pic",
				OrdinalIndex = 0,
				ImageType = "jpg",
			};

			_viewModelManager = DependencyResolverHelper.GetRealDependency<IHomePageViewModelManager>();

			UnitOfWork.AddToSession(_testPic);
			PersistenceManager.CommitCachedObjects();
		}

		[Test]
		public void HomePageController_can_render_TopPictureSlideShow_view()
		{
			//Setup
            var viewModel = _viewModelManager.Get();

            //Action
            var derivedPic = 
                viewModel.TopSlidePictures.Select(
                    t =>
                    new Picture { Alt = t.Alt, OrdinalIndex = t.OrdinalIndex, ImageType = t.ImageType }).
                    ToList()[0];

            //Assertions
            Assert.That(viewModel.TopSlidePictures.Count == 1);
            Assert.AreEqual(derivedPic.Alt, _testPic.Alt);
            Assert.AreEqual(derivedPic.OrdinalIndex, _testPic.OrdinalIndex);
            Assert.AreEqual(derivedPic.ImageType, _testPic.ImageType);
            Assert.That(viewModel.TopSlidePictures.First().Url == ConfigurationManager.AppSettings["PhotoDirectory"] + _testPic.ID + "." + _testPic.ImageType);
        }
	}
}
