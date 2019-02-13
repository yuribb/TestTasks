using Core.Interfaces;
using System;
using System.IO;
using System.Linq;
namespace RuleLibrary.Operations
{
    public class GetBracesCount : Operation, IOperation
    {
        public override string OperationName => "количество открывающих скобок \"{{\" совпадает с количеством закрывающих скобок \"}}\"";

        public override string ExecuteOperation(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));
            var text = File.ReadAllText(filePath);
            string result = text.Count(c => c == '{') == text.Count(c => c == '}') ? "да" : "нет";
            return string.Format(ResultPattern, filePath, result);
        }
    }
}
