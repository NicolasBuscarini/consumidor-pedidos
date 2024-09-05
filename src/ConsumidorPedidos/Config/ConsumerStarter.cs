using ConsumidorPedidos.Core.Consumer;
using ConsumidorPedidos.Data.Messaging;

public class ConsumerStarter
{
    private readonly RabbitMqService _rabbitMqService;
    private readonly MessageConsumer _messageConsumer;

    public ConsumerStarter(RabbitMqService rabbitMqService, MessageConsumer messageConsumer)
    {
        _rabbitMqService = rabbitMqService;
        _messageConsumer = messageConsumer;
    }

    public void StartConsumer(string queueName)
    {
        Task.Run(() =>
        {
            _messageConsumer.StartConsuming(queueName);
        });

        AppDomain.CurrentDomain.ProcessExit += (s, e) => _rabbitMqService.Close();
    }
}