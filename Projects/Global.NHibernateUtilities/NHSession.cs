using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

using HyperQueryNH.Core.Enums;

namespace HyperQueryNH.Core
{
    public sealed class NHSession
    {
        private const string NHibernateSessionKey = "__NHibernateSession";
        
        private static Assembly _businessModel;

        private static ISession _session;
        
        private static ISessionFactory _sessionFactory;

        public NHSession() { }

        public NHSession(Assembly businessModel)
        {
            _businessModel = businessModel;
        }

        public static ISession CurrentSession
        {
            get 
            {
                switch (Environment)
                {
                    case PlatformEnvironment.Desktop:
                        if (_session == null)
                        {
                            _session = CreateSession();
                        }

                        return _session;

                    case PlatformEnvironment.Browser:
                        ISession requestSession = HttpContext.Current.Items[NHibernateSessionKey] as ISession;

                        if (requestSession == null)
                        {
                            requestSession = CreateSession();
                            HttpContext.Current.Items[NHibernateSessionKey] = requestSession;
                        }

                        return requestSession;

                    default :
                        goto case PlatformEnvironment.Desktop;
                }
            }
        }

        public static void CommitSession()
        {
            if (CurrentSession != null && CurrentSession.IsOpen)
            {
                Reconnect();

                using (ITransaction tx = CurrentSession.BeginTransaction())
                {
                    try
                    {
                        if (!TransactionFailed)
                        {
                            CurrentSession.Flush();
                            tx.Commit();
                        }
                        else
                        {
                            tx.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        Disconnect();
                    }
                }
            }
        }

        public static void CloseSession()
        {
            if (_session != null)
            {
                if (_session.IsOpen)
                {
                    _session.Clear();
                    _session.Close();
                }

                _session.Dispose();
            }

            _session = null;
        }

        public static ISession CreateSession()
        {
            ISession newSession = CurrentFactory.OpenSession();

            if (newSession.IsConnected)
            {
                newSession.Disconnect();
            }

            return newSession;
        }

        public static bool TransactionFailed { get; set; }

        private static ISessionFactory CurrentFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = CreateFactory();
                }

                return _sessionFactory;
            }
        }

        private static ISessionFactory CreateFactory()
        {
            Configuration config = new Configuration();

            if (_businessModel != null)
            {
                config.AddAssembly(_businessModel);
            }

            config.Configure();

            #region Compose SQL Database
            //DONOT UN-COMMENT UNLESS BUILDING/REBUILDING DATABASE!
            //ExportBusinessModelSchemaToDatabase(config);
            #endregion Compose SQL Database

            ISessionFactory newfactory = config.BuildSessionFactory();

            return newfactory;
        }

        private static PlatformEnvironment Environment
        {
            get
            {
                return System.Web.HttpContext.Current == null
                    ? PlatformEnvironment.Desktop
                    : PlatformEnvironment.Browser;
            }
        }
        
        private static void Reconnect()
        {
            if (!CurrentSession.IsConnected)
            {
                CurrentSession.Reconnect();
            }
        }

        private static void Disconnect()
        {
            switch (Environment)
            {
                case PlatformEnvironment.Desktop:
                    if (_session != null)
                    {
                        _session.Disconnect();
                    }
                    break;

                case PlatformEnvironment.Browser:
                    ISession requestSession = HttpContext.Current.Items[NHibernateSessionKey] as ISession;

                    if (requestSession != null)
                    {
                        requestSession.Disconnect();
                    }
                    break;

                default:
                    goto case PlatformEnvironment.Desktop;
            }
        }

        /// <summary>
        /// Extrapolates Business Model to Relational Database Tables on server declared in 'hibernate-configuration' element of config file.
        /// </summary>
        /// <param name="config"></param>
        private static void ExportBusinessModelSchemaToDatabase(NHibernate.Cfg.Configuration config)
        {
            new SchemaExport(config).Create(false, true);
        }
    }
}
