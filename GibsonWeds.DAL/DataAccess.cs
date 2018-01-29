using GibsonWeds.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GibsonWeds.DAL
{
    public class DataAccess
    {
        public static bool runInDesktopMode = false;

        public static GibsonWedsEntities desktopModeMetadata = null;

        private const string DataContextKey = "GibsonWedsDataContextKey";

        public static void EF_Fix()
        {
            var ensureDLLIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        private static GibsonWedsEntities InternalDataContext
        {
            get
            {
                return (GibsonWedsEntities)HttpContext.Current.Items[DataContextKey];
            }

            set
            {
                HttpContext.Current.Items[DataContextKey] = value;
            }
        }

        internal static GibsonWedsEntities metadata
        {
            get
            {
                if (runInDesktopMode)
                {
                    // return from Thread Context
                    if (desktopModeMetadata == null)
                    {
                        desktopModeMetadata = new GibsonWedsEntities();
                        desktopModeMetadata.Configuration.LazyLoadingEnabled = false;
                    }
                    return desktopModeMetadata;
                }


                // If the context is missing, create a new one
                if (InternalDataContext == null)
                {
                    // Note: in a real app, this should get the connection string from secure storage and pass it to the context constructor.
                    //string connectionString = @"Data Source=(local)\sqlexpress;Initial Catalog=CaveatEmptor;Integrated Security=True;Pooling=False";
                    //InternalDataContext = new DataPropEntities(connectionString);
                    InternalDataContext = new GibsonWedsEntities();
                    InternalDataContext.Configuration.LazyLoadingEnabled = false;
                }

                return InternalDataContext;
            }
        }
        internal static GibsonWedsEntities getMetadata()
        {
            return metadata;
        }

        public static GibsonWedsEntities getDesktopMetadata()
        {
            var md = new GibsonWedsEntities();
            md.Configuration.LazyLoadingEnabled = false;
            return md;
        }

        public static void SaveChanges()
        {
            metadata.SaveChanges();
        }

        public static void CleanUp()
        {
            if (InternalDataContext != null)
            {
                InternalDataContext.Dispose();
                InternalDataContext = null;
            }
        }

        public static void CleanUpDesktopMode()
        {
            if (desktopModeMetadata != null)
            {
                desktopModeMetadata.Dispose();
                desktopModeMetadata = null;
            }
        }

        //public static bool LazyLoadingEnabled
        //{
        //    get { return DataAccess.metadata.ContextOptions.LazyLoadingEnabled = false; }
        //    set { DataAccess.metadata.ContextOptions.LazyLoadingEnabled = value; }
        //}

        private static string GetClause<TEntity>(IQueryable<TEntity> clause) where TEntity : class
        {
            string snippet = "FROM [dbo].[";

            string sql = ((ObjectQuery<TEntity>)clause).ToTraceString();
            string sqlFirstPart = sql.Substring(sql.IndexOf(snippet));

            sqlFirstPart = sqlFirstPart.Replace("AS [Extent1]", "");
            sqlFirstPart = sqlFirstPart.Replace("[Extent1].", "");

            return sqlFirstPart;
        }

        //public static int DeleteAll<TEntity>(IQueryable<TEntity> clause) where TEntity : class
        //{
        //    string sqlClause = GetClause<TEntity>(clause);
        //    return metadata.ExecuteStoreCommand(string.Format(System.Globalization.CultureInfo.InvariantCulture, "DELETE {0}", sqlClause));
        //}
    }
}
