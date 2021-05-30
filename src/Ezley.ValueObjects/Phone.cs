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
        public static bool operator ==(Phone left, Phone right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Phone left, Phone right)
        {
            return !Equals(left, right);
        }

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

        public static Phone Create(EncryptedPhone encPhone, byte[] key)
        {
            string countryCode = (key == null) ? "0" : Encryptor.DecryptFromBase64(encPhone.CountryCode, key);
            string number = (key == null) ? "0000000" : Encryptor.DecryptFromBase64(encPhone.Number, key);
            return new Phone(countryCode, number);
        }

        public Phone(string countryCode, string number)
        {
            CountryCode = countryCode;
            Number = number;
        }
    }
}