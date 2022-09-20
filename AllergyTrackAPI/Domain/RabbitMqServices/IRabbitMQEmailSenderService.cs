namespace Domain.RabbitMqServices
{
    public interface IRabbitMQEmailSenderService
    {
        void Send(ReadOnlyMemory<byte> message);
    }
}
