using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;

using Global.Utilities.ExtensionMethods;
using HyperQueryNH.Core;
using LikeMyDessert.Domain;
using LikeMyDessert.Repositories.Mappings.Configurations;
using LikeMyDessert.Repositories.Mappings.Conventions;
using LikeMyDessert.Repositories.Mappings.ClassMaps;

namespace LikeMyDessert.Repositories
{
	/// <summary>
	/// Abstract class used to administrate the caching and committing of persistant objects to a configured data source.
	/// This particular project contracts FluentNHibernate to fulfill its caching and persistence needs.
	/// This class binds implementations of FluentNHibernate to the project's repository layer.
	/// </summary>
	public static class PersistenceManager
	{
        private const string DATABASE_KEY = "LikeMyDessert";
		private static ISession _currentSession;

		static PersistenceManager()
		{
			var repoAssembly = typeof(PersistenceManager).Assembly;

			//TODO: Use optional parameters of FNHSession to dynamically construct configuration.
			#region Instantiating new FluentNHibernate Session

			FluentConfiguration config = Fluently.Configure()
				.Database(MsSqlConfiguration.MsSql2008.ConnectionString(c => c.FromConnectionStringWithKey(DATABASE_KEY)).ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssembly(repoAssembly)
				               	.Conventions.Add(GetConventions()));
	
			FNHSession.Init(config);
			_currentSession = FNHSession.CurrentSession;

			#endregion Instantiating new FluentNHibernate Session
		}

		/// <summary>
		/// Calls static constructor to setup a new instance.
		/// </summary>
		public static void Initilize() { }

        /// <summary>
        /// Initiates a session so that transient persistent objects can be cached
        /// </summary>
        public static void OpenSession()
        {
            //A session is created if one doesnot already exist
            var session = FNHSession.CurrentSession;
        }

		/// <summary>
		/// Returns the current object cache associated with this Persistence Manager.
		/// </summary>
		public static ISession CurrentObjectCache
		{
            get { return FNHSession.CurrentSession; }
		}

		/// <summary>
		/// Inserts, updates, and/or deletes all cached objects to data source.
		/// </summary>
		public static void CommitCachedObjects()
		{
			FNHSession.CommitSession();
		}

		/// <summary>
		/// Clears transient objects from cache.
		/// </summary>
		public static void ClearCache()
		{
			FNHSession.ClearSession();
		}

		/// <summary>
		/// Disposes persistant object cache and ends communication with data source.
		/// </summary>
		public static void DisposeCache()
		{
			FNHSession.CloseSession();
		}

		/// <summary>
		/// Cancels all persistant object transactions and clears cache.
		/// </summary>
		public static void RollbackCachedObjects()
		{
			FNHSession.TransactionFailed = true;
			FNHSession.CommitSession();
			FNHSession.CloseSession();

			var session = FNHSession.CurrentSession;
		}

		/// <summary>
		/// Sets rollback flage to true so that cached persistance objects are rolled back, not committed, on the next attempt to commit.
		/// </summary>
		public static bool RollbackCachedObjectsOnCommit
		{
			get { return FNHSession.TransactionFailed; }
			set { FNHSession.TransactionFailed = value; }
		}

		/// <summary>
		/// MADE FOR TESTING PURPOSES ONLY!!! Removes all foreign keys, then DELETES THE ENTIRE DATABASE!
		/// </summary>
		public static void DropDatabase()
		{
			FNHSession.DropAllTables();
		}

		/// <summary>
		/// DO NOT CALL UNLESS YOU INTEND TO RE-CREATE THE CURRENT DATABASE! Creates SQL Database based on current NHibernate Configuration.
		/// </summary>
		public static void CreateDatabase()
		{
			FNHSession.ExportModelToDatabase();
		}
		
		/// <summary>
		/// Gets the FluentNHibernate conventions used in this project.
		/// </summary>
		/// <returns></returns>
		private static FluentNHibernate.Conventions.IConvention[] GetConventions()
		{
			var conventions = new IConvention[] {
				Table.Is(t => "tbl" + t.EntityType.Name.Pluralize()),
				PrimaryKey.Name.Is(pk => "ID"),
				ForeignKey.EndsWith("ID"),
				DefaultAccess.Property(),
				DefaultCascade.SaveUpdate(),
				DefaultLazy.Always(),
				new CustomManyToManyTableNameConvention()
			};

			return conventions;
		}
	}
}