using System.Collections.Generic;
using System.Linq;

namespace SmartTodo.Business.Models
{
    public class OperationResponse<T>
    {
        public OperationResponse()
        {}

        public OperationResponse(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public OperationResponse(T result)
        {
            Result = result;
        }

        public IEnumerable<string> Errors { get; set; } = new List<string>();

        public T Result { get; set; }

        public bool IsValid => !Errors.Any();
    }
}