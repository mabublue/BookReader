using Domain.BaseTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Exceptions
{
    public class ValidationErrorsFoundException : Exception
    {
        public ValidationErrorsFoundException() : base("Validation errors were found. This is a sign of a bug because validation should be checked before creating.")
        {
        }

        public ValidationErrorsFoundException(string message) : base(message)
        {
        }

        public ValidationErrorsFoundException(IEnumerable<ValidationError> errors) : base(FormatErrors(errors))
        {

        }

        public ValidationErrorsFoundException(string message,
            Exception innerException) : base(message, innerException)
        {
        }

        private static string FormatErrors(IEnumerable<ValidationError> errors)
        {
            return string.Join(", ", errors.Select(e => !string.IsNullOrEmpty(e.Field) ? $"{e.Field}: {e.Message}" : e.Message));
        }
    }
}
