using System;
using ApplicationServices.AesEncryption;
using Ezley.ValueObjects.Encrypted;

namespace Ezley.ValueObjects
{
    public class DisplayName : IEquatable<DisplayName>
    {
        public bool Equals(DisplayName other)
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
            return Equals((DisplayName) obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }

        public string Value { get; }

        public DisplayName(EncryptedDisplayName encryptedDisplayName, byte[] key)
        {
            var noKey = "NoKey";
            if (key == null)
            {
                Value = $"{noKey} Value";
            }
            else
            {
                Value = Encryptor.DecryptFromBase64(encryptedDisplayName.Value, key);
            }
        }

        public DisplayName(string value)
        {
            Value = value;
        }
    }
}