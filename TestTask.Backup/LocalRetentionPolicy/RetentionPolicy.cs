using Backup.Core.Interfaces;
using Backup.Core.Model;
using System;

namespace LocalRetentionPolicy
{
    [Serializable]
    public class RetentionPolicy : IRetentionPolicy
    {
        private class ErrorCodes
        {
            public const string INCORRECT_NUM_OF_COPIES_FORMAT = "Incorrect {0} num of copies";
            public const string INCORRECT_NUM_OF_DAYS_FORMAT = "Incorrect {0} num of days";
        }

        public int NumOfCopies { get; set; }
        public int MinNumOfDays { get; set; }
        public int MaxNumOfDays { get; set; }

        public RetentionPolicy(RetentionPolicyConfiguration retentionPolicyConfiguration)
        {
            if (retentionPolicyConfiguration == null) throw new ArgumentNullException(nameof(retentionPolicyConfiguration));
            if (retentionPolicyConfiguration.NumOfCopies < 0) throw new ArgumentException(string.Format(ErrorCodes.INCORRECT_NUM_OF_COPIES_FORMAT, retentionPolicyConfiguration.NumOfCopies), nameof(retentionPolicyConfiguration.NumOfCopies));

            NumOfCopies = retentionPolicyConfiguration.NumOfCopies;
            MinNumOfDays = retentionPolicyConfiguration.MinNumOfDays; //3 //14
            MaxNumOfDays = retentionPolicyConfiguration.MaxNumOfDays; //7 //infinite
        }

        public RetentionPolicy(int numOfCopies, int minDays, int maxDays = -1)
        {
            if (numOfCopies < 0) throw new ArgumentException(string.Format(ErrorCodes.INCORRECT_NUM_OF_COPIES_FORMAT, numOfCopies), nameof(numOfCopies));

            NumOfCopies = numOfCopies;
            MinNumOfDays = minDays; //3 //14
            MaxNumOfDays = maxDays; //7 //infinite
        }


        /// <summary>
        /// old date
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public DateTime GetMinDate(DateTime now)
        {
            if (MaxNumOfDays >= 0)
            {
                DateTime minDate = now.Date.AddDays(0 - MaxNumOfDays).Date;
                return minDate;
            }
            return DateTime.MinValue;
        }

        /// <summary>
        /// New date
        /// </summary>
        /// <param name="now"></param>
        /// <returns></returns>
        public DateTime GetMaxDate(DateTime now)
        {
            if (MinNumOfDays >= 0)
            {
                DateTime maxDate = now.Date.AddDays(0 - MinNumOfDays).Date;
                return maxDate;
            }
            return now.Date;
        }

        /// <summary>
        /// Date pairs
        /// </summary>
        public MinMaxDate MinMaxDate
        {
            get
            {
                DateTime now = DateTime.Now.Date;
                return new MinMaxDate(GetMinDate(now), GetMaxDate(now));
            }
        }

        public override string ToString()
        {
            if (MaxNumOfDays < 0) return $"keep {NumOfCopies} backup older than {MinNumOfDays} days";
            return $"keep no more than {NumOfCopies} backups {MinNumOfDays}-{MaxNumOfDays} days old";
        }
    }
}