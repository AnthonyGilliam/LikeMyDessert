using System;

using HyperQueryNH.Core;
using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;

namespace LikeMyDessert.Repositories
{
    public class DessertRepository : RepositoryBase<Dessert>, IDessertRepository
    {
        public DessertRepository(IUnitOfWork<Guid> unitOfWork) : base(unitOfWork)
        {}
    }
}
