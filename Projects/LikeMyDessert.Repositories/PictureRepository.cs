using System;
using System.Linq;
using System.Collections.Generic;

using HyperQueryNH.Core;
using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;

namespace LikeMyDessert.Repositories
{
	public class PictureRepository : RepositoryBase<Picture>, IPictureRepository
	{
		public PictureRepository(IUnitOfWork<Guid> unitOfWork) : base(unitOfWork)
		{}

        public virtual IList<Picture> GetAllInOrder(int skip, int take)
        {
            IList<Picture> objList = UnitOfWork.GetAll<Picture, int>(p => p.OrdinalIndex, true, skip, take);
            return objList;
        }

        public override void Save(Picture picture)
        {
            picture.OrdinalIndex = (int)base.GetCount() + 1;

            base.Save(picture);
        }

        public Picture GetNextRandomPicture(IEnumerable<Guid> referencePictureIDs)
        {
            var randomPicture = referencePictureIDs != null
                ? UnitOfWork.GetRandom<Picture>(picture => !referencePictureIDs.ToArray().Contains(picture.ID))
                : null;
            
            return randomPicture;
        }
    }
}
