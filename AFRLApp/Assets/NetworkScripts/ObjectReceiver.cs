using System;
#if NETFX_CORE
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Windows.Networking.Sockets;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
#endif

namespace HLNetwork
{
    public class ObjectReceiver
    {
        private enum MessageType { Image = 1, PositionIDRequest = 2, ArrowPlacement = 3 }
        private static ObjectReceiver _theInstance = null;
#if NETFX_CORE
        private StreamSocketListener _socketListener;
#endif

        /// <summary>
        /// ObjectReceiver is currently capable of receiving images over TCP
        /// port 33334
        /// </summary>
        public ObjectReceiver()
        {
#if NETFX_CORE
            _socketListener = new StreamSocketListener();
            _socketListener.ConnectionReceived += ConnectionReceived;
            _socketListener.BindServiceNameAsync("33334");
            System.Diagnostics.Debug.WriteLine("Listening for connections");
#endif
        }

        public static ObjectReceiver getTheInstance()
        {
            if (_theInstance == null)
            {
                _theInstance = new ObjectReceiver();
            }
            return _theInstance;
        }

        public void SendPositionIDResponse(uint posID)
        {
            byte[] length = BitConverter.GetBytes(6);
            byte[] msgType = BitConverter.GetBytes((short)MessageType.PositionIDRequest);
            byte[] id = BitConverter.GetBytes(posID);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(length);
                Array.Reverse(msgType);
                Array.Reverse(id);
            }
            byte[] msg = new byte[10];
            Array.Copy(length, 0, msg, 0, 4);
            Array.Copy(msgType, 0, msg, 4, 2);
            Array.Copy(id, 0, msg, 6, 4);
#if NETFX_CORE
            _socket.OutputStream.WriteAsync(msg.AsBuffer());
#endif
        }

#if NETFX_CORE
        /// <summary>
        /// The function which is called by the StreamSocketListener when a
        /// connection is received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void ConnectionReceived(
            Windows.Networking.Sockets.StreamSocketListener sender,
            Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine("Got a connection");
            _socket = args.Socket;

            //
            // This function used to be async, and the call to Listen used to
            // be awaited.  When I changed it to its current form, the
            // application stopped crashing on connection loss.  I'm not
            // fully certain as to why; it might be because some kind of
            // socket timeout now only silently crashes that connection's
            // thread.  Reconnection works without restarting, though.
            //

            Listen();
        }

        /// <summary>
        /// This is the main loop in which a connection is monitored.
        /// </summary>
        /// <returns></returns>
        private async Task Listen()
        {
            System.Diagnostics.Debug.WriteLine("Listening");
            while (true)
            {
                byte[] lengthBytes = await ReceiveBytes(4);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(lengthBytes);
                }
                int messageLength = BitConverter.ToInt32(lengthBytes, 0);
                byte[] serializedObj = await ReceiveBytes((int)messageLength);
                InterpretMessage(serializedObj);
            }
        }

        /// <summary>
        /// Receives a given number of bytes from the object's current
        /// connection.  This is where the object actually waits for
        /// data most of the time.
        /// </summary>
        /// <param name="length">The number of bytes to receive</param>
        /// <returns>A byte array containing the received bytes</returns>
        private async Task<byte[]> ReceiveBytes(int length)
        {
            System.Diagnostics.Debug.WriteLine("Trying to receive " + length + " bytes");
            Stream inStream = _socket.InputStream.AsStreamForRead();
            byte[] buffer = new byte[length];
            int received = 0;

            while (received < length)
            {
                received += inStream.Read(buffer, received, length - received);
                if (received < length)
                {
                    await Task.Delay(20);
                }
            }

            System.Diagnostics.Debug.WriteLine("Successfully received " + length + " bytes");
            return buffer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg">The message received from the network</param>
        private void InterpretMessage(byte[] msg)
        {
            System.Diagnostics.Debug.WriteLine("Interpreting message");

            //
            // Read the message type code (first two bytes)
            //

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(msg, 0, 2);
            }
            short objTypeCode = BitConverter.ToInt16(msg, 0);

            //
            // Cut off the message type code
            //

            byte[] remainder = new byte[msg.Length - 2];
            Array.ConstrainedCopy(msg, 2, remainder, 0, msg.Length - 2);

            //
            // The magic number '1' below represents an image message.
            // This constant should probably be replaced by something else.
            //

            switch ((MessageType)objTypeCode)
            {
                case MessageType.Image:
                    ReadJpeg(remainder);
                    break;
                case MessageType.PositionIDRequest:
                    System.Diagnostics.Debug.WriteLine("Got PositionIDRequest");
                    OnPositionIDRequestReceived(new PositionIDRequestReceivedEventArgs());
                    break;
                case MessageType.ArrowPlacement:
                    ReadArrowPlacement(remainder);
                    break;
            }
        }

        /// <summary>
        /// Decodes the image message into a SoftwareBitmap and raises the
        /// BitmapReceived event
        /// </summary>
        /// <param name="msg">The contents of the image message</param>
        private async void ReadJpeg(byte[] msg)
        {
            System.Diagnostics.Debug.WriteLine("Building JpegReceivedEventArgs");
            //MemoryStream thing1 = new MemoryStream(msg);
            //BitmapDecoder decoder = await BitmapDecoder.CreateAsync(thing1.AsRandomAccessStream());
            //SoftwareBitmap result = await decoder.GetSoftwareBitmapAsync();

            OnJpegReceived(new JpegReceivedEventArgs(msg));
        }
        
        /// <summary>
        /// Decodes the image message into a SoftwareBitmap and raises the
        /// BitmapReceived event
        /// </summary>
        /// <param name="msg">The contents of the image message</param>
        private async void ReadArrowPlacement(byte[] msg)
        {
            System.Diagnostics.Debug.WriteLine("Building ArrowPlacementReceivedEventArgs");
            int id = BitConverter.ToInt32(msg, 0);
            int width = BitConverter.ToInt16(msg, 4);
            int height = BitConverter.ToInt16(msg, 6);
            int x = BitConverter.ToInt16(msg, 8);
            int y = BitConverter.ToInt16(msg, 10);

            OnArrowPlacementReceived(new ArrowPlacementReceivedEventArgs(id, width, height, x, y));
        }
        
#endif
        public event EventHandler<JpegReceivedEventArgs> JpegReceived;
        public event EventHandler<PositionIDRequestReceivedEventArgs> PositionIDRequestReceived;
        public event EventHandler<ArrowPlacementReceivedEventArgs> ArrowPlacementReceived;
#if NETFX_CORE

        /// <summary>
        /// Raises the BitmapReceived event
        /// (Does this need to be a separate function?)
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnJpegReceived(JpegReceivedEventArgs e)
        {
            JpegReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the PositionIDRequestReceived event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPositionIDRequestReceived(PositionIDRequestReceivedEventArgs e)
        {
            PositionIDRequestReceived?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the PositionIDRequestReceived event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnArrowPlacementReceived(ArrowPlacementReceivedEventArgs e)
        {
            ArrowPlacementReceived?.Invoke(this, e);
        }

        /// <summary>
        /// The current connection
        /// </summary>
        private StreamSocket _socket;
#endif
    }
}