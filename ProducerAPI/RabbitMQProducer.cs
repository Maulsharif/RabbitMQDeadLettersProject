using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Text;

namespace ProducerAPI
{
    public class RabbitMQProducer : IMessageProducer
    {
        private const string WORK_QUEUE = "order";
        private const string WORK_EXCHANGE = "WorkExchange"; // dead letter exchange
        private const string RETRY_EXCHANGE = "RetryExchange";
        private const string RETRY_QUEUE = "RetryQueue";
        private const int RETRY_DELAY = 30000; // in ms

        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            var json = JsonConvert.SerializeObject(message);
             var body = Encoding.UTF8.GetBytes(json);
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(WORK_EXCHANGE, "direct"); var queueArgs = new Dictionary<string, object> {
    { "x-dead-letter-exchange", WORK_EXCHANGE },
    { "x-message-ttl", RETRY_DELAY }
};
                channel.QueueDeclare(WORK_QUEUE, true, false, false, null);
                channel.QueueBind(WORK_QUEUE, WORK_EXCHANGE, string.Empty, null);

                channel.ExchangeDeclare(RETRY_EXCHANGE, "direct");
                channel.QueueDeclare(RETRY_QUEUE, true, false, false, queueArgs);
                channel.QueueBind(RETRY_QUEUE, RETRY_EXCHANGE, string.Empty, null);

                channel.BasicPublish(RETRY_EXCHANGE, string.Empty, null, body);
            };


            //    var connection = factory.CreateConnection();
            //    using var channel = connection.CreateModel();

            //    channel.QueueDeclare("orders", exclusive: false);

            //    var json = JsonConvert.SerializeObject(message);
            //    var body = Encoding.UTF8.GetBytes(json);
            //    // ch.default_exchange.publish 'message content', routing_key: DELAYED_QUEUE
            //    channel.ExchangeDeclare(WORK_EXCHANGE, "direct");
            //    channel.QueueDeclare(WORK_QUEUE, true, false, false, null);
            //    channel.QueueBind(WORK_QUEUE, WORK_EXCHANGE, string.empty, null);
            //rou
        }
    }
}
