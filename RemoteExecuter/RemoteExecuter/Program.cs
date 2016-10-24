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
        private static string _iotEndpoint = "172.31.61.147";
        private static string _clientID = "123456789";
        private static string _targetTopic = "sakisaki";
        private static MqttClient _client = null;

        private static Shared.RemoexecSetting _setting = null;

        static void Main(string[] args)
        {
            Console.WriteLine("こちらからクライアントに指令します");

            // 環境設定
            _setting = new Shared.RemoexecSetting(_clientID.ToString());

            // 環境設定をパラメータにセット
            _iotEndpoint = _setting.EndPoint;

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
