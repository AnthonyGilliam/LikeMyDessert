using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LikeMyDessert.Domain;

namespace LikeMyDessert.Repositories.Interfaces
{
    public interface IDessertRepository
    {
        Dessert GetByID(Guid id);
        IList<Dessert> GetInOrder(Expression<Func<Dessert, bool>> expression, Expression<Func<Dessert, int>> sortExpression, bool ascending);
        void Save(Dessert dessert);
        void Update(Dessert dessert);
    }
}
