using System.Configuration;
using System.Linq;
using System.Web.Mvc;

using NUnit.Framework;

using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;
using LikeMyDessert.Repositories;
using LikeMyDessert.Web.ViewModelManagers.Interfaces;
using LikeMyDessert.Web.ViewModels.Picture;
using LikeMyDessert.Web.Controllers;
using LikeMyDessert.Tests.DependencyInjection;

namespace LikeMyDessert.Tests.IntegrationTests.PictureTests
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
			//Arrange
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

    [TestFixture]
    public class When_the_next_random_picture_for_TopSlidePicture_show_has_been_requested : IntegrationTestConcerns
    {
        private Picture _testPic1;
        private Picture _testPic2;
        private Picture _testPic3;
        private Picture _testPic4;
        private IPictureViewModelManager _pictureViewModelManager;

        [SetUp]
        public void Context()
        {
            _testPic1 = new Picture
            {
                Alt = "First Test Pic",
                OrdinalIndex = 0,
                ImageType = "jpg",
            };
            UnitOfWork.AddToSession(_testPic1);

            _testPic2 = new Picture
            {
                Alt = "Second Test Pic",
                OrdinalIndex = 1,
                ImageType = "jpg",
            };
            UnitOfWork.AddToSession(_testPic2);

            _testPic3 = new Picture
            {
                Alt = "Third Test Pic",
                OrdinalIndex = 2,
                ImageType = "jpg",
            };
            UnitOfWork.AddToSession(_testPic3);

            _testPic4 = new Picture
            {
                Alt = "Fourth Test Pic",
                OrdinalIndex = 3,
                ImageType = "jpg",
            };
            UnitOfWork.AddToSession(_testPic4);

            PersistenceManager.CommitCachedObjects();

            _pictureViewModelManager = DependencyResolverHelper.GetRealDependency<IPictureViewModelManager>();
        }

        [Test]
        public void The_PictureController_can_return_a_random_picture_unequal_to_reference_picture_in_request()
        {
            //Arrange
            var pictureController = new PictureController(_pictureViewModelManager);
            var referencePictureID = UnitOfWork.GetRandom<Picture>().ID;

            //Act
            var actionResult = pictureController.GetNextTopSlidePicture(referencePictureID);
            var viewModel = ((ViewResult)actionResult).Model as PictureViewModel;

            //Assert
            Assert.NotNull(viewModel);
            Assert.NotNull(viewModel.ID);
            Assert.NotNull(viewModel.Alt);
            Assert.NotNull(viewModel.ImageType);
            Assert.NotNull(viewModel.OrdinalIndex);
            Assert.NotNull(viewModel.Url);
            Assert.AreNotEqual(referencePictureID, viewModel.ID);
        }
    }
}
