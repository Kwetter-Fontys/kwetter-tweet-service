﻿using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TweetService.DAL.Repositories;

namespace TweetService.Services
{
    public class MessageReceiver : BackgroundService
    {
    
        private IModel _channel;
        private IConnection _connection;
        private readonly ITweetService _tweetService;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;

        public MessageReceiver(ITweetService tweetServ)
        {

            _hostname = "38.242.248.109";
            _username = "guest";
            _password = "pi4snc7kpg#77Q#F";
            _queueName = "deleteTweets";
            _tweetService = tweetServ;
            InitializeRabbitMqListener();
        }


        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                HandleMessage(content);

                _channel.BasicAck(ea.DeliveryTag, false);
            };


            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private void HandleMessage(string messageContent)
        {
            _tweetService.DeleteTweets(messageContent);
        }


        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}


