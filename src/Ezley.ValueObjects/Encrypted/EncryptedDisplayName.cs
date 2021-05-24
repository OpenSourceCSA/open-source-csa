using System;
using ApplicationServices.AesEncryption;

namespace Ezley.ValueObjects.Encrypted
{
    public class EncryptedDisplayName : IEquatable<EncryptedDisplayName>
    {
        public string Value { get; set; }

        public bool Equals(EncryptedDisplayName other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EncryptedDisplayName) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public EncryptedDisplayName(bool inputEncrypted, byte[] key, byte[] iv, string value)
        {
            Init(inputEncrypted, key, iv, value);
        }

        public EncryptedDisplayName()
        {
        }

        public EncryptedDisplayName(DisplayName displayName, byte[] key, byte[] iv)
        {
            const bool encrypted = false;
            Init(encrypted, key, iv, displayName.Value);
        }

        public void Init(bool inputEncrypted, byte[] key, byte[] iv, string value)
        {
            Value = inputEncrypted ? value : Encryptor.EncryptToBase64(value, key, iv);
        }
    }
}