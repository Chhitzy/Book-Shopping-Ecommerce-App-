using Dapper;
using Ecommerce_App.Data;
using Ecommerce_App.DataAccess.Repository.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_App.DataAccess.Repository

{
   
    public class SP_CALL : ISP_CALL
    {
        private readonly ApplicationDbContext _context;
        private static string ConnectionString = "";
        public SP_CALL(ApplicationDbContext context)
        { 
            _context = context;
            ConnectionString = _context.Database.GetDbConnection().ConnectionString;
        }
        public void Dispose()
        {
            _context.Dispose();  
        }

        public void Execute(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection SqlCon = new SqlConnection(ConnectionString))
            {
                SqlCon.Open();
                SqlCon.Execute(procedureName, param, commandType: CommandType.StoredProcedure);

            }
        }

        public IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection SqlCon = new SqlConnection(ConnectionString))
            {
                SqlCon.Open();
                return SqlCon.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);
            };
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection Sql = new SqlConnection(ConnectionString))
            {
                Sql.Open();
                var result = Sql.QueryMultiple(procedureName, param, commandType: CommandType.StoredProcedure);
                var item_1 = result.Read<T1>();
                var item_2 = result.Read<T2>();
                if(item_1 != null && item_2 != null)
                {
                    return new Tuple<IEnumerable<T1>,IEnumerable<T2>>(item_1, item_2);
                }
                else
                {
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
                }

            };
        }

        public T OneRecord<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection Sql = new SqlConnection(ConnectionString))
            {
                Sql.Open();
                var value = Sql.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);
                return value.FirstOrDefault();
            };
        }

        public T Single<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection Sql = new SqlConnection(ConnectionString))
            {
                Sql.Open();
               
                return Sql.ExecuteScalar<T>(procedureName, param, commandType: CommandType.StoredProcedure);
            };
        }

        
        public IEnumerable<TFirst> ListWithRelated<TFirst, TSecond>(
    string procedureName,
    Func<TFirst, TSecond, TFirst> map,
    string splitOn,
    DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(ConnectionString))
            {
                sqlCon.Open();
                return sqlCon.Query<TFirst, TSecond, TFirst>(
                    procedureName,
                    map,
                    param,
                    commandType: CommandType.StoredProcedure,
                    splitOn: splitOn
                );
            }
        }


    }

}
