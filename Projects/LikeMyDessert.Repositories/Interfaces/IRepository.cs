using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using LikeMyDessert.Domain;

namespace LikeMyDessert.Repositories.Interfaces
{
    public interface IRepository<TPersistentObject>
        where TPersistentObject : PersistentObject
    {
        TPersistentObject GetByID(Guid objectID);

        IList<TPersistentObject> GetAll();

        TPersistentObject GetFirst(Expression<Func<TPersistentObject, bool>> queryExpression
            , Expression<Func<TPersistentObject, string>> sortExpression
            , bool ascending);

        TPersistentObject GetFirst(Expression<Func<TPersistentObject, string>> sortExpression
            , bool ascending);

        IList<TPersistentObject> GetInOrder<TSortType>(Expression<Func<TPersistentObject, bool>> queryExpression
            , Expression<Func<TPersistentObject, TSortType>> sortExpression
            , bool ascending);

        void Save(TPersistentObject obj);
        
		void Update(TPersistentObject obj);
        
		void Delete(TPersistentObject obj);
    }
}
