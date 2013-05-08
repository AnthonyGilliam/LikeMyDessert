using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using LikeMyDessert.Domain;

namespace LikeMyDessert.Services.Interfaces
{
	public interface IService<TPersistentObject> 
		where TPersistentObject : PersistentObject
	{
		TPersistentObject GetByID(Guid objectID);

		IList<TPersistentObject> GetAll();

		TPersistentObject GetFirst(Expression<Func<TPersistentObject, bool>> queryExpression
			, Expression<Func<TPersistentObject, string>> sortExpression
			, bool ascending);

		TPersistentObject GetFirst(Expression<Func<TPersistentObject, string>> sortExpression
			, bool ascending);

		IList<TPersistentObject> GetInOrder(Expression<Func<TPersistentObject, bool>> queryExpression
			, Expression<Func<TPersistentObject, string>> sortExpression
			, bool ascending);

		void Save(TPersistentObject obj);

		void Update(TPersistentObject obj);

		void Delete(TPersistentObject obj);
	}
}
