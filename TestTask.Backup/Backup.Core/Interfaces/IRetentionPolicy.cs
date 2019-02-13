using Backup.Core.Model;
using System;

namespace Backup.Core.Interfaces
{
    public interface IRetentionPolicy
    {
        MinMaxDate MinMaxDate { get; }
        int NumOfCopies { get; set; }
        int MinNumOfDays { get; set; }
        int MaxNumOfDays { get; set; }

    }
}