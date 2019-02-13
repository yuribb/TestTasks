using Core.Interfaces;
using System.Collections.Generic;

namespace RuleLibrary.Rules
{
    public class HTMLRule : Rule, IRule
    {
        const string FYLE_TYPE = "HTML";

        public HTMLRule(IList<IOperation> operations) : base(operations) { }

        public override string FileType { get => FYLE_TYPE; }
        public override bool ForAllTypes => false;
    }
}