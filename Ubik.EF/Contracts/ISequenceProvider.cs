using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ubik.EF.Contracts
{
    public interface ISequenceBase { }

    public interface ISequenceProvider
    {
        void Next(DbEntityEntry entry);
    }
}
