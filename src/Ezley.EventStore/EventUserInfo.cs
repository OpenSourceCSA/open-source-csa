using System;
using System.Runtime.CompilerServices;

namespace Ezley.EventStore
{
    public class EventUserInfo
    {
        private const string anonymous = "anonymous";
        public string AuthServiceUserId { get; }
        public bool IsApiKey { get; }

        public AppMetaData AppMetaData { get; }

        public EventUserInfo(string authServiceUserId, bool isApiKey = true)
        {
            AuthServiceUserId = (authServiceUserId.Trim().Length == 0)
                ? anonymous
                : authServiceUserId;
            IsApiKey = isApiKey;
        }

        public EventUserInfo(string authServiceUserId, AppMetaData appMetaData)
        {
            AuthServiceUserId = authServiceUserId;
            AppMetaData = appMetaData;
            IsApiKey = false;
        }

        public void VerifyIsApiKey_ThrowsException()
        {
            if (!IsApiKey)
                throw new UnauthorizedAccessException("Expected ApiKey.");
        }

        public void VerifyIsToken_ThrowsException()
        {
            if (IsApiKey || AuthServiceUserId == anonymous)
                throw new UnauthorizedAccessException("Expected token.");
        }

        public void VerifyIsAnonymous_ThrowsException()
        {
            if (IsApiKey || AuthServiceUserId != anonymous)
                throw new UnauthorizedAccessException("Expected anonymous.");
        }
       
    }

    public class AppMetaData
    {
        public string ClientId { get; }
        public string ProducerIds { get; }

        public AppMetaData(string clientId, string producerIds)
        {
            ClientId = clientId;
            ProducerIds = producerIds;
        }   
    }
}