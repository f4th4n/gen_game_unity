namespace GenGame
{
    public class PhxChannelResponse
    {
        public long? JoinRef;
        public long? MessageRef;
        public String TopicName;
        public String EventName;
        public dynamic Payload;

        public PhxChannelResponse(IEnumerable<object> data)
        {
            JoinRef = (long?)data.ElementAt(0);
            MessageRef = (long?)data.ElementAt(1);
            TopicName = (String)data.ElementAt(2);
            EventName = (String)data.ElementAt(3);
            Payload = data.ElementAt(4);
        }

        public override string ToString()
        {
            return $"join ref: {MessageRef.ToString()}, msg ref: {MessageRef.ToString()}, topic: {TopicName}, event: {EventName}, payload: {Payload}";
        }
    }
}