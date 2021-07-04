using Domain.BaseTypes;
using Domain.Exceptions;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Extensions
{
    public static class Throw
    {
        [ContractAnnotation("toCheck:null => halt")]
        public static void IfNull(object toCheck,
            string argumentName)
        {
            if (toCheck == null)
                throw new ArgumentNullException(argumentName);
        }

        public static void IfHasValidationErrors(IEnumerable<ValidationError> errors)
        {
            if (errors == null)
                return;
            IList<ValidationError> validationErrors = errors as IList<ValidationError> ?? errors.ToList();
            if (validationErrors.Any())
                throw new ValidationErrorsFoundException(validationErrors);
        }

        [ContractAnnotation("toCheck:null => halt")]
        public static void IfNullOrEmpty(string toCheck,
            string argumentName)
        {
            if (string.IsNullOrEmpty(toCheck))
                throw new ArgumentNullException(argumentName);
        }
    }

}
