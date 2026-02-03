using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarpetCleaningSystem.Application.Exceptions
{
    public class PhoneNumberAlreadyInUseException : Exception
    {
        public PhoneNumberAlreadyInUseException(string phoneNumber)
            : base($"The phone number {phoneNumber} is already in use by another customer.")
        {
        }
    }
}
