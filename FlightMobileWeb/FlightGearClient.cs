using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlightMobileWeb
{
    public class FlightGearClient
    {
        private readonly BlockingCollection<AsyncCommand> _queue;
        private TcpClient _client;
        private NetworkStream Stream;

        public FlightGearClient()
        {
            _queue = new BlockingCollection<AsyncCommand>();
            _client = new TcpClient();
            this.Start();
        }
        // Called by the WebApi Controller, it will await on the returned Task<>
        // This is not an async method, since it does not await anything.
        public Task<Result> Execute(Command cmd)
        {
            var asyncCommand = new AsyncCommand(cmd);
            _queue.Add(asyncCommand);
            return asyncCommand.Task;
        }
        public void Start()
        {
            Task.Factory.StartNew(ProcessCommands);
        }
        private void ProcessCommands()
        {
            foreach (AsyncCommand command in _queue.GetConsumingEnumerable())
            {
                Result res;
                try
                {

                    Write("set /controls/engines/current-engine/throttle " + String.Format("{0:0.##}", command.Command.throttle) + "\r\n");
                    Write("set /controls/flight/rudder " + String.Format("{0:0.##}", command.Command.rudder) + "\r\n");
                    Write("set /controls/flight/aileron " + String.Format("{0:0.##}", command.Command.aileron) + "\r\n");
                    Write("set /controls/flight/elevator " + String.Format("{0:0.##}", command.Command.elevator) + "\r\n");

                    double aileron = Double.Parse(writeAndGet("get /controls/flight/aileron " + "\r\n"));
                    double rudder = Double.Parse(writeAndGet("get /controls/flight/rudder " + "\r\n"));
                    double elevator = Double.Parse(writeAndGet("get /controls/flight/elevator " + "\r\n"));
                    double throttle = Double.Parse(writeAndGet("get /controls/engines/current-engine/throttle " + "\r\n"));

                  /* Console.WriteLine(aileron);
                    Console.WriteLine(rudder);

                    Console.WriteLine(elevator);

                    Console.WriteLine(throttle);*/

                    //check validation
                   if (aileron != Double.Parse(String.Format("{0:0.##}", command.Command.aileron)) || rudder != Double.Parse(String.Format("{0:0.##}", command.Command.rudder)) ||
                            elevator != Double.Parse(String.Format("{0:0.##}", command.Command.elevator)) || throttle != Double.Parse(String.Format("{0:0.##}", command.Command.throttle)))
                    {
                        res = Result.NotOk;
                    }
                    res = Result.Ok;
                }
                catch
                {
                    res = Result.NotOk;
                }
                //Check is value defined and then set Result  
                command.Completion.SetResult(res);

            }
        }
        public void connect(string ip, int port)
        {
                if (!_client.Connected)
                {
                    _client.Dispose();
                    _client = new TcpClient();
                    _client.Connect(ip, port);
                    Stream = _client.GetStream();
                    Write("data" + "\r\n");
                }
                Stream = _client.GetStream();
        }

        public void Write(string messsage)
        {
            byte[] buffer = new byte[1024];
            var encoding = Encoding.ASCII;
            //  Stream = TcpClient.GetStream();
            var bytesToSend = encoding.GetBytes(messsage);
            Stream.Write(bytesToSend, 0, bytesToSend.Length);
            Stream.Flush();
        }
        public string writeAndGet(string message)
        {
            byte[] buffer = new byte[1024];
            var encoding = System.Text.Encoding.ASCII;
            //   Stream = TcpClient.GetStream();
            var bytesToSend = encoding.GetBytes(message);
            Stream.Write(bytesToSend, 0, bytesToSend.Length);
            Stream.Flush();
            Stream.ReadTimeout = 10000;
            return read(buffer, encoding);
        }
        public string read(byte[] buffer, Encoding encoding)
        {
            Stream.Flush();
            string receivedString;
            var receivedBytesCount = Stream.Read(buffer, 0, buffer.Length);
            receivedString = encoding.GetString(buffer, 0, receivedBytesCount);
            return receivedString;
        }

        public void disconnect()
        {
            if ((_client != null) && (_client.Connected))
            {
                if (Stream != null) { Stream.Close(); }
                _client.Close();
            }
        }
    }


}

