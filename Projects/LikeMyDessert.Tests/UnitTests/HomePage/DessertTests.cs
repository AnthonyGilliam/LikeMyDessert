using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;
using NUnit.Framework;

using Global.Utilities.Interfaces;
using Global.Utilities;
using HyperQueryNH.Core;
using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;
using LikeMyDessert.Repositories;
using LikeMyDessert.Services.Interfaces;
using LikeMyDessert.Services;
using LikeMyDessert.Web.ViewModels.Picture;
using LikeMyDessert.Web.ViewModels.Dessert;
using LikeMyDessert.Web.ViewModels.HomePage;
using LikeMyDessert.Web.ModelMappers;
using LikeMyDessert.Web.ViewModelManagers.Interfaces;
using LikeMyDessert.Web.ViewModelManagers;
using LikeMyDessert.Web.Controllers;

namespace LikeMyDessert.Tests.UnitTests.HomePage
{
	[TestFixture]
	public class When_a_list_of_Desserts_has_been_requested
	{
	    private Picture _testPic1;
	    private Picture _testPic2;
		private Dessert _testDessert1;
		private Dessert _testDessert2;
	    private PictureViewModel _pictureViewModel;
	    private DessertBoxViewModel _dessertViewModel;
		private Mock<IUnitOfWork<Guid>> _unitOfWorkMock;
        private Mock<IDessertRepository> _dessertRepoMock;
        private Mock<IPictureService> _picServiceMock;
        private Mock<IDessertService> _dessertServiceMock;
		private Mock<IHomePageViewModelManager> _viewModelManagerMock;

		[SetUp]
		public void Context()
		{
            _testPic1 = new Picture
			    {
					ID = new Guid("ca5e0210-16ac-4383-82a7-01efcc651f06"),
			        Alt = "First Test Pic",
					OrdinalIndex = 0,
			        ImageType = "jpg",
			    };

            _testPic2 = new Picture
			    {
                    ID = new Guid("E06EB0EC-F107-431F-B79A-9548BA697B42"),
			        Alt = "Second Test Pic",
					OrdinalIndex = 0,
			        ImageType = "jpg",
			    };

			_testDessert1 = new Dessert
			                {
                                ID = new Guid("ca5e0210-16ac-4383-82a7-01efcc651f06"),
                                Picture = _testPic1,
			                  	Name = "Test Dessert",
                                Likes = 5,
                                Dislikes = 2,
			                  	Description = "A yummy tasty treat",
			                };

			_testDessert2 = new Dessert
			                {
                                ID = new Guid("E06EB0EC-F107-431F-B79A-9548BA697B42"),
                                Picture = _testPic2,
			                  	Name = "Test Dessert",
                                Likes = 10,
                                Dislikes = 3,
			                  	Description = "A yummy tasty treat",
			                };

		    _pictureViewModel = new PictureViewModel
		                            {
		                                Alt = _testPic1.Alt,
		                                ImageType = _testPic1.ImageType,
		                                OrdinalIndex = _testPic1.OrdinalIndex,
		                                Url = "/Context/Photos/" + _testPic1.ID.ToString() + _testPic1.ImageType
		                            };

		    _dessertViewModel = new DessertBoxViewModel
		                            {
                                        ID = _testDessert1.ID,
                                        Name = _testDessert1.Name,
                                        Description = _testDessert1.Description,
                                        Likes = _testDessert1.Likes,
                                        Dislikes = _testDessert1.Dislikes,
                                        Picture = _pictureViewModel
		                            };

            _unitOfWorkMock = new Mock<IUnitOfWork<Guid>>();
            _unitOfWorkMock.Setup(uow => uow.GetAll<Dessert, int>(dessert => dessert.Likes >= 0
                , dessert => dessert.Likes
                , false))
				.Returns(new List<Dessert>() { _testDessert2, _testDessert1 });

            _dessertRepoMock = new Mock<IDessertRepository>();
            _dessertRepoMock.Setup(dr => dr.GetInOrder(dessert => dessert.Likes >= 0
                , dessert => dessert.Likes
                , false))
                .Returns(new List<Dessert>() { _testDessert2, _testDessert1 });

			_picServiceMock = new Mock<IPictureService>();
			_picServiceMock.Setup(ps => ps.GetFirstPictures(5))
				.Returns(new List<Picture>() { _testPic2, _testPic1 });

            _dessertServiceMock = new Mock<IDessertService>();
            _dessertServiceMock.Setup(ds => ds.GetRatedDesserts(false))
                .Returns(new List<Dessert>() { _testDessert2, _testDessert1 });

            _viewModelManagerMock = new Mock<IHomePageViewModelManager>();
		    _viewModelManagerMock.Setup(hpvmm => hpvmm.Get())
		        .Returns(new HomePageViewModel{ Desserts = new List<DessertBoxViewModel>{ _dessertViewModel }});
		}

		[Test]
		public void The_DessertRepository_can_query_in_order_of_rating_List_from_datasource()
		{
			//Setup
			IDessertRepository dessertRepo = new DessertRepository(_unitOfWorkMock.Object);

			//Action
			IList<Dessert> dessertList = dessertRepo.GetInOrder(dessert => dessert.Likes >= 0
                , dessert => dessert.Likes
                , false);

			//Assertions
            _unitOfWorkMock.Verify(uow => uow.GetAll<Dessert, int>(dessert => dessert.Likes >= 0
                , dessert => dessert.Likes
                , false));
            Assert.That(dessertList.Count == 2);
            Assert.AreEqual(dessertList[0], _testDessert2);
            Assert.AreEqual(dessertList[1], _testDessert1);
		}

        [Test]
        public void The_DessertService_can_receive_list_in_order_of_rating_from_repository()
        {
            //Setup
            IDessertService dessertService = new DessertService(_dessertRepoMock.Object);

            //Action
            IList<Dessert> dessertList = dessertService.GetRatedDesserts(false);

            //Assertions
            _dessertRepoMock.Verify(drm => drm.GetInOrder(dessert => dessert.Likes >= 0
                , dessert => dessert.Likes
                , false), Times.Once());
            Assert.That(dessertList.Count == 2);
            Assert.AreEqual(dessertList[0], _testDessert2);
            Assert.AreEqual(dessertList[1], _testDessert1);
        }

        [Test]
        public void The_HomePageViewModelManager_can_receive_list_of_DessertViewModels()
        {
            //Setup
            IHomePageViewModelManager homePageVMM = new HomePageViewModelManager(_picServiceMock.Object
                , _dessertServiceMock.Object);

            //Action
            HomePageViewModel viewModel = homePageVMM.Get();
            Dessert derivedDessert =
                viewModel.Desserts.Select(
                    t =>
                    new Dessert { ID = t.ID, Name = t.Name, Description = t.Description, Likes = t.Likes, Dislikes = t.Dislikes, Picture = _testPic2}).
                    ToList()[0];

            //Assertions
            Assert.That(viewModel.Desserts.Count == 2);
            Assert.AreEqual(derivedDessert.Name, _testDessert2.Name);
            Assert.AreEqual(derivedDessert.Description, _testDessert2.Description);
            Assert.AreEqual(derivedDessert.Likes, _testDessert2.Likes);
            Assert.AreEqual(derivedDessert.Dislikes, _testDessert2.Dislikes);
            Assert.AreEqual(derivedDessert.Picture, _testDessert2.Picture);
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

    [TestFixture]
    public class When_a_user_likes_a_dessert
    {
        private readonly Guid _dessertGuid = new Guid("ca5e0210-16ac-4383-82a7-01efcc651f06");
        private Dessert _testDessert;
        private Mock<IUnitOfWork<Guid>> _unitOfWorkMock;
        private Mock<IHttpHelper> _httpHelperMock;
        private Mock<IPictureService> _pictureServiceMock;
        private Mock<IDessertService> _dessertServiceMock;
        private Mock<IDessertViewModelManager> _dessertViewModelManagerMock;

        [SetUp]
        public void Context()
        {
            _testDessert = new Dessert
            {
                ID = _dessertGuid,
                Likes = 0,
            };

            _unitOfWorkMock = new Mock<IUnitOfWork<Guid>>();
            _unitOfWorkMock.Setup(uow => uow.Get<Dessert>(_dessertGuid))
                .Returns(_testDessert);
            _unitOfWorkMock.Setup(uow => uow.Update(_testDessert));

            _httpHelperMock = new Mock<IHttpHelper>();

            _pictureServiceMock = new Mock<IPictureService>();

            _dessertServiceMock = new Mock<IDessertService>();
            _dessertServiceMock.Setup(svc => svc.LikeDessert(_dessertGuid));

            _dessertViewModelManagerMock = new Mock<IDessertViewModelManager>();
            _dessertViewModelManagerMock.Setup(vmm => vmm.Like(_dessertGuid));
        }

        [Test]
        public void The_DessertService_can_like_the_dessert()
        {
            //Setup
            var dessertRepo = new DessertRepository(_unitOfWorkMock.Object);
            var dessertSvc = new DessertService(dessertRepo);
            var oldLikes = _testDessert.Likes;

            //Actions
            var newDessert = dessertSvc.LikeDessert(_testDessert.ID);

            //Assertions
            _unitOfWorkMock.Verify(uow => uow.Get<Dessert>(_dessertGuid));
            _unitOfWorkMock.Verify(uow => uow.Update(_testDessert));
            Assert.That(newDessert.Likes == oldLikes + 1);
        }

        [Test]
        public void The_DessertViewModelManager_can_call_the_DessertService_to_like_the_dessert()
        {
            //Setup
            var _viewModelManager = new DessertViewModelManager(_httpHelperMock.Object, _pictureServiceMock.Object, _dessertServiceMock.Object);

            //Actions
            _viewModelManager.Like(_dessertGuid);

            //Assertions
            _dessertServiceMock.Verify(svc => svc.LikeDessert(_dessertGuid));
        }

        [Test]
        public void The_DessertController_can_call_the_DessertViewModelManager_to_like_the_dessert()
        {
            //Setup
            var dessertController = new DessertController(_dessertViewModelManagerMock.Object);

            //Actions
            dessertController.Like(_dessertGuid);

            //Assertions
            _dessertViewModelManagerMock.Verify(vmm => vmm.Like(_dessertGuid));
        }
    }

    [TestFixture]
    public class When_a_user_dislikes_a_dessert
    {
        private readonly Guid _dessertGuid = new Guid("ca5e0210-16ac-4383-82a7-01efcc651f06");
        private Dessert _testDessert;
        private Mock<IUnitOfWork<Guid>> _unitOfWorkMock;
        private Mock<IHttpHelper> _httpHelperMock;
        private Mock<IPictureService> _pictureServiceMock;
        private Mock<IDessertService> _dessertServiceMock;
        private Mock<IDessertViewModelManager> _dessertViewModelManagerMock;

        [SetUp]
        public void Context()
        {
            _testDessert = new Dessert
            {
                ID = _dessertGuid,
                Dislikes = 0,
            };

            _unitOfWorkMock = new Mock<IUnitOfWork<Guid>>();
            _unitOfWorkMock.Setup(uow => uow.Get<Dessert>(_dessertGuid))
                .Returns(_testDessert);
            _unitOfWorkMock.Setup(uow => uow.Update(_testDessert));

            _httpHelperMock = new Mock<IHttpHelper>();

            _pictureServiceMock = new Mock<IPictureService>();

            _dessertServiceMock = new Mock<IDessertService>();
            _dessertServiceMock.Setup(svc => svc.DislikeDessert(_dessertGuid));

            _dessertViewModelManagerMock = new Mock<IDessertViewModelManager>();
            _dessertViewModelManagerMock.Setup(vmm => vmm.Dislike(_dessertGuid));
        }

        [Test]
        public void The_DessertService_can_dislike_the_dessert()
        {
            //Setup
            var dessertRepo = new DessertRepository(_unitOfWorkMock.Object);
            var dessertSvc = new DessertService(dessertRepo);
            var oldDislikes = _testDessert.Dislikes;

            //Actions
            var newDessert = dessertSvc.DislikeDessert(_testDessert.ID);

            //Assertions
            _unitOfWorkMock.Verify(uow => uow.Get<Dessert>(_dessertGuid));
            _unitOfWorkMock.Verify(uow => uow.Update(_testDessert));
            Assert.That(newDessert.Dislikes == oldDislikes + 1);
        }

        [Test]
        public void The_DessertViewModelManager_can_call_the_DessertService_to_dislike_the_dessert()
        {
            //Setup
            var _viewModelManager = new DessertViewModelManager(_httpHelperMock.Object, _pictureServiceMock.Object, _dessertServiceMock.Object);

            //Actions
            _viewModelManager.Dislike(_dessertGuid);

            //Assertions
            _dessertServiceMock.Verify(svc => svc.DislikeDessert(_dessertGuid));
        }

        [Test]
        public void The_DessertController_can_call_the_DessertViewModelManager_to_dislike_the_dessert()
        {
            //Setup
            var dessertController = new DessertController(_dessertViewModelManagerMock.Object);

            //Actions
            dessertController.Dislike(_dessertGuid);

            //Assertions
            _dessertViewModelManagerMock.Verify(vmm => vmm.Dislike(_dessertGuid));
        }
    }

    [TestFixture]
    public class When_a_user_uploads_a_new_add_dessert_form_picture
    {
        private TempPictureViewModel _tempPictureViewModel;
        private Mock<HttpRequestBase> _httpRequestMock;
        private Mock<HttpContextBase> _httpContextMock;
        private Mock<IHttpHelper> _httpHelperMock;
        private Mock<HttpFileCollectionBase> _postedFileKeyCollectionMock;
        private Mock<HttpPostedFileBase> _postedfileMock;
        private Mock<IPictureService> _pictureServiceMock;
        private Mock<IDessertService> _dessertServiceMock;
        private Mock<IDessertViewModelManager> _dessertVMMMock;
        private const string TEMP_PIC_FILE_NAME = "Test Pic.jpg";
        private const string TEMP_PIC_FILE_PATH = @"C:\Projects\LikeMyDessert\Projects\LikeMyDessert.Tests\IntegrationTests\Content\Temp\" + TEMP_PIC_FILE_NAME;
        private const string TEMP_PIC_SESSION_KEY = "TempPicture";
        private readonly string _tempPictureUrl = "/C:/Projects/LikeMyDessert/Projects/LikeMyDessert.Tests/IntegrationTests/Content/Temp/" + TEMP_PIC_FILE_NAME;

        [SetUp]
        public void Context()
        {
            _httpRequestMock = new Mock<HttpRequestBase>();
            _httpContextMock = new Mock<HttpContextBase>();
            _httpHelperMock = new Mock<IHttpHelper>();
            _postedFileKeyCollectionMock = new Mock<HttpFileCollectionBase>();
            _postedfileMock = new Mock<HttpPostedFileBase>();
            _dessertVMMMock = new Mock<IDessertViewModelManager>();
            _pictureServiceMock = new Mock<IPictureService>();
            _dessertServiceMock = new Mock<IDessertService>();

            _tempPictureViewModel = new TempPictureViewModel
                                        {
                                            Name = TEMP_PIC_FILE_NAME,
                                            Uri = _tempPictureUrl,
                                            SavedFIlePath = TEMP_PIC_FILE_PATH,
                                            ImageType = "jpg"
                                        };
            
            //Mock System.Web.Request
            _httpContextMock.SetupGet(ctx => ctx.Request)
                .Returns(_httpRequestMock.Object);

            //Mock System.Web.Request.Files
            _httpRequestMock.SetupGet(req => req.Files)
                .Returns(_postedFileKeyCollectionMock.Object);

            //TODO: Add this block in test to find and delete old picture
            ////Mock wrapper over HttpContext.Current
            _httpHelperMock.Setup(hpr => hpr.ExistsInSession(TEMP_PIC_SESSION_KEY))
                .Returns(true);
            
            //Mock System.Web.Request.Files enumerator
            _postedFileKeyCollectionMock.Setup(keys => keys[0])
                .Returns(_postedfileMock.Object);

            //Mock System.Web.Request.Files.FileName
            _postedfileMock.SetupGet(file => file.FileName)
                .Returns(TEMP_PIC_FILE_NAME);

            _dessertVMMMock.Setup(vmm => vmm.UploadTempPicture(_postedfileMock.Object))
                .Returns(_tempPictureViewModel);
        }

        [Test]
        public void The_DessertViewModelManager_can_save_the_picture_to_the_Temp_folder_and_add_it_to_session()
        {
            //Setup
            var dessertVMM = new DessertViewModelManager( _httpHelperMock.Object, _pictureServiceMock.Object, _dessertServiceMock.Object);

            //Actions
            var tempPicViewModel = dessertVMM.UploadTempPicture(_postedfileMock.Object);

            //Assertions
            _httpHelperMock.Verify(hpr => hpr.ExistsInSession(TEMP_PIC_SESSION_KEY));
            _postedfileMock.VerifyGet(file => file.FileName, Times.Once());
            _postedfileMock.Verify(file => file.SaveAs(TEMP_PIC_FILE_PATH), Times.Once());
            _httpHelperMock.Verify(hpr => hpr.AddToAppSession(TEMP_PIC_SESSION_KEY, It.IsAny<TempPictureViewModel>()), Times.Once());
            Assert.AreEqual(tempPicViewModel.Name, TEMP_PIC_FILE_NAME);
            Assert.AreEqual(tempPicViewModel.SavedFIlePath, TEMP_PIC_FILE_PATH);
            Assert.AreEqual(tempPicViewModel.Uri, _tempPictureUrl);
            Assert.AreEqual(tempPicViewModel.ImageType, "jpg");
        }

        [Test]
        public void The_DessertController_can_receive_the_picture_file_and_return_its_temp_path()
        {
            //Setup
            var controller = new DessertController(_dessertVMMMock.Object);
            controller.ControllerContext = new ControllerContext(_httpContextMock.Object, new RouteData(), controller);

            //Actions
            var tempPicView = controller.AddPictureChange();

            //Assertions
            _httpContextMock.Verify(ctx => ctx.Request, Times.Once());
            _httpRequestMock.Verify(rqs => rqs.Files, Times.Once());
            _postedFileKeyCollectionMock.Verify(keys => keys[0], Times.Once());
            Assert.AreEqual(tempPicView.Model, _tempPictureViewModel);
        }
    }

    [TestFixture]
    public class When_a_user_requests_to_add_a_dessert
    {
        private Dessert _testDessert;
        private TempPictureViewModel _tempPictureViewModel;
        private DessertBoxViewModel _testDessertViewModel;
        private Mock<IUnitOfWork<Guid>> _unitOfWorkMock;
        private Mock<IHttpHelper> _httpHelperMock;
        private Mock<IDessertRepository> _dessertRepoMock;
        private Mock<IDessertService> _dessertServiceMock;
        private Mock<IDessertViewModelManager> _dessertVMMMock;
        private const string TEMP_PIC_FILE_NAME = "Test Pic.jpg";
        private const string TEMP_PIC_FILE_PATH = @"C:\Projects\LikeMyDessert\Projects\LikeMyDessert.Tests\IntegrationTests\Content\Temp" + TEMP_PIC_FILE_NAME;
        private const string TEMP_PIC_SESSION_KEY = "TempPicture";
        private readonly string _tempPictureUrl = "/" + TEMP_PIC_FILE_NAME;
        private Mock<IPictureService> _pictureServiceMock;

        [SetUp]
        public void Context()
        {
            _testDessert = new Dessert
                               {
                                   ID = new Guid("ca5e0210-16ac-4383-82a7-01efcc651f06"),
                                   //TODO: Check to see if this needs validating
                                   //Picture = ,
                                   Name = "Test Dessert",
                                   Likes = 5,
                                   Dislikes = 2,
                                   Description = "A yummy tasty treat"
                               };

            _tempPictureViewModel = new TempPictureViewModel
                                        {
                                            Name = TEMP_PIC_FILE_NAME,
                                            SavedFIlePath = TEMP_PIC_FILE_PATH,
                                            ImageType = "jpg"
                                        };

            _testDessertViewModel = new DessertBoxViewModel
                                        {
                                            ID = new Guid("ca5e0210-16ac-4383-82a7-01efcc651f06"),
                                            //TODO: Check to see if this needs validating
                                            //Picture = ,
                                            Name = "Test Dessert",
                                            Likes = 5,
                                            Dislikes = 2,
                                            Description = "A yummy tasty treat"
                                        };

            _unitOfWorkMock = new Mock<IUnitOfWork<Guid>>();
            _httpHelperMock = new Mock<IHttpHelper>();
            //TODO: Add test for when TempPictureViewModel already exists in session.
            _httpHelperMock.Setup(helper => helper.GetFromAppSession<TempPictureViewModel>(TEMP_PIC_SESSION_KEY))
                .Returns(_tempPictureViewModel);
            _dessertRepoMock = new Mock<IDessertRepository>();
            _pictureServiceMock = new Mock<IPictureService>();
            _dessertServiceMock = new Mock<IDessertService>();
            _dessertVMMMock = new Mock<IDessertViewModelManager>();
            File.Copy(@"C:\Projects\LikeMyDessert\Projects\LikeMyDessert.Tests\IntegrationTests\Content\Test Pic.jpg", TEMP_PIC_FILE_PATH, true);
        }

        [Test]
        public void The_dessert_repository_can_persist_the_dessert()
        {
            //Setup
            var repo = new DessertRepository(_unitOfWorkMock.Object);

            //Action
            repo.Save(_testDessert);

            //Assertions
            _unitOfWorkMock.Verify(uow => uow.AddToSession(_testDessert));
        }

        [Test]
        public void The_dessert_service_can_call_the_repository_to_save_the_dessert()
        {
            //Setup
            var service = new DessertService(_dessertRepoMock.Object);

            //Action
            service.Save(_testDessert);

            //Assertions
            _dessertRepoMock.Verify(repo => repo.Save(_testDessert));
        }

        [Test]
        public void The_dessert_view_model_manager_can_call_the_service_to_save_the_dessert()
        {
            //Setup
            HomePageModelMapper.Init();
            var viewModelManager = new DessertViewModelManager(_httpHelperMock.Object, _pictureServiceMock.Object, _dessertServiceMock.Object);

            //Action
            var dessert = AutoMapper.Mapper.Map<DessertBoxViewModel, Dessert>(_testDessertViewModel);
            viewModelManager.Add(_testDessertViewModel);

            //Assertions
            _dessertServiceMock.Verify(service => service.Save(It.IsAny<Dessert>()));
            HyperAssert.ThatPropertiesAreEqual(_testDessert, dessert);
        }

        [Test]
        public void The_dessert_controller_can_call_the_view_model_manager_to_add_the_dessert()
        {
            //Setup
            var controller = new DessertController(_dessertVMMMock.Object);

            //Action
            controller.Add(_testDessertViewModel);

            //Assertions
            _dessertVMMMock.Verify(vmm => vmm.Add(_testDessertViewModel));
        }
    }

    [TestFixture]
    public class When_a_user_closes_the_add_dessert_form
    {
        [SetUp]
        public void SetUp()
        {
            
        }

        [Test]
        public void The_DessertViewModelManager_is_called_from_the_controller_to_delete_the_cached_TempPictureViewModel()
        {
            
        }
    }
}
