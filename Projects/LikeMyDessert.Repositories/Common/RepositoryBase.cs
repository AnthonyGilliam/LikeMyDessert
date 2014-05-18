using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using HyperQueryNH.Core;
using LikeMyDessert.Domain;


namespace LikeMyDessert.Repositories
{
    /// <summary>
    /// Repositories are the utilities that query specific objects from the database gateway.
    /// </summary>
    /// <typeparam name="TPersistentObject">The object type queries will be based on.</typeparam>
	public abstract class RepositoryBase<TPersistentObject>
        where TPersistentObject : PersistentObject
    {
        protected readonly IUnitOfWork<Guid> UnitOfWork;

        protected RepositoryBase(IUnitOfWork<Guid> unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public int GetCount()
        {
            return UnitOfWork.GetCount<TPersistentObject>();
        }

        public virtual TPersistentObject GetByID(Guid objectID)
        {
            return UnitOfWork.Get<TPersistentObject>(objectID);
        }

        public virtual TPersistentObject GetFirst(Expression<Func<TPersistentObject, string>> sortExpression
            , bool ascending)
        {
            return UnitOfWork.GetFirst<TPersistentObject>(o => true
                , sortExpression
                , ascending);
        }

        public virtual TPersistentObject GetFirst(Expression<Func<TPersistentObject, bool>> queryExpression
            , Expression<Func<TPersistentObject, string>> sortExpression
            , bool ascending)
        {
            return UnitOfWork.GetFirst<TPersistentObject>(queryExpression
                , sortExpression
                , ascending);
        }

        public virtual IList<TPersistentObject> GetInOrder<TSortType>(Expression<Func<TPersistentObject, bool>> queryExpression
            , Expression<Func<TPersistentObject, TSortType>> sortExpression
            , bool ascending)
        {
            return UnitOfWork.GetAll(queryExpression
                , sortExpression
                , ascending);
        }

        protected IList<TPersistentObject> GetWhere(Expression<Func<TPersistentObject, bool>> queryExpression)
        {
            return UnitOfWork.GetAll<TPersistentObject>(queryExpression);
        }

        public virtual IList<TPersistentObject> GetAll()
        {
            IList<TPersistentObject> objList = UnitOfWork.GetAll<TPersistentObject>();
            return objList;
        }

        public virtual void Save(TPersistentObject obj)
        {
            UnitOfWork.AddToSession(obj);
        }

        public virtual void Update(TPersistentObject obj)
        {
            UnitOfWork.Update(obj);
        }

        public virtual void Delete(TPersistentObject obj)
        {
            UnitOfWork.Delete(obj);
        }
    }
}
