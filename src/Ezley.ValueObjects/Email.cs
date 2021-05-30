using System;
using ApplicationServices.AesEncryption;
using Ezley.ValueObjects.Encrypted;

namespace Ezley.ValueObjects
{
    public class Email : IEquatable<Email>
    {
        public static bool operator ==(Email left, Email right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Email left, Email right)
        {
            return !Equals(left, right);
        }

        public bool Equals(Email other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Address == other.Address;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Email) obj);
        }

        public override int GetHashCode()
        {
            return (Address != null ? Address.GetHashCode() : 0);
        }

        public string Address { get; }

        public static Email Create(EncryptedEmail encEmail, byte[] key)
        {
            var noKey = "NoKey";
             string address = (key == null) ? $"{noKey} Address" : Encryptor.DecryptFromBase64(encEmail.Address, key);;
             return new Email(address);
        }

        public Email(string address)
        {
            Address = address;
        }
    }
}