using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

using Moq;
using NUnit.Framework;

using HyperQueryNH.Core;
using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;
using LikeMyDessert.Repositories;
using LikeMyDessert.Services.Interfaces;
using LikeMyDessert.Services;
using LikeMyDessert.Web.ViewModels.HomePage;
using LikeMyDessert.Web.ViewModelManagers.Interfaces;
using LikeMyDessert.Web.ViewModelManagers;
using LikeMyDessert.Web.Controllers;

namespace LikeMyDessert.Tests.UnitTests.HomePage
{
	[TestFixture]
	public class When_a_list_of_Pictures_has_been_requested
	{
		private Picture _testPic;
		private Mock<IUnitOfWork<Guid>> _unitOfWorkMock;
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

			_unitOfWorkMock = new Mock<IUnitOfWork<Guid>>();
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
			//Setup
			IPictureRepository picRepo = new PictureRepository(_unitOfWorkMock.Object);

			//Action
			IList<Picture> picList = picRepo.GetAllInOrder(0, HomePageViewModel.TOP_SLIDE_PIC_COUNT);

			//Assertions
            _unitOfWorkMock.Verify(uow => uow.GetAll<Picture, int>(p => p.OrdinalIndex, true, 0, HomePageViewModel.TOP_SLIDE_PIC_COUNT));
			Assert.That(picList.Count == 1);
			Assert.AreEqual(picList[0], _testPic);
		}

		[Test]
		public void PictureService_can_receive_list_from_repository()
		{
			//Setup
			IPictureService picService = new PictureService(_picRepoMock.Object);

			//Action
			IList<Picture> picList = picService.GetFirstPictures(HomePageViewModel.TOP_SLIDE_PIC_COUNT);

			//Assertions
			_picRepoMock.Verify(prm => prm.GetAllInOrder(0, HomePageViewModel.TOP_SLIDE_PIC_COUNT), Times.Once());
			Assert.That(picList.Count == 1);
			Assert.AreEqual(picList[0], _testPic);
		}

		[Test]
		public void HomePageViewModelManager_can_receive_list_of_PictureViewModels()
		{
			//Setup
			IHomePageViewModelManager homePageVMM = new HomePageViewModelManager(_picServiceMock.Object
                , _dessertServiceMock.Object);

			//Action
			HomePageViewModel viewModel = homePageVMM.Get();
			Picture derivedPic =
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
            Assert.That(viewModel.TopSlidePictures.First().Url == ConfigurationManager.AppSettings["PhotoDirectory"] + _testPic.ID + "." + _testPic.ImageType);
		}

        [Test]
        public void The_HomeController_can_receive_a_viewModel()
        {
            //Setup
            var controller = new HomeController(_viewModelManagerMock.Object);

            //Action
            var viewResults = controller.Index();

            //Assertions
            _viewModelManagerMock.Verify(vmm => vmm.Get());
        }
	}
}
