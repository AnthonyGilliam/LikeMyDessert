using System;
using System.Collections.Generic;

using LikeMyDessert.Domain;

namespace LikeMyDessert.Repositories.Interfaces
{
	public interface IPictureRepository : IRepository<Picture>
	{
        IList<Picture> GetAllInOrder(int skip, int take);
        Picture GetNextRandomPicture(Guid referencePictureID);
    }
}
