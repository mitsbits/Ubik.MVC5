using System;
using System.Threading.Tasks;
using Ubik.Infra.DataManagement;

namespace Ubik.Web.Infra.Contracts
{
    public interface IErrorLogManager
    {
        Task LogException(Exception ex);

        Task<int> ClearLogs(DateTime startUtc, DateTime endUtc);

        Task<int> ClearLog(string id);

        Task<PagedResult<ErrorLog>> GetLogs(int pageNumer, int pageSize);
    }
}