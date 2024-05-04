using Newtonsoft.Json;

namespace GenGame
{
    public class PhxChannelRequest
    {
        public long JoinRef;
        public long MessageRef;
        public String TopicName;
        public String EventName;
        public dynamic Payload;

        public PhxChannelRequest(long argJoinRef, long argMessageRef, String argTopicName, String argEventName, object argPayload)
        {
            JoinRef = argJoinRef;
            MessageRef = argMessageRef;
            TopicName = argTopicName;
            EventName = argEventName;
            Payload = argPayload;
        }

        public String Encode()
        {
            List<object> msg = [JoinRef, MessageRef, TopicName, EventName, Payload];
            //JsonConvert.Serialize
            return JsonConvert.SerializeObject(msg, Formatting.Indented);
        }
    }
}