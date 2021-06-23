using System.Collections.Generic;

namespace BTC.Services.Interfaces
{
    public interface IDataWorker<T>
    {
        void CreateTable();
        IEnumerable<T> ReadTable();
        void WriteTable(T row);
    }
}