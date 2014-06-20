using System;
using System.Collections.Generic;

using LikeMyDessert.Domain;

namespace LikeMyDessert.Services.Interfaces
{
	public interface IPictureService : IService<Picture>
	{
		IList<Picture> GetFirstPictures(int numberOfPictures);
        Picture GetNextRandomPicture(IEnumerable<Guid> referencePictureIDs);
	}
}
