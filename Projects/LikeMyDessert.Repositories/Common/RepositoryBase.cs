using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HyperQueryEF.Core;
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
        protected readonly IUnitOfWork UnitOfWork;

        protected RepositoryBase(IUnitOfWork unitOfWork)
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

        public virtual TPersistentObject GetFirst(Expression<Func<TPersistentObject, string>> sortExpression , bool ascending)
        {
            return ascending 
                ? UnitOfWork.GetAll<TPersistentObject>()
                    .OrderBy(sortExpression)
                    .FirstOrDefault()
                : UnitOfWork.GetAll<TPersistentObject>()
                    .OrderByDescending(sortExpression)
                    .FirstOrDefault();
        }

        public virtual TPersistentObject GetFirst(Expression<Func<TPersistentObject, bool>> queryExpression
            , Expression<Func<TPersistentObject, string>> sortExpression
            , bool ascending)
        {
            return ascending
                ? UnitOfWork.GetAll<TPersistentObject>()
                    .OrderBy(sortExpression)
                    .FirstOrDefault(queryExpression)
                : UnitOfWork.GetAll<TPersistentObject>()
                    .OrderByDescending(sortExpression)
                    .FirstOrDefault(queryExpression);
        }

        public virtual IList<TPersistentObject> GetInOrder<TSortType>(Expression<Func<TPersistentObject, bool>> queryExpression
            , Func<TPersistentObject, TSortType> sortExpression
            , bool ascending)
        {
            return ascending
                ? UnitOfWork.GetAll<TPersistentObject>(queryExpression)
                    .OrderBy(sortExpression)
                    .ToList()
                : UnitOfWork.GetAll<TPersistentObject>(queryExpression)
                    .OrderByDescending(sortExpression)
                    .ToList();
        }

        protected IList<TPersistentObject> GetWhere(Expression<Func<TPersistentObject, bool>> queryExpression)
        {
            return UnitOfWork.GetAll(queryExpression).ToList();
        }

        public virtual IList<TPersistentObject> GetAll()
        {
            return UnitOfWork.GetAll<TPersistentObject>().ToList();
        }

        public virtual void Save(TPersistentObject obj)
        {
            UnitOfWork.Save(obj);
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
