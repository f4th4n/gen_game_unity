using System.Collections.Generic;
using Newtonsoft.Json;

namespace GenGame
{
    public class PhxChannelRequest
    {
        public long JoinRef;
        public long MessageRef;
        public string TopicName;
        public string EventName;
        public dynamic Payload;

        public PhxChannelRequest(long argJoinRef, long argMessageRef, string argTopicName, string argEventName, object argPayload)
        {
            JoinRef = argJoinRef;
            MessageRef = argMessageRef;
            TopicName = argTopicName;
            EventName = argEventName;
            Payload = argPayload;
        }

        public string Encode()
        {
            List<object> msg = new List<object> { JoinRef, MessageRef, TopicName, EventName, Payload };
            //JsonConvert.Serialize
            return JsonConvert.SerializeObject(msg, Formatting.Indented);
        }
    }
}