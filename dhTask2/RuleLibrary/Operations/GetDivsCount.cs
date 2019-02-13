using Core.Interfaces;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace RuleLibrary.Operations
{
    public class GetDivsCount : Operation, IOperation
    {
        public override string OperationName => "количество тегов div";

        public override string ExecuteOperation(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            var text = File.ReadAllText(filePath);
            return string.Format(ResultPattern, filePath, DivCount(text));
        }

        private int DivCount(string text)
        {
            Regex regex = new Regex(@"<div>(\w*)");
            MatchCollection matches = regex.Matches(text);
            return matches.Count;
        }
    }
}