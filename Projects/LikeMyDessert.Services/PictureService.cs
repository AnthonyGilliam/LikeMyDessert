using System;
using System.Collections.Generic;
using System.Linq;

using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;
using LikeMyDessert.Services.Interfaces;

namespace LikeMyDessert.Services
{
	public class PictureService : IPictureService
	{
	    private readonly IPictureRepository _pictureRepository;

	    public PictureService(IPictureRepository pictureRepository)
	    {
	        _pictureRepository = pictureRepository;
	    }

		public IList<Picture> GetFirstPictures(int numberOfPictures)    
		{
            return _pictureRepository.GetAllInOrder(0, numberOfPictures);
		}

        public Picture GetNextRandomPicture(IEnumerable<Guid> referencePictureIDs)
        {
            return _pictureRepository.GetNextRandomPicture(referencePictureIDs);
        }

	    public void Save(Picture picture)
	    {
	        _pictureRepository.Save(picture);
	    }
	}
}
