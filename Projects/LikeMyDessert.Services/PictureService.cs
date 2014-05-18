using System;
using System.Collections.Generic;
using System.Linq;

using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;
using LikeMyDessert.Services.Interfaces;

namespace LikeMyDessert.Services
{
	public class PictureService : ServiceBase<Picture, IPictureRepository>, IPictureService
	{
		public PictureService(IPictureRepository pictureRepository) : base(pictureRepository)
		{
		}

        //TODO: Refactor UnitOfWork to get the first n pictures from NHibernate Session
		public IList<Picture> GetFirstPictures(int numberOfPictures)
		{
            return Repository.GetAllInOrder(0, numberOfPictures);
		}

        public Picture GetNextRandomPicture(Guid referencePictureID)
        {
            return Repository.GetNextRandomPicture(referencePictureID);
        }
    }
}
