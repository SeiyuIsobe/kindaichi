﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace Remoteworker
{
    class Program
    {
        private static string _iotEndpoint = "192.168.11.14";
        private static string _clientID = "987654321";
        private static string _targetTopic = "sakisaki";
        private static MqttClient _client = null;

        static void Main(string[] args)
        {
            _client = new MqttClient(_iotEndpoint);
            _client.Connect(_clientID);

            Console.WriteLine("connected.");

            _client.Subscribe(new[] { _targetTopic }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            _client.MqttMsgPublishReceived += (sender, e) =>
            {
                var msg = Encoding.UTF8.GetString(e.Message);
                var topic = e.Topic;

                Console.WriteLine(topic + ", " + msg);

                System.Diagnostics.Process.Start(@"C:\Windows\System32\notepad.exe");

            };

            while(_client.IsConnected)
            {

            }

            Console.ReadLine();
        }
    }
}