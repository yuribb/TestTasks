using Core;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RuleLibrary.Rules
{
    public abstract class Rule : IRule
    {
        public Rule(IList<IOperation> operations)
        {
            Operations = operations;
        }
        public abstract bool ForAllTypes { get; }
        public IList<IOperation> Operations { get; }
        public abstract string FileType { get; }

        public string ApplyRule(string filePath)
        {
            StringBuilder result = new StringBuilder();
            foreach(IOperation operation in Operations)
            {
                string res;
                try
                {
                    res = operation.ExecuteOperation(filePath);
                }
                catch(Exception ex)
                {
                    Logger.Error($"Can't execute operation '{operation}' to file '{filePath}'", ex);
                    throw;
                }
                
                if (!string.IsNullOrWhiteSpace(res))
                {
                    result.AppendLine(res);
                }
            }
            return result.ToString();
        }

        public virtual bool IsRuleApply(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return false;
            }

            if (!File.Exists(filePath))
            {
                return false;
            }

            if (ForAllTypes)
            {
                return true;
            }

            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                return fileInfo.Extension.ToUpper() == $".{FileType.ToUpper()}";
            }
            catch(Exception ex)
            {
                Logger.Error($"Can't apply rule to '{filePath}'", ex);
                throw;
            }
        }

        public override string ToString()
        {
            return FileType ?? (ForAllTypes ? "All" : "Unknown");
        }
    }
}