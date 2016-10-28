using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

using Moq;
using NUnit.Framework;

using HyperQueryEF.Core;
using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;
using LikeMyDessert.Repositories;
using LikeMyDessert.Services.Interfaces;
using LikeMyDessert.Services;
using LikeMyDessert.Web.ViewModels.HomePage;
using LikeMyDessert.Web.ViewModels.Picture;
using LikeMyDessert.Web.ViewModelManagers.Interfaces;
using LikeMyDessert.Web.ViewModelManagers;
using LikeMyDessert.Web.Controllers;

namespace LikeMyDessert.Tests.UnitTests.PictureTests
{
	[TestFixture]
	public class When_a_list_of_Pictures_has_been_requested
	{
		private Picture _testPic;
		private Mock<IUnitOfWork> _unitOfWorkMock;
		private Mock<IPictureRepository> _picRepoMock;
		private Mock<IPictureService> _picServiceMock;
        private Mock<IDessertService> _dessertServiceMock;
        private Mock<IHomePageViewModelManager> _viewModelManagerMock;

		[SetUp]
		public void Context()
		{
			_testPic = new Picture
			                {
								ID = new Guid("ca5e0210-16ac-4383-82a7-01efcc651f06"),
			                  	Alt = "Test Pic",
								OrdinalIndex = 0,
			                  	ImageType = "jpg",
			                };

			_unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.GetAll<Picture, int>(p => p.OrdinalIndex, true, 0, HomePageViewModel.TOP_SLIDE_PIC_COUNT))
				.Returns(new List<Picture>() { _testPic });

			_picRepoMock = new Mock<IPictureRepository>();
			_picRepoMock.Setup(prm => prm.GetAllInOrder(0, HomePageViewModel.TOP_SLIDE_PIC_COUNT))
				.Returns(new List<Picture>() { _testPic });

			_picServiceMock = new Mock<IPictureService>();
            _picServiceMock.Setup(psm => psm.GetFirstPictures(HomePageViewModel.TOP_SLIDE_PIC_COUNT))
				.Returns(new List<Picture>() { _testPic });

            _dessertServiceMock = new Mock<IDessertService>();
            _dessertServiceMock.Setup(dsm => dsm.GetRatedDesserts(false))
                .Returns(new List<Dessert>() {});

            
            _viewModelManagerMock = new Mock<IHomePageViewModelManager>();
		    _viewModelManagerMock.Setup(hpvmm => hpvmm.Get());
		}

		[Test]
		public void PictureRepository_can_query_List_from_datasource()
		{
			//Arrange
			var picRepo = new PictureRepository(_unitOfWorkMock.Object);

			//Action
			var picList = picRepo.GetAllInOrder(0, HomePageViewModel.TOP_SLIDE_PIC_COUNT);

			//Assertions
            _unitOfWorkMock.Verify(uow => uow.GetAll<Picture, int>(p => p.OrdinalIndex, true, 0, HomePageViewModel.TOP_SLIDE_PIC_COUNT));
			Assert.That(picList.Count == 1);
			Assert.AreEqual(picList[0], _testPic);
		}

		[Test]
		public void PictureService_can_receive_list_from_repository()
		{
			//Arrange
			var picService = new PictureService(_picRepoMock.Object);

			//Action
			var picList = picService.GetFirstPictures(HomePageViewModel.TOP_SLIDE_PIC_COUNT);

			//Assertions
			_picRepoMock.Verify(prm => prm.GetAllInOrder(0, HomePageViewModel.TOP_SLIDE_PIC_COUNT), Times.Once());
			Assert.That(picList.Count == 1);
			Assert.AreEqual(picList[0], _testPic);
		}

		[Test]
		public void HomePageViewModelManager_can_receive_list_of_PictureViewModels()
		{
			//Arrange
			var homePageVMM = new HomePageViewModelManager(_picServiceMock.Object
                , _dessertServiceMock.Object);

			//Action
			var viewModel = homePageVMM.Get();
			var derivedPic =
				viewModel.TopSlidePictures.Select(
					t =>
					new Picture { Alt = t.Alt, OrdinalIndex = t.OrdinalIndex, ImageType = t.ImageType }).
					ToList()[0];

			//Assertions
            _picServiceMock.Verify(psm => psm.GetFirstPictures(HomePageViewModel.TOP_SLIDE_PIC_COUNT), Times.Once());
			Assert.That(viewModel.TopSlidePictures.Count == 1);
			Assert.AreEqual(derivedPic.Alt, _testPic.Alt);
			Assert.AreEqual(derivedPic.OrdinalIndex, _testPic.OrdinalIndex);
			Assert.AreEqual(derivedPic.ImageType, _testPic.ImageType);
            Assert.AreEqual(string.Format("{0}{1}.{2}", ConfigurationManager.AppSettings["PhotoDirectory"], _testPic.ID, _testPic.ImageType)
                            , viewModel.TopSlidePictures.First().Url);
		}

        [Test]
        public void The_HomeController_can_receive_a_viewModel()
        {
            //Arrange
            var controller = new HomeController(_viewModelManagerMock.Object);

            //Action
            var viewResults = controller.Index();

            //Assertions
            _viewModelManagerMock.Verify(vmm => vmm.Get());
        }
	}

    [TestFixture]
    public class When_the_next_random_picture_for_TopSlidePicture_show_has_been_requested
    {
        private IEnumerable<Guid> _referencePictureIDs;
        private Picture _randomPicture;
        private PictureViewModel _randomPictureViewModel;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IPictureRepository> _pictureRepositoryMock;
        private Mock<IPictureService> _pictureServiceMock;
        private Mock<IPictureViewModelManager> _pictureViewModelManagerMock;

        [SetUp]
        public void Context()
        {
            _referencePictureIDs = new List<Guid>(){ Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };
            _randomPicture = new Picture { 
                                            ID = Guid.NewGuid(),
                                            Alt = "Random Picture",
                                            OrdinalIndex = 0,
                                            ImageType = "jpg"
                                         };
            _randomPictureViewModel = new PictureViewModel
                                        {
                                            ID = _randomPicture.ID,
                                            Alt = _randomPicture.Alt,
                                            OrdinalIndex = _randomPicture.OrdinalIndex,
                                            ImageType = _randomPicture.ImageType,
                                            Url = string.Format("{0}{1}.{2}"
                                            , ConfigurationManager.AppSettings["PhotoDirectory"]
                                            , _randomPicture.ID, _randomPicture.ImageType)
                                        };

            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.GetRandom<Picture>(It.Is<Expression<Func<Picture, bool>>>(e => ReferencePictureIDsDoesNotContainRandomPictureID(e))))
                .Returns(_randomPicture);

            _pictureRepositoryMock = new Mock<IPictureRepository>();
            _pictureRepositoryMock.Setup(repo => repo.GetNextRandomPicture(_referencePictureIDs))
                .Returns(_randomPicture);

            _pictureServiceMock = new Mock<IPictureService>();
            _pictureServiceMock.Setup(svc => svc.GetNextRandomPicture(_referencePictureIDs))
                .Returns(_randomPicture);

            _pictureViewModelManagerMock = new Mock<IPictureViewModelManager>();
            _pictureViewModelManagerMock.Setup(mgr => mgr.GetNextTopSlidePicture(_referencePictureIDs))
                .Returns(_randomPictureViewModel);
        }

        private bool ReferencePictureIDsDoesNotContainRandomPictureID(Expression<Func<Picture, bool>> expr)
        {
            var func = expr.Compile();

            Assert.IsFalse(func(new Picture { ID = _referencePictureIDs.ToList()[3] }));

            return true;
        }

        [Test]
        public void The_PictureRepository_can_query_next_random_picture()
        {
            //Arrange
            var picRepo = new PictureRepository(_unitOfWorkMock.Object);

            //Act
            var randomPicture = picRepo.GetNextRandomPicture(_referencePictureIDs);

            //Assert
            _unitOfWorkMock.Verify(uow =>
                uow.GetRandom<Picture>(It.Is<Expression<Func<Picture, bool>>>(e => ReferencePictureIDsDoesNotContainRandomPictureID(e)))
                , Times.Once());

            Assert.AreEqual(_randomPicture, randomPicture);
        }

        [Test]
        public void The_PictureService_can_return_a_random_picture_from_the_PictureRepository()
        {
            //Arrange
            var picService = new PictureService(_pictureRepositoryMock.Object);

            //Act
            var randomPicture = picService.GetNextRandomPicture(_referencePictureIDs);

            //Assert
            _pictureRepositoryMock.Verify(repo => repo.GetNextRandomPicture(_referencePictureIDs), Times.Once());
            Assert.AreEqual(_randomPicture, randomPicture);
        }

        [Test]
        public void The_PictureViewModelManager_can_return_the_next_TopSlidePicture_view_model()
        {
            //Arrange
            var pictureVMM = new PictureViewModelManager(_pictureServiceMock.Object);

            //Act
            var pictureViewModel = pictureVMM.GetNextTopSlidePicture(_referencePictureIDs);

            //Assert
            _pictureServiceMock.Verify(svc => svc.GetNextRandomPicture(_referencePictureIDs), Times.Once());
            Assert.AreEqual(_randomPicture.ID, pictureViewModel.ID);
            Assert.AreEqual(_randomPicture.Alt, pictureViewModel.Alt);
            Assert.AreEqual(_randomPicture.ImageType, pictureViewModel.ImageType);
            Assert.AreEqual(_randomPicture.OrdinalIndex, pictureViewModel.OrdinalIndex);
            Assert.AreEqual(string.Format("{0}{1}.{2}", ConfigurationManager.AppSettings["PhotoDirectory"], _randomPicture.ID, _randomPicture.ImageType)
                            , pictureViewModel.Url);
        }

        [Test]
        public void The_PictureController_can_return_the_next_TopSlidePicture_file()
        {
            //Arrange
            var pictureController = new PictureController(_pictureViewModelManagerMock.Object);

            //Act
            var actionResult = pictureController.GetNextTopSlidePicture(_referencePictureIDs);

            //Assert
            _pictureViewModelManagerMock.Verify(mgr => mgr.GetNextTopSlidePicture(_referencePictureIDs), Times.Once());
            
            var viewModel = ((ViewResult)actionResult).Model;

            //Assert the json data returned from the controller is the same as the viewModel returned from the view model manager.
            viewModel.GetType()
            .GetProperties()
            .ToList()
            .ForEach(modelProp => 
                Assert.AreEqual(typeof(PictureViewModel)
                                .GetProperties()
                                .Single(picProp => picProp.Name == modelProp.Name)
                                .GetValue(_randomPictureViewModel, null), modelProp.GetValue(viewModel, null)));

            Assert.AreNotEqual(_referencePictureIDs, ((PictureViewModel)viewModel).ID);
        }
    }
}