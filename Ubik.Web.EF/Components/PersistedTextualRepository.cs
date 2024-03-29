﻿using Mehdime.Entity;
using Ubik.EF;
using Ubik.Web.EF.Components.Contracts;

namespace Ubik.Web.EF.Components
{
    public class PersistedTextualRepository : ReadWriteRepository<PersistedTextual, ComponentsDbContext>,
        IPersistedTextualRepository
    {
        public PersistedTextualRepository(IAmbientDbContextLocator ambientDbContextLocator) : base(ambientDbContextLocator)
        {
        }
    }
}