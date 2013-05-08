using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using NHibernate;

namespace HyperQueryNH.Core
{
    public interface IUnitOfWork
    {
        void Reconnect();
        void Disconnect();

        void Initialize(Object mappedObject);
        
        void AddToSession(object mappedObject);
        void Update(object mappedObject);
        void Delete(object mappedObject);

        TLibraryObject Get<TLibraryObject>(Guid id);
        TLibraryObject Get<TLibraryObject>(Expression<Func<TLibraryObject, bool>> queryExpression);
        TLibraryObject GetFirst<TLibraryObject>(Expression<Func<TLibraryObject, bool>> queryExpression
            , Expression<Func<TLibraryObject, string>> sortExpression
            , bool ascending);
        
        IList<TLibraryObject> GetAll<TLibraryObject>();
        IList<TLibraryObject> GetAll<TLibraryObject>(Expression<Func<TLibraryObject, bool>> queryExpression);
        IList<TLibraryObject> GetAll<TLibraryObject>(Expression<Func<TLibraryObject, string>> sortExpression
            , bool ascending);
        IList<TLibraryObject> GetAll<TLibraryObject>(Expression<Func<TLibraryObject, bool>> queryExpression
            , Expression<Func<TLibraryObject, string>> sortExpression
            , bool ascending);

        IQuery CreateFilter(object collection, string queryString);
    }
}
