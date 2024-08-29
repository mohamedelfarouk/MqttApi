using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MqttApi.Mqtt
{
    public interface IMqttService
    {
        void Publish(string topic, string message);
        void Subscribe(string topic);
        void Unsubscribe(string topic);
    }
}
