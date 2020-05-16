using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksDAL.Repository.IRepository
{
    public interface IStoredProcedureCall : IDisposable
    {
        // Gets the 1st column of 1st row only (1 cell)
        T Single<T>(string procedureName, DynamicParameters parameters = null);

        // Gets a complete row
        T OneRecord<T>(string procedureName, DynamicParameters parameters = null);

        // Gets a complete table
        IEnumerable<T> List<T>(string procedureName, DynamicParameters parameters = null);

        // Gets a combination of 2 tables
        Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters parameters = null);

        void Execute(string procedureName, DynamicParameters parameters = null);
    }
}
