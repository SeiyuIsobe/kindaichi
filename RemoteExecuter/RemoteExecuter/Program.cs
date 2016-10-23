using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace RemoteExecuter
{
    class Program
    {
        private static string _iotEndpoint = "192.168.11.14";
        private static string _clientID = "123456789";
        private static string _targetTopic = "sakisaki";
        private static MqttClient _client = null;

        static void Main(string[] args)
        {
            Console.WriteLine("This is Publisher...");

            _client = new MqttClient(_iotEndpoint);
            _client.Connect(_clientID);
            if (true == _client.IsConnected)
            {
                Console.WriteLine("connected.");

                var cmd = Console.ReadLine();
                _client.Publish(_targetTopic, Encoding.UTF8.GetBytes(cmd));
            }
            else
            {

            }

            Console.ReadLine();
        }
    }
}
