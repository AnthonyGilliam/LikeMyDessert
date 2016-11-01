using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Global.Utilities;
using Global.Utilities.ExtensionMethods;
using HyperQueryEF.Core;
using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;

namespace LikeMyDessert.Repositories
{
    public class DessertRepository : RepositoryBase<Dessert>, IDessertRepository
    {
        public DessertRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {}

        public IList<Dessert> GetInOrder(Expression<Func<Dessert, bool>> expression, Expression<Func<Dessert, int>> sortExpression, bool ascending)
        {
            var desserts = UnitOfWork.GetAll<Dessert>(expression)
                .OrderBy(sortExpression)
                .ToList();

            return desserts;
        }

        public new void Save(Dessert dessert)
        {
            UnitOfWork.Save(dessert);
        }
    }
}
