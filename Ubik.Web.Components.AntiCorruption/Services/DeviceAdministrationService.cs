using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Mehdime.Entity;
using Ubik.Infra.Contracts;
using Ubik.Infra.DataManagement;
using Ubik.Web.Components.Contracts;
using Ubik.Web.Components.Domain;
using Ubik.Web.EF.Components;

namespace Ubik.Web.Components.AntiCorruption.Services
{
    public class DeviceAdministrationService : IDeviceAdministrationService<int>
    {

        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly ICRUDRespoditory<PersistedDevice> _persistedDeviceRepo;

        public DeviceAdministrationService(IDbContextScopeFactory dbContextScopeFactory, ICRUDRespoditory<PersistedDevice> persistedDeviceRepo)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _persistedDeviceRepo = persistedDeviceRepo;
        }


        public async Task<PagedResult<Device<int>>> All(int pageNumber, int pageSize)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {


                var result = await _persistedDeviceRepo.FindAsync(x => true,
                       new[] { new OrderByInfo<PersistedDevice>() { Ascending = true, Property = x => x.FriendlyName } },
                       pageNumber, pageSize);

                var output = new PagedResult<Device<int>>
                {
                    PageNumber = result.PageNumber,
                    PageSize = result.PageSize,
                    TotalPages = result.TotalPages,
                    TotalRecords = result.TotalRecords,
                    Data = new List<Device<int>>(result.Data.Select(Mapper.MapToDomain))
                };
                return output;
            }
        }

        public async Task<Device<int>> Get(int id)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {

                var entity = await _persistedDeviceRepo.GetAsync(x => x.Id == id, x => x.Sections);
                return Mapper.MapToDomain(entity);
            }
        }
    }
}
