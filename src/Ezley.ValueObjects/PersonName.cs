using System;
using ApplicationServices.AesEncryption;
using Ezley.ValueObjects.Encrypted;
using Microsoft.AspNetCore.Mvc;

namespace Ezley.ValueObjects
{
    public class PersonName : IEquatable<PersonName>
    {
        public static bool operator ==(PersonName left, PersonName right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PersonName left, PersonName right)
        {
            return !Equals(left, right);
        }

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
        
        public string FirstName { get;  }
        public string LastName { get;   }

        public static PersonName Create(EncryptedPersonName encPersonName, byte[] key)
        {
            var noKey = "NoKey";
            
                var firstName = (key == null) ? $"{noKey} FirstName" :Encryptor.DecryptFromBase64(encPersonName.FirstName, key);
                var lastName = (key == null) ?  $"{noKey} LastName": Encryptor.DecryptFromBase64(encPersonName.LastName, key);
                return new PersonName(firstName, lastName);
        }
        
        public PersonName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}