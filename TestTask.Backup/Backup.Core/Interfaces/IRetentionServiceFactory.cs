using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup.Core.Interfaces
{
    public interface IRetentionServiceFactory
    {
        IList<IRetention> Retentions { get; set; }
    }
}
