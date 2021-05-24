using System;
using ApplicationServices.AesEncryption;
using Ezley.ValueObjects.Encrypted;

namespace Ezley.ValueObjects
{
    public class PersonName : IEquatable<PersonName>
    {
        public bool Equals(PersonName other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return FirstName == other.FirstName && LastName == other.LastName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PersonName) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName);
        }

        public string FirstName { get; }
        public string LastName { get; }

        public PersonName(EncryptedPersonName encPersonName, byte[] key)
        {
            var noKey = "NoKey";
            if (key == null)
            {
                FirstName = $"{noKey} FirstName";
                LastName = $"{noKey} LastName";
            }
            else
            {
                FirstName = Encryptor.DecryptFromBase64(encPersonName.FirstName, key);
                LastName = Encryptor.DecryptFromBase64(encPersonName.LastName, key);
            }
        }

        public PersonName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}