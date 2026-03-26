using Common.Exceptions.enums;
using UnityEngine;

namespace Common.Exceptions.DTO
{
    public class RegisterError
    {
        public RegisterErrorsEnums errorType { get; set;  }
        public string message { get; set; }

        public RegisterError(RegisterErrorsEnums errorType, string message)
        {
            this.errorType = errorType;
            this.message = message;
        }
    }
}