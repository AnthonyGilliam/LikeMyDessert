using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Interfaces;

namespace LikeMyDessert.Services
{
	public abstract class ServiceBase<TPersistentObject, TRepository>
		where TPersistentObject : PersistentObject
        where TRepository : IRepository<TPersistentObject>
	{
		protected readonly TRepository Repository;

	    protected ServiceBase(TRepository repository)
		{
			Repository = repository;
		}

		public TPersistentObject GetByID(Guid objectID)
		{
			return Repository.GetByID(objectID);
		}

		public IList<TPersistentObject> GetAll()
		{
			return Repository.GetAll();
		}

		public TPersistentObject GetFirst(Expression<Func<TPersistentObject, bool>> queryExpression, Expression<Func<TPersistentObject, string>> sortExpression, bool ascending)
		{
			throw new NotImplementedException();
		}

		public TPersistentObject GetFirst(Expression<Func<TPersistentObject, string>> sortExpression, bool ascending)
		{
			throw new NotImplementedException();
		}

		public IList<TPersistentObject> GetInOrder(Expression<Func<TPersistentObject, bool>> queryExpression, Expression<Func<TPersistentObject, string>> sortExpression, bool ascending)
		{
			throw new NotImplementedException();
		}

		public void Save(TPersistentObject obj)
		{
			Repository.Save(obj);
		}

		public void Update(TPersistentObject obj)
		{
			Repository.Update(obj);
		}

		public void Delete(TPersistentObject obj)
		{
			Repository.Delete(obj);
		}

	}
}
