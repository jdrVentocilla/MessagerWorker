using MessagerWorker.Models;
using MessageWorker.Queue.Client;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;


namespace MessagerWorker.Queue
{
    public class TransferEntityQueueSender : IMessageQueueSender<TransferEntity>
    {

        private IConnection _connection;
        private IModel _channel;
        private readonly TransferQueueSetting _settings;
        public TransferEntityQueueSender(TransferQueueSetting settings)
        {
            _settings = settings;
        }

        public void Execute(TransferEntity mensaje)
        {

            var factory = new ConnectionFactory() { HostName = _settings.HostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            var jsonMessage = JsonConvert.SerializeObject(mensaje);
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            _channel.BasicPublish(exchange: "", routingKey: "Transfer", basicProperties: null, body);

        }
        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
            }

            if (_channel !=null)
            {
                _channel.Dispose();

            }
           
        }

        
    }
}
