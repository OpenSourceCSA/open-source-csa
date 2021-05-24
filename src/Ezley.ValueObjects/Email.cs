using System;
using ApplicationServices.AesEncryption;
using Ezley.ValueObjects.Encrypted;

namespace Ezley.ValueObjects
{
    public class Email : IEquatable<Email>
    {
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

        public Email(EncryptedEmail encEmail, byte[] key)
        {
            var noKey = "NoKey";
            if (key == null)
            {
                Address = $"{noKey} Address";
            }
            else
            {
                Address = Encryptor.DecryptFromBase64(encEmail.Address, key);
            }
        }

        public Email(string address)
        {
            Address = address;
        }
    }
}