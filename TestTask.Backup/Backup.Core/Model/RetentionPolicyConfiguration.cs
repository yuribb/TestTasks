using System;

namespace Backup.Core.Model
{
    [Serializable]
    public class RetentionPolicyConfiguration
    {
        public int NumOfCopies { get; set; }
        public int MinNumOfDays { get; set; }
        public int MaxNumOfDays { get; set; }

        public RetentionPolicyConfiguration() { }

        public RetentionPolicyConfiguration(int numOfCopies, int minDays, int maxDays = -1)
        {
            NumOfCopies = numOfCopies;
            MinNumOfDays = minDays;
            MaxNumOfDays = maxDays;
        }
    }
}