using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;

using HyperQueryNH.Core.Enums;
using Global.FluentNHibernateUtilities.Enums;

namespace Global.FluentNHibernateUtilities
{
    public static class FNHSession
    {
        private const string NHibernateSessionKey = "__NHibernateSession";

        private static ISession _session;
        private static ISessionFactory _sessionFactory;
		private static IPersistenceConfigurer _databaseConfig;
		private static Configuration _nHibernateConfiguration;
		private static FluentConfiguration _sessionFactoryConfiguration;

		//TODO:  Use overrides to wire up Fluent Automapping to work.
		public static void Initialize(IPersistenceConfigurer databaseConfiguration
			, Assembly classMapAssembly
			, Assembly autoMapAssembly = null
			, Assembly autoMappingOverrideAssembly = null
			, object baseClass = null
			, Type[] otherClassObjectsToMap = null
			, bool ignoreBase = false
			, SessionDefaultCascadeMode defaultCascade = SessionDefaultCascadeMode.SaveUpdate
			, bool defaultLazyLoad = true)
        {

		}

		public static void Initialize(FluentConfiguration sessionFactoryconfiguration)
		{
			_sessionFactoryConfiguration = sessionFactoryconfiguration;
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
                        throw;
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
            ISession newSession = CurrentSessionFactory.OpenSession();

            if (newSession.IsConnected)
            {
                newSession.Disconnect();
            }

            return newSession;
        }

        public static bool TransactionFailed { get; set; }

    	private static ISessionFactory CurrentSessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = CreateSessionFactory();
                }

                return _sessionFactory;
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
			if (_sessionFactoryConfiguration != null)
			{
				_nHibernateConfiguration = _sessionFactoryConfiguration
					.BuildConfiguration();
			}
			else
			{
				throw new NotImplementedException();
			}

			#region Sample Configurations
			//return Fluently.Configure()
			//    .Database(GetSqlConfiguration())
			//    .Mappings(m =>
			//    {
			//        m.FluentMappings.AddFromAssembly(AssemblyToMap);
			//        m.AutoMappings.Add(
			//            AutoMap.Assemblies(AssemblyToMap)
			//                .Where(t => t.IsSubclassOf(BaseClassOfTypeToMap) || OtherTypesToMap.Contains(t))
			//                .UseOverridesFromAssembly(AssemblyToMap)
			//                .IgnoreBase(BaseClassOfTypeToMap)
			//                .Conventions.Add(
			//                    DefaultCascade.SaveUpdate(),
			//                    DefaultLazy.Always()
			//                )
			//            )
			//            .ExportTo(Path.GetTempPath());
			//    })
			//    .BuildConfiguration();
			#endregion Sample Configurations

			ISessionFactory newfactory = _nHibernateConfiguration.BuildSessionFactory();

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
		/// MADE FOR TESTING PURPOSES ONLY!!! Removes all foreign keys, then DELETES THE ENTIRE DATABASE!
		/// </summary>
		public static void DropAllTables()
		{
			if (_nHibernateConfiguration == null)
			{
				throw new NullReferenceException("The current NHibernate Configuration has not been set.");
			}

			Reconnect();

			IDbCommand command = CurrentSession.Connection.CreateCommand();
			command.CommandText = @"
				/* Drop all Foreign Key constraints */
				DECLARE @name VARCHAR(128)
				DECLARE @constraint VARCHAR(254)
				DECLARE @SQL VARCHAR(254)

				SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' ORDER BY TABLE_NAME)

				WHILE @name is not null
				BEGIN
					SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME)
					WHILE @constraint IS NOT NULL
					BEGIN
						SELECT @SQL = 'ALTER TABLE [dbo].[' + RTRIM(@name) +'] DROP CONSTRAINT ' + RTRIM(@constraint)
						EXEC (@SQL)
						PRINT 'Dropped FK Constraint: ' + @constraint + ' on ' + @name
						SELECT @constraint = (SELECT TOP 1 CONSTRAINT_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' AND CONSTRAINT_NAME <> @constraint AND TABLE_NAME = @name ORDER BY CONSTRAINT_NAME)
					END
				SELECT @name = (SELECT TOP 1 TABLE_NAME FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS WHERE constraint_catalog=DB_NAME() AND CONSTRAINT_TYPE = 'FOREIGN KEY' ORDER BY TABLE_NAME)
				END";

			using (ITransaction tx = CurrentSession.BeginTransaction())
			{
				try
				{
					tx.Enlist(command);
					command.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					tx.Rollback();
					throw;
				}
			}

			Disconnect();

			new SchemaExport(_nHibernateConfiguration).Drop(false, true);
		}

		/// <summary>
		/// DO NOT CALL UNLESS YOU INTEND TO RECREATE THE CURRENT DATABASE! Creates SQL Database based on current NHibernate Configuration.
		/// Extrapolates Business Model to Relational Database Tables on server declared in 'hibernate-configuration' element of config file.
		/// </summary>
		public static void ExportToDatabase()
		{
			if (_nHibernateConfiguration == null)
			{
				throw new NullReferenceException("The current NHibernate Configuration has not been set.");
			}

			new SchemaExport(_nHibernateConfiguration).Create(false, true);
		}
	}
}
