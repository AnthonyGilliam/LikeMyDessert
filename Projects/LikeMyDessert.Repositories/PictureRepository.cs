using System;
using System.Linq;
using System.Collections.Generic;
using Global.Utilities;
using HyperQueryEF.Core;
using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;

namespace LikeMyDessert.Repositories
{
	public class PictureRepository : RepositoryBase<Picture>, IPictureRepository
	{
		public PictureRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
		{}

        public virtual IList<Picture> GetAllInOrder(int skip, int take)
        {
            IList<Picture> objList = UnitOfWork.GetAll<Picture>()
                .OrderBy(p => p.OrdinalIndex)
                .Skip(skip)
                .Take(take)
                .ToList();

            return objList;
        }

        public Picture GetNextRandomPicture(IEnumerable<Guid> referencePictureIDs)
        {
            var randomPicture = referencePictureIDs != null
                ? UnitOfWork.GetRandom<Picture, int>(picture => !referencePictureIDs.ToArray().Contains(picture.ID), picture => picture.OrdinalIndex)
                : null;
            
            return randomPicture;
        }

	    public override void Save(Picture picture)
        {
            picture.OrdinalIndex = GetCount() + 1;

            base.Save(picture);
        }
    }
}
