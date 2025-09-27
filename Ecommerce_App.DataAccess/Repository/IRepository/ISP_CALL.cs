using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce_App.DataAccess.Repository.IRepository
{
    public  interface ISP_CALL:IDisposable
    {
        void Execute(String procedureName, DynamicParameters param = null);
        T Single<T>(String procedureName, DynamicParameters param = null);
        T OneRecord<T>(String procedureName, DynamicParameters param = null);
        IEnumerable<T> List<T>(String procedureName, DynamicParameters param = null);
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(String procedureName, DynamicParameters param = null);

     

        public IEnumerable<TFirst> ListWithRelated<TFirst, TSecond>(
    string procedureName,
    Func<TFirst, TSecond, TFirst> map,
    string splitOn,
    DynamicParameters param = null);
    }
}
