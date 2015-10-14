using Mehdime.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Ubik.Infra.Contracts;
using Ubik.Infra.DataManagement;
using Ubik.Web.Components.Contracts;
using Ubik.Web.Components.Domain;
using Ubik.Web.Components.ViewModels;
using Ubik.Web.EF.Components;

namespace Ubik.Web.Components.AntiCorruption.Services
{
    public class DeviceAdministrationService : IDeviceAdministrationService<int>, IDeviceAdministrationViewModelService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly ICRUDRespoditory<PersistedDevice> _persistedDeviceRepo;

        private readonly IViewModelBuilder<Device<int>, DeviceViewModel> _deviceBuilder;

        public DeviceAdministrationService(IDbContextScopeFactory dbContextScopeFactory, ICRUDRespoditory<PersistedDevice> persistedDeviceRepo)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _persistedDeviceRepo = persistedDeviceRepo;

            _deviceBuilder = new DeviceViewModelBuilder();
        }

        #region IDeviceAdministrationService

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

        #endregion IDeviceAdministrationService

        #region IDeviceAdministrationViewModelService

        public async Task<DeviceViewModel> DeviceModel(int id)
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var data = await _persistedDeviceRepo.GetAsync(x => x.Id == id) ?? new PersistedDevice();
                var entity = Mapper.MapToDomain(data);
                var model = _deviceBuilder.CreateFrom(entity);
                _deviceBuilder.Rebuild(model);
                return model;
            }
        }

        public async Task<IEnumerable<DeviceViewModel>> DeviceModels()
        {
            using (_dbContextScopeFactory.CreateReadOnly())
            {
                var data = await _persistedDeviceRepo.GetQuery().Include(x => x.Sections).ToListAsync();
                var bucket = new List<DeviceViewModel>();
                foreach (var persistedDevice in data)
                {
                    var entity = Mapper.MapToDomain(persistedDevice);
                    var model = _deviceBuilder.CreateFrom(entity);
                    _deviceBuilder.Rebuild(model);
                    bucket.Add(model);
                    ;
                }
                return bucket;
            }
        }

        #endregion IDeviceAdministrationViewModelService
    }
}