namespace ProducerAPI
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }
}
