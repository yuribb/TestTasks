using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup.Core.Interfaces
{
    public interface IRetention
    {
        IRetentionPolicy RetentionPolicy { get; set; }
        bool KeepAndClearBackups();
    }
}
