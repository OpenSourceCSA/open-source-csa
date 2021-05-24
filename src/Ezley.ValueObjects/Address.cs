using System;
using ApplicationServices.AesEncryption;
using Ezley.ValueObjects.Encrypted;

namespace Ezley.ValueObjects
{
    public class Address : IEquatable<Address>
    {
        public bool Equals(Address other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Line1 == other.Line1 && Line2 == other.Line2 && Line3 == other.Line3 && City == other.City &&
                   PostalCode == other.PostalCode && State == other.State && Country == other.Country;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Address) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Line1, Line2, Line3, City, PostalCode, State, Country);
        }

        public string Line1 { get; }
        public string Line2 { get; }
        public string Line3 { get; }
        public string City { get; }
        public string PostalCode { get; }
        public string State { get; }
        public string Country { get; }

        public Address(EncryptedAddress encAddress, byte[] key)
        {
            var none = "NoKey";
            if (key == null)
            {
                Line1 = $"{none} Line1";
                Line2 = $"{none} Line2";
                Line3 = $"{none} Line3";
                City = $"{none} City";
                PostalCode = $"{none} PostalCode";
                State = $"{none} State";
                Country = $"{none} Country";
            }
            else
            {
                Line1 = Encryptor.DecryptFromBase64(encAddress.Line1, key);
                Line2 = Encryptor.DecryptFromBase64(encAddress.Line2, key);
                Line3 = Encryptor.DecryptFromBase64(encAddress.Line3, key);
                City = Encryptor.DecryptFromBase64(encAddress.City, key);
                PostalCode = Encryptor.DecryptFromBase64(encAddress.PostalCode, key);
                State = Encryptor.DecryptFromBase64(encAddress.State, key);
                Country = Encryptor.DecryptFromBase64(encAddress.Country, key);
            }
        }

        public Address(
            string line1, string line2, string line3, string city, string postalCode, string state, string country)
        {
            Line1 = line1;
            Line2 = line2;
            Line3 = line3;
            City = city;
            PostalCode = postalCode;
            State = state;
            Country = country;
        }
    }
}