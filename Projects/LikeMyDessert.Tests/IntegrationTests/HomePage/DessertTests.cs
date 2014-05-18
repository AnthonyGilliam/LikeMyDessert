using System;
using System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;
using Moq;

using Global.Utilities.Interfaces;
using LikeMyDessert.Domain;
using LikeMyDessert.Repositories;
using LikeMyDessert.Services.Interfaces;
using LikeMyDessert.Web.ModelMappers;
using LikeMyDessert.Web.ViewModels.Picture;
using LikeMyDessert.Web.ViewModels.Dessert;
using LikeMyDessert.Web.ViewModels.HomePage;
using LikeMyDessert.Web.ViewModelManagers.Interfaces;
using LikeMyDessert.Web.ViewModelManagers;
using LikeMyDessert.Web.Controllers;
using LikeMyDessert.Tests.DependencyInjection;

namespace LikeMyDessert.Tests.IntegrationTests.DessertTests
{
    [TestFixture]
    public class When_a_list_of_Desserts_has_been_requested : IntegrationTestConcerns 
    {
        private Picture _testPic1;
        private Picture _testPic2;
        private Dessert _testDessert1;
        private Dessert _testDessert2;
        private IHomePageViewModelManager _homePageVMM;

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
                OrdinalIndex = 0,
                ImageType = "jpg",
            };
            UnitOfWork.AddToSession(_testPic2);

            _testDessert1 = new Dessert
            {
                Name = "Test Dessert 1",
                Likes = 5,
                Dislikes = 2,
                Description = "A yummy tasty treat",
                Picture = _testPic1,
            };
            UnitOfWork.AddToSession(_testDessert1);

            _testDessert2 = new Dessert
            {
                Name = "Test Dessert 2",
                Likes = 10,
                Dislikes = 3,
                Description = "A yummy tasty treat",
                Picture = _testPic2,
            };
            UnitOfWork.AddToSession(_testDessert2);

            PersistenceManager.CommitCachedObjects();
        }

        [Test]
        public void The_HomeController_can_return_a_list_of_desserts()
        {
            //Arrange
            _homePageVMM = DependencyResolverHelper.GetRealDependency<IHomePageViewModelManager>();

            //Action
            HomePageViewModel viewModel = _homePageVMM.Get();
            Dessert derivedDessert =
                viewModel.Desserts.Select(
                    d =>
                    new Dessert { ID = d.ID, Name = d.Name, Picture = _testPic2, Description = d.Description, Likes = d.Likes, Dislikes = d.Dislikes }).
                    ToList()[0];

            //Assertions
            Assert.That(viewModel.Desserts.Count == 2);
            Assert.AreEqual(derivedDessert.Name, _testDessert2.Name);
            Assert.AreEqual(derivedDessert.Description, _testDessert2.Description);
            Assert.AreEqual(derivedDessert.Likes, _testDessert2.Likes);
            Assert.AreEqual(derivedDessert.Dislikes, _testDessert2.Dislikes);
            Assert.AreEqual(derivedDessert.Picture, _testDessert2.Picture);
        }
    }

    [TestFixture]
    public class When_a_user_likes_a_dessert : IntegrationTestConcerns
    {
        private Picture _testPic;
        private Dessert _testDessert;
        private IDessertViewModelManager _dessertViewModelManager;

        [SetUp]
        public void Context()
        {
            _testPic = new Picture
            {
                Alt = "Second Test Pic",
                OrdinalIndex = 0,
                ImageType = "jpg",
            };
            UnitOfWork.AddToSession(_testPic);

            _testDessert = new Dessert
            {
                Name = "Test Dessert",
                Description = "A yummy tasty treat",
                Likes = 0,
                Picture = _testPic
            };
            UnitOfWork.AddToSession(_testDessert);
            
            PersistenceManager.CommitCachedObjects();
        }

        [Test]
        public void The_amount_of_likes_for_the_dessert_is_increased_by_one_in_the_DataSource()
        {
            //Arrange

            var httpHelper = DependencyResolverHelper.GetMockDependency<IHttpHelper>();
            var picService = DependencyResolverHelper.GetDependency<IPictureService>();
            var dessertService = DependencyResolverHelper.GetDependency<IDessertService>();
            _dessertViewModelManager = new DessertViewModelManager(httpHelper, picService, dessertService);

            var dessertController = new DessertController(_dessertViewModelManager);
            var oldDessertLikes = _testDessert.Likes;

            //Actions
            dessertController.Like(_testDessert.ID);
            PersistenceManager.CommitCachedObjects();
            PersistenceManager.ClearCache();
            
            //Assertions
            var likedDessert = UnitOfWork.Get<Dessert>(_testDessert.ID);
            Assert.That(likedDessert.Likes == oldDessertLikes + 1);
        }
    }

    [TestFixture]
    public class When_a_user_dislikes_a_dessert : IntegrationTestConcerns
    {
        private Picture _testPic;
        private Dessert _testDessert;
        private IDessertViewModelManager _dessertViewModelManager;

        [SetUp]
        public void Context()
        {
            _testPic = new Picture
            {
                Alt = "Second Test Pic",
                OrdinalIndex = 0,
                ImageType = "jpg",
            };
            UnitOfWork.AddToSession(_testPic);

            _testDessert = new Dessert
            {
                Name = "Test Dessert",
                Description = "A yummy tasty treat",
                Dislikes = 0,
                Picture = _testPic
            };
            UnitOfWork.AddToSession(_testDessert);
            
            PersistenceManager.CommitCachedObjects();
        }

        [Test]
        public void The_amount_of_dislikes_for_the_dessert_is_decreased_by_one_in_the_DataSource()
        {
            //Arrange

            var httpHelper = DependencyResolverHelper.GetMockDependency<IHttpHelper>();
            var picService = DependencyResolverHelper.GetDependency<IPictureService>();
            var dessertService = DependencyResolverHelper.GetDependency<IDessertService>();
            _dessertViewModelManager = new DessertViewModelManager(httpHelper, picService, dessertService);

            var dessertController = new DessertController(_dessertViewModelManager);
            var oldDessertDislikes = _testDessert.Dislikes;

            //Actions
            dessertController.Dislike(_testDessert.ID);
            PersistenceManager.CommitCachedObjects();
            PersistenceManager.ClearCache();
            
            //Assertions
            var likedDessert = UnitOfWork.Get<Dessert>(_testDessert.ID);
            Assert.That(likedDessert.Dislikes == oldDessertDislikes + 1);
        }
    }

    [TestFixture]
    public class When_a_user_adds_a_dessert : IntegrationTestConcerns
    {
        private PictureViewModel _testPictureViewModel;
        private DessertBoxViewModel _testDessertBoxViewModel;
        private IDessertViewModelManager _dessertViewModelManager;

        [SetUp]
        public void Context()
        {
            HomePageModelMapper.Init();

            _testDessertBoxViewModel = new DessertBoxViewModel
            {
                Picture = _testPictureViewModel,
                Name = "Test Dessert",
                Likes = 5,
                Dislikes = 2,
                Description = "A yummy tasty treat"
            };

            var httpHelper = DependencyResolverHelper.GetMockDependency<IHttpHelper>();
            var picService = DependencyResolverHelper.GetDependency<IPictureService>();
            var dessertService = DependencyResolverHelper.GetDependency<IDessertService>();
            _dessertViewModelManager = new DessertViewModelManager(httpHelper, picService, dessertService);
        }

        [Test]
        public void The_dessert_is_saved_to_the_database_when_correctly_entered()
        {
            //Arrange
            var controller = new DessertController(_dessertViewModelManager);
            controller.Add(_testDessertBoxViewModel);
            PersistenceManager.CommitCachedObjects();
            PersistenceManager.ClearCache();

            //Actions
            var dessert = UnitOfWork.GetAll<Dessert>()[0];
            
            var testDirectory = @"C:\Projects\LikeMyDessert\Projects\LikeMyDessert.Tests\bin\Debug";
            var testFile = Path.Combine(testDirectory, dessert.Picture.ID.ToString() + ".jpg");
            File.Delete(testFile);

            //Assertions
            Assert.That(_testDessertBoxViewModel.Name == dessert.Name);
            Assert.That(_testDessertBoxViewModel.Description == dessert.Description);
            Assert.That(_testDessertBoxViewModel.Likes == dessert.Likes);
            Assert.That(_testDessertBoxViewModel.Dislikes == dessert.Dislikes);
            Assert.That(dessert.Picture.OrdinalIndex == 1);
        }
    }
}
