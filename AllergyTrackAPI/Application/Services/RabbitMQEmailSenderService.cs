using Domain.RabbitMqServices;
using Infrastructure.RabbitMQ;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RabbitMQEmailSenderService : IRabbitMQEmailSenderService, IDisposable
    {

        private readonly RabbitMQHelper _rabbitMqHelperService;
        private readonly IConfiguration _configuration;
        private IModel _channel; 
        private IConnection _connection;

        private const string configKey = "RabbitMq:NotificationSender:";
        private const string exchangeType = "direct";

        public RabbitMQEmailSenderService(RabbitMQHelper rabbitMqHelperService,
            IConfiguration configuration)
        {
            _rabbitMqHelperService = rabbitMqHelperService;
            _configuration = configuration;

            var factory = _rabbitMqHelperService.GetConnectionFactory();

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(exchange: _configuration[configKey + "exchangeName"], type : exchangeType);
        }

        public void Send(ReadOnlyMemory<byte> message)
        {
            _channel.BasicPublish(exchange: _configuration[configKey + "exchangeName"],
                                  routingKey: _configuration[configKey+ "routingKey"],
                                  basicProperties: null,
                                  body: message);            
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}

