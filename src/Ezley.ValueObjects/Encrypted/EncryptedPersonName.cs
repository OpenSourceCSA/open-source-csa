using System;
using ApplicationServices.AesEncryption;

namespace Ezley.ValueObjects.Encrypted
{
    public class EncryptedPersonName : IEquatable<EncryptedPersonName>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public EncryptedPersonName()
        {
        }

        public EncryptedPersonName(bool inputEncrypted, byte[] key, byte[] iv, string firstName, string lastName)
        {
            Init(inputEncrypted, key, iv, firstName, lastName);
        }

        public EncryptedPersonName(PersonName personName, byte[] key, byte[] iv)
        {
            const bool encrypted = false;
            Init(encrypted, key, iv, personName.FirstName, personName.LastName);
        }

        private void Init(bool inputEncrypted, byte[] key, byte[] iv, string firstName, string lastName)
        {
            FirstName = inputEncrypted ? firstName : Encryptor.EncryptToBase64(firstName, key, iv);
            LastName = inputEncrypted ? firstName : Encryptor.EncryptToBase64(lastName, key, iv);
        }

        public bool Equals(EncryptedPersonName other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return FirstName == other.FirstName && LastName == other.LastName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName);
        }
    }
}