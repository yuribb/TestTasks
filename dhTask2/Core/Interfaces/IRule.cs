using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRule
    {
        bool ForAllTypes { get; }
        string FileType { get; }

        IList<IOperation> Operations { get; }

        bool IsRuleApply(string filePath);

        string ApplyRule(string filePath);
    }
}
