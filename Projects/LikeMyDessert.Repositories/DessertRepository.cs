using System;

using HyperQueryEF.Core;
using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;

namespace LikeMyDessert.Repositories
{
    public class DessertRepository : RepositoryBase<Dessert>, IDessertRepository
    {
        public DessertRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {}
    }
}
