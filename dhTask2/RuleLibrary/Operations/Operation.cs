using Core.Interfaces;

namespace RuleLibrary.Operations
{
    public abstract class Operation : IOperation
    {
        public abstract string OperationName { get; }

        protected string ResultPattern
        {
            get
            {
                return $"{{0}} {OperationName}: {{1}}";
            }
        }

        public abstract string ExecuteOperation(string filePath);

        public override string ToString()
        {
            return OperationName ?? "Unknown";
        }
    }
}