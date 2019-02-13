using Core.Interfaces;
using System.Collections.Generic;

namespace RuleLibrary.Rules
{
    public class CSSRule : Rule, IRule
    {
        const string FYLE_TYPE = "CSS";
        public CSSRule(IList<IOperation> operations) : base(operations) { }

        public override string FileType => FYLE_TYPE;
        public override bool ForAllTypes => false;
    }
}