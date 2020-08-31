using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FlightMobileWeb
{
    public class Global
    {
        static FlightGearClient flightGearClient = new FlightGearClient();
        static String IP;
        static int telnetPort;
        static int HttpPort;

        public static FlightGearClient flightGearClientprop
        {
            get
            {
                return flightGearClient;
            }
            set
            {
                flightGearClient = value;
            }
        }
        public static String IPprop
        {
            get
            {
                return IP;
            }
            set
            {
                IP = value;
            }
        }
        public static int telnetPortprop
        {
            get
            {
                return telnetPort;
            }
            set
            {
                telnetPort = value;
            }
        }
        public static int HttpPortprop
        {
            get
            {
                return HttpPort;
            }
            set
            {
                HttpPort = value;
            }
        }
    }
}
