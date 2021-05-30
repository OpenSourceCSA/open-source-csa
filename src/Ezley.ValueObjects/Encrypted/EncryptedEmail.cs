using System;
using ApplicationServices.AesEncryption;

namespace Ezley.ValueObjects.Encrypted
{
    public class EncryptedEmail : IEquatable<EncryptedEmail>
    {
        public static bool operator ==(EncryptedEmail left, EncryptedEmail right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EncryptedEmail left, EncryptedEmail right)
        {
            return !Equals(left, right);
        }

        public string Address { get; set; }

        public bool Equals(EncryptedEmail other)
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
            return Equals((EncryptedEmail) obj);
        }

        public override int GetHashCode()
        {
            return (Address != null ? Address.GetHashCode() : 0);
        }

        public EncryptedEmail()
        {
        }

        public EncryptedEmail(Email email, byte[] key, byte[] iv)
        {
            const bool encrypted = false;
            Init(encrypted, key, iv, email.Address);
        }

        public void Init(bool inputEncrypted, byte[] key, byte[] iv, string address)
        {
            Address = inputEncrypted ? address : Encryptor.EncryptToBase64(address, key, iv);
        }
    }
}