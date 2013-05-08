using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using NHibernate;
using NHibernate.Linq;

namespace HyperQueryNH.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        public ISession _session;

        #region IUnitOfWork<Guid> Members

        public UnitOfWork(ISession currentNHibernateSession)
        {
			_session = currentNHibernateSession;
        }

        public void Reconnect()
        {
            if (!_session.IsConnected)
            {
                _session.Reconnect();
            }
        }

        public void Disconnect()
        {
            if (_session.IsConnected)
            {
                _session.Disconnect();
            }
        }

        public void Initialize(Object mappedObject)
        {
            Reconnect();

            if (!NHibernateUtil.IsInitialized(mappedObject))
            {
                NHibernateUtil.Initialize(mappedObject);
            }

            //Disconnect();
        }

        public void AddToSession(object mappedObject)
        {
            Reconnect();

            _session.Save(mappedObject);
            
            //Disconnect();
        }

        public void Update(object mappedObject)
        {
            Reconnect();

            _session.SaveOrUpdate(mappedObject);
            
            //Disconnect();
        }

        public void Delete(object mappedObject)
        {
            Reconnect();

            _session.Delete(mappedObject);

            //Disconnect();
        }

        public void Evict(object mappedObject)
        {
            Reconnect();

            _session.Evict(mappedObject);

            //Disconnect();
        }

        public TDomainObject Get<TDomainObject>(Guid id)
        {
            Reconnect();

            TDomainObject obj = _session.Get<TDomainObject>(id);

            //Disconnect();

            return obj;
        }

        public TDomainObject Get<TDomainObject>(Expression<Func<TDomainObject, bool>> queryExpression)
        {
            Reconnect();

            TDomainObject obj = _session.Query<TDomainObject>().Where(queryExpression).SingleOrDefault();

            //Disconnect();

            return obj;
        }

        public TDomainObject GetFirst<TDomainObject>(Expression<Func<TDomainObject, bool>> queryExpression
            , Expression<Func<TDomainObject, string>> sortExpression
            , bool ascending)
        {
            Reconnect();

            TDomainObject obj = ascending == true
                ?  _session.Query<TDomainObject>()
                    .Where(queryExpression)
                    .OrderBy(sortExpression)
                    .FirstOrDefault()
                :  _session.Query<TDomainObject>()
                    .Where(queryExpression)
                    .OrderByDescending(sortExpression)
                    .FirstOrDefault();

            //Disconnect();

            return obj;
        }

        public IList<TDomainObject> GetAll<TDomainObject>()
        {
            Reconnect();

        	IList<TDomainObject> objList = _session.Query<TDomainObject>().ToList();

            //Disconnect();

            return objList;
        }

        public IList<TDomainObject> GetAll<TDomainObject>(Expression<Func<TDomainObject, bool>> queryExpression)
        {
            Reconnect();

            IList<TDomainObject> objList =  _session.Query<TDomainObject>().Where(queryExpression).ToList();

            //Disconnect();

            return objList;
        }

        public IList<TDomainObject> GetAll<TDomainObject>(Expression<Func<TDomainObject, string>> sortExpression
            , bool ascending)
        {
            Reconnect();

            IList<TDomainObject> objList = ascending == true
                ? _session.Query<TDomainObject>()
                    .OrderBy(sortExpression)
                    .ToList()
                : _session.Query<TDomainObject>()
                    .OrderByDescending(sortExpression)
                    .ToList();

            //Disconnect();

            return objList;
        }

        public IList<TDomainObject> GetAll<TDomainObject>(Expression<Func<TDomainObject, bool>> queryExpression
            , Expression<Func<TDomainObject, string>> sortExpression
            , bool ascending)
        {
            Reconnect();

            IList<TDomainObject> objList = ascending == true
                ? _session.Query<TDomainObject>()
                    .Where(queryExpression)
                    .OrderBy(sortExpression)
                    .ToList()
                : _session.Query<TDomainObject>()
                    .Where(queryExpression)
                    .OrderByDescending(sortExpression)
                    .ToList();

            //Disconnect();

            return objList;
        }

        public IQuery CreateFilter(object collection, string queryString)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
