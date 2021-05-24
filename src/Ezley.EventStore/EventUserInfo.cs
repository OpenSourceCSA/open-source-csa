namespace Ezley.EventStore
{
    public class EventUserInfo
    {
        public string AuthServiceUserId { get; }
        public bool IsApiKey { get; }

        public AppMetaData AppMetaData { get; }

        public EventUserInfo(string authServiceUserId, bool isApiKey = true)
        {
            AuthServiceUserId = authServiceUserId;
            IsApiKey = isApiKey;
        }

        public EventUserInfo(string authServiceUserId, AppMetaData appMetaData)
        {
            AuthServiceUserId = authServiceUserId;
            AppMetaData = appMetaData;
            IsApiKey = false;
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