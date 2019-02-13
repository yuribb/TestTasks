using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleLibrary.Rules
{
    public class CommonRule : Rule, IRule
    {
        public CommonRule(IList<IOperation> operations) : base(operations) { }

        public override bool ForAllTypes => true;

        public override string FileType => null;

    }
}