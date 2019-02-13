using Core.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace RuleLibrary.Operations
{
    public class GetPunctuationCount : Operation, IOperation
    {
        public override string OperationName => "количество знаков препинания";

        public override string ExecuteOperation(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            var text = File.ReadAllText(filePath);
            return string.Format(ResultPattern, filePath, PunctuationCount(text));
        }

        private int PunctuationCount(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));
            return text.Count(char.IsPunctuation);
        }
    }
}