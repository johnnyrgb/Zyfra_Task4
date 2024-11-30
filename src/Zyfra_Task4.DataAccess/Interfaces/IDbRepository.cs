using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zyfra_Task4.DataAccess.Entities;

namespace Zyfra_Task4.DataAccess.Interfaces
{
    public interface IDbRepository
    {
        IEntityRepository<DataEntry> DataEntry { get; set; }
    }
}
