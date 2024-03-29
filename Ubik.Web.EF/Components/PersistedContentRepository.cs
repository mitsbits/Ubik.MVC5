﻿using Mehdime.Entity;
using Ubik.EF;

namespace Ubik.Web.EF.Components
{
    internal class PersistedContentRepository : ReadWriteRepository<PersistedContent, ComponentsDbContext>
    {
        public PersistedContentRepository(IAmbientDbContextLocator ambientDbContextLocator)
            : base(ambientDbContextLocator)
        {
        }
    }
}