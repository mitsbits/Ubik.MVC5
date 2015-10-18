using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Mehdime.Entity;
using Ubik.Infra.Contracts;
using Ubik.Infra.DataManagement;
using Ubik.Web.EF;
using Ubik.Web.Infra;
using Ubik.Web.Infra.Contracts;

namespace Ubik.Web.Backoffice
{
    public class ErrorLogManager : IErrorLogManager
    {
        private readonly ICRUDRespoditory<PersistedExceptionLog> _repo;
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        public ErrorLogManager(ICRUDRespoditory<PersistedExceptionLog> repo, IDbContextScopeFactory dbContextScopeFactory)
        {
            _repo = repo;
            _dbContextScopeFactory = dbContextScopeFactory;
        }


        public async Task<int> ClearLogs(DateTime startUtc, DateTime endUtc)
        {
            using (var db = _dbContextScopeFactory.CreateWithTransaction(IsolationLevel.ReadCommitted))
            {
                var data = await _repo.FindAsync(x => x.TimeUtc >= startUtc && x.TimeUtc <= endUtc, null, 1, 1000000);
                foreach (var persistedExceptionLog in data.Data.Select(x => x.ErrorId).ToList())
                {
                    var log = persistedExceptionLog;
                    await _repo.DeleteAsync(x => x.ErrorId == log);
                }
                return await db.SaveChangesAsync();
            }
        }

        public async Task<PagedResult<ErrorLog>> GetLogs(int pageNumer, int pageSize)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var data = await _repo.FindAsync(x => true,
                    new[] { new OrderByInfo<PersistedExceptionLog>() { Ascending = false, Property = l => l.TimeUtc } },
                    pageNumer, pageSize);
                return new PagedResult<ErrorLog>()
                {
                    Data = data.Data.Select(MapToDomain),
                    PageNumber = data.PageNumber,
                    PageSize = data.PageSize,
                    TotalPages = data.TotalPages,
                    TotalRecords = data.TotalRecords
                };
            }
        }

        public Task LogException(Exception ex)
        {

            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            return Task.FromResult(false);

        }

        private static ErrorLog MapToDomain(PersistedExceptionLog data)
        {
            return new ErrorLog() { ErrorCode = data.StatusCode, ErrorDatetTimeUtc = data.TimeUtc, ErrorMessage = data.Message, ErrorType = data.Type, Host = data.Host, Id = data.ErrorId.ToString(), User = data.User };

        }
    }
}
