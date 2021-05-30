using System;
using System.Net.WebSockets;
using ApplicationServices.AesEncryption;
using Ezley.ValueObjects.Encrypted;

namespace Ezley.ValueObjects
{
    public class DisplayName : IEquatable<DisplayName>
    {
        public static bool operator ==(DisplayName left, DisplayName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(DisplayName left, DisplayName right)
        {
            return !Equals(left, right);
        }

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

        public static DisplayName Create(EncryptedDisplayName encryptedDisplayName, byte[] key)
        {
            var noKey = "NoKey";

            string value = (key == null)
                ? $"{noKey} Value"
                : Encryptor.DecryptFromBase64(encryptedDisplayName.Value, key);
            return new DisplayName(value);
        }

        public DisplayName(string value)
        {
            Value = value;
        }
    }
}