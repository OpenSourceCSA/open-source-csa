using System;
using ApplicationServices.AesEncryption;
using Ezley.ValueObjects.Encrypted;
using FluentValidation;
using Microsoft.Azure.Documents.SystemFunctions;

namespace Ezley.ValueObjects
{
    public class PhoneValidator : AbstractValidator<Phone>
    {
        public PhoneValidator()
        {
            RuleFor(phone => phone.CountryCode).NotEmpty();
            RuleFor(phone => phone.CountryCode).IsNumber();

            RuleFor(phone => phone.Number).NotEmpty();
            RuleFor(phone => phone.Number).IsNumber();
        }
    }

    public class Phone : IEquatable<Phone>
    {
        public bool Equals(Phone other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return CountryCode == other.CountryCode && Number == other.Number;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Phone) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CountryCode, Number);
        }

        public string CountryCode { get; }
        public string Number { get; }

        public Phone(EncryptedPhone encPhone, byte[] key)
        {
            if (key == null)
            {
                CountryCode = "0";
                Number = "0000000";
            }
            else
            {
                CountryCode = Encryptor.DecryptFromBase64(encPhone.CountryCode, key);
                Number = Encryptor.DecryptFromBase64(encPhone.Number, key);
            }
        }

        public Phone(string countryCode, string number)
        {
            CountryCode = countryCode;
            Number = number;
        }
    }
}