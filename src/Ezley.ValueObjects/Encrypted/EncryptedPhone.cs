using System;
using ApplicationServices.AesEncryption;

namespace Ezley.ValueObjects.Encrypted
{
    public class EncryptedPhone : IEquatable<EncryptedPhone>
    {
        public static bool operator ==(EncryptedPhone left, EncryptedPhone right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EncryptedPhone left, EncryptedPhone right)
        {
            return !Equals(left, right);
        }

        public string CountryCode { get; set; }
        public string Number { get; set; }

        public EncryptedPhone()
        {
        }

        public EncryptedPhone(Phone phone, byte[] key, byte[] iv)
        {
            const bool encrypted = false;
            Init(encrypted, key, iv, phone.Number, phone.CountryCode);
        }

        public void Init(bool inputEncrypted, byte[] key, byte[] iv, string phoneNumber, string countryCode)
        {
            CountryCode = inputEncrypted ? countryCode : Encryptor.EncryptToBase64(countryCode, key, iv);
            Number = inputEncrypted ? phoneNumber : Encryptor.EncryptToBase64(phoneNumber, key, iv);
        }

        public bool Equals(EncryptedPhone other)
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
            return Equals((EncryptedPhone) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CountryCode, Number);
        }
    }
}