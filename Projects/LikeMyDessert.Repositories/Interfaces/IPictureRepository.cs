using System;
using System.Collections.Generic;

using LikeMyDessert.Domain;

namespace LikeMyDessert.Repositories.Interfaces
{
	public interface IPictureRepository
	{
        IList<Picture> GetAllInOrder(int skip, int take);
        Picture GetNextRandomPicture(IEnumerable<Guid> referencePictureIDs);
	    void Save(Picture picture);
	}
}
