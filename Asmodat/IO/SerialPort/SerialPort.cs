using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Types;
using Asmodat.Extensions.Objects;
using Asmodat.Debugging;

namespace Asmodat.IO
{
    public partial class SerialPort
    {
        public static int[] bauds = new int[] { 9600, 14400, 19200, 28800, 38400, 56000, 57600, 115200 };

        /// <summary> 
        /// End of transmition byte in this case EOT (ASCII 4). 
        /// </summary> 
        public const byte terminator = 0x4;

        ThreadedTimers Timers = new ThreadedTimers();
        System.IO.Ports.SerialPort _serialPort = new System.IO.Ports.SerialPort();
        public BufferedArray<string> Data { get; set; }

        public SerialPort(int bufferSize = 1024)
        {
            Data = new BufferedArray<string>(bufferSize);
            _serialPort.DataReceived += SerialPortMain_DataReceived;

            _serialPort.ReadTimeout = 4000;
            _serialPort.WriteTimeout = 6000;
            Timers.Run(() => Peacemaker(), 1000);
        }

        public bool Started { get; private set; } = false;
        public void Start(string portName, int baudRate, int dataBits = 8, System.IO.Ports.StopBits stopBits = System.IO.Ports.StopBits.One, System.IO.Ports.Parity parity = System.IO.Ports.Parity.None)
        {
            _serialPort.BaudRate = baudRate;
            _serialPort.PortName = portName;
            _serialPort.DataBits = dataBits;
            _serialPort.Parity = parity;
            _serialPort.StopBits = stopBits;
            Started = true;
        }

        public string PortName { get { return _serialPort.PortName; } }
        public int BaudRate { get { return _serialPort.BaudRate; } }
        public int DataBits { get { return _serialPort.DataBits; } }
        public System.IO.Ports.Parity Parity { get { return _serialPort.Parity; } }
        public System.IO.Ports.StopBits StopBits { get { return _serialPort.StopBits; } }

        public void Stop()
        {
            Started = false;
        }


        public bool IsOpen { get { return _serialPort.IsOpen; } }


        public void Peacemaker()
        {
            if (Started && !_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Open();
                }
                catch(Exception ex)
                {
                    ex.ToOutput();

                }
            }
            else if (!Started && _serialPort.IsOpen)
                _serialPort.Close();
        }

        public List<string> Ports
        {
            get
            {
                string[] list = System.IO.Ports.SerialPort.GetPortNames();
                if (list == null)
                    return new List<string>();
                else
                    return list.ToList();
            }
        }

        public bool Send(string text)
        {
            if (!text.IsNullOrEmpty() && _serialPort.IsOpen)
            {
                try
                {
                    _serialPort.WriteLine(text);
                    return true;
                }
                catch(Exception ex)
                {
                    ex.ToOutput();
                    return false;
                }
            }
            else
                return false;
        }


        private void SerialPortMain_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            byte[] buffer;
            int bytesRead;
            string result = "";
            do
            {
                buffer = new byte[_serialPort.ReadBufferSize];
                bytesRead = _serialPort.Read(buffer, 0, buffer.Length);

                result += Encoding.ASCII.GetString(buffer, 0, bytesRead);

            } while (bytesRead <= 0);

            if(!result.IsNullOrEmpty())
                Data.Write(result);
        }

    }
}


