using System;
using ApplicationServices.AesEncryption;

#nullable enable
namespace Ezley.ValueObjects.Encrypted
{
    public class EncryptedAddress : IEquatable<EncryptedAddress>
    {
        public bool Equals(EncryptedAddress? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Line1 == other.Line1 
                   && Line2 == other.Line2 
                   && Line3 == other.Line3 
                   && City == other.City 
                   && PostalCode == other.PostalCode 
                   && State == other.State 
                   && Country == other.Country;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EncryptedAddress) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Line1, Line2, Line3, City, PostalCode, State, Country);
        }

        public static bool operator ==(EncryptedAddress? left, EncryptedAddress? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EncryptedAddress? left, EncryptedAddress? right)
        {
            return !Equals(left, right);
        }

        public string Line1 { get; set; } = string.Empty;
        public string Line2 { get; set; } = string.Empty;
        public string Line3 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        

        public EncryptedAddress()
        {
        }

        public EncryptedAddress(Address address, byte[] key, byte[] iv)
        {
            const bool encrypted = false;

            Init(encrypted, key, iv, address.Line1, address.Line2, address.Line3, address.City,
                address.PostalCode, address.State, address.Country);
        }

        private void Init(
            bool inputsEncrypted, byte[] key, byte[] iv, string line1, string line2, string line3, string city,
            string postalCode,
            string state, string country)
        {
            Line1 = inputsEncrypted ? line1 : Encryptor.EncryptToBase64(line1, key, iv);
            Line2 = inputsEncrypted ? line2 : Encryptor.EncryptToBase64(line2, key, iv);
            Line3 = inputsEncrypted ? line3 : Encryptor.EncryptToBase64(line3, key, iv);
            City = inputsEncrypted ? city : Encryptor.EncryptToBase64(city, key, iv);
            PostalCode = inputsEncrypted ? postalCode : Encryptor.EncryptToBase64(postalCode, key, iv);
            State = inputsEncrypted ? state : Encryptor.EncryptToBase64(state, key, iv);
            Country = inputsEncrypted ? country : Encryptor.EncryptToBase64(country, key, iv);
        }
    }
}