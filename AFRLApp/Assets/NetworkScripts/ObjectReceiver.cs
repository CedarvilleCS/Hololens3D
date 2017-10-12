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

    /// <summary>
    /// Represents the network connection for the Unity application.
    /// Provides events for when certain types of messages are received.
    /// Enables certain types of messages to be sent.
    /// Maintains the underlying socket.
    /// </summary>
    public class ObjectReceiver
    {

        #region Private Methods

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
#endif

        /// <summary>
        /// Determine the type of a received message and send it to the
        /// appropriate mechanism for interpretation
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
            // Send the remainder of the message to a handler for its 
            // specific type
            //

            switch ((MessageType)objTypeCode)
            {
                case MessageType.Image:
                    ReadJpeg(remainder);
                    break;
                case MessageType.PDF:
                    ReadPDF(remainder);
                    break;
                case MessageType.PositionIDRequest:
                    System.Diagnostics.Debug.WriteLine("Got PositionIDRequest");
                    OnPositionIDRequestReceived(new PositionIDRequestReceivedEventArgs());
                    break;
                case MessageType.MarkerPlacement:
                    ReadMarkerPlacement(remainder);
                    break;
                case MessageType.MarkerErasure:
                    ReadMarkerErasure(remainder);
                    break;
            }
        }

        /// <summary>
        /// Decodes the image message into a SoftwareBitmap and raises the
        /// BitmapReceived event
        /// </summary>
        /// <param name="msg">The contents of the image message</param>
        private void ReadJpeg(byte[] msg)
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
        private void ReadPDF(byte[] msg)
        {
            System.Diagnostics.Debug.WriteLine("Building JpegReceivedEventArgs");
            //MemoryStream thing1 = new MemoryStream(msg);
            //BitmapDecoder decoder = await BitmapDecoder.CreateAsync(thing1.AsRandomAccessStream());
            //SoftwareBitmap result = await decoder.GetSoftwareBitmapAsync();

            //TODO: parse pdf here
            PDFDocument pdf = null;
            OnJpegReceived(new PDFReceivedEventArgs(pdf));
        }
        
        /// <summary>
        /// Decodes the componentes of the MarkerPlacement message and raises the
        /// MarkerPlacementReceived event
        /// </summary>
        /// <param name="msg">The contents of the marker placement message</param>
        private void ReadMarkerPlacement(byte[] msg)
        {
            System.Diagnostics.Debug.WriteLine("Building MarkerPlacementReceivedEventArgs");
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(msg, 0, 4);
                Array.Reverse(msg, 4, 2);
                Array.Reverse(msg, 6, 2);
                Array.Reverse(msg, 8, 2);
                Array.Reverse(msg, 10, 2);
                Array.Reverse(msg, 12, 2);
            }
            int id = BitConverter.ToInt32(msg, 0);
            int width = BitConverter.ToInt16(msg, 4);
            int height = BitConverter.ToInt16(msg, 6);
            int x = BitConverter.ToInt16(msg, 8);
            int y = BitConverter.ToInt16(msg, 10);
            int dir = BitConverter.ToInt16(msg, 12);

            int r = 255, g = 255, b = 255;
            if (msg.Length >= 17)
            {
                r = msg[14];
                g = msg[15];
                b = msg[16];
            }

            OnMarkerPlacementReceived(new MarkerPlacementReceivedEventArgs(id, width, height, x, y, r, g, b, dir));
        }

        /// <summary>
        /// Decodes the componentes of the MarkerErasure message and raises the
        /// MarkerErasureReceived event
        /// </summary>
        /// <param name="msg">The contents of the marker erasure message</param>
        private void ReadMarkerErasure(byte[] msg)
        {
            System.Diagnostics.Debug.WriteLine("Building MarkerErasureReceivedEventArgs");
            bool all = true;
            int id = 0;
            if (msg.Length > 0)
            {
                all = false;
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(msg, 0, 4);
                }
                id = BitConverter.ToInt32(msg, 0);
            }

            OnMarkerErasureReceived(new MarkerErasureReceivedEventArgs(all, id));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Automatically starts listening on port 33334
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

        /// <summary>
        /// Get the singleton instance of the class
        /// </summary>
        /// <returns>the singleton instance of the class</returns>
        public static ObjectReceiver getTheInstance()
        {
            if (_theInstance == null)
            {
                _theInstance = new ObjectReceiver();
            }
            return _theInstance;
        }

        /// <summary>
        /// Sends a response to a PositionIDRequest over the network
        /// </summary>
        /// <param name="posID">The ID to send in the message</param>
        public void SendPositionIDResponse(int posID)
        {
            System.Diagnostics.Debug.WriteLine("Sending Position ID: " + posID);
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

#endregion

        #region Events

        public event EventHandler<JpegReceivedEventArgs> JpegReceived;
        public event EventHandler<PositionIDRequestReceivedEventArgs> PositionIDRequestReceived;
        public event EventHandler<PDFReceivedEventArgs> PDFReceived;
        public event EventHandler<MarkerPlacementReceivedEventArgs> MarkerPlacementReceived;
        public event EventHandler<MarkerErasureReceivedEventArgs> MarkerErasureReceived;
        
        ///
        /// The newer ?. operator is not used in the following methods
        /// in order to conform with Unity's older C# expectations.
        ///

        /// <summary>
        /// Raises the BitmapReceived event
        /// (Does this need to be a separate function?)
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnJpegReceived(JpegReceivedEventArgs e)
        {
            if (JpegReceived != null)
            {
                JpegReceived.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the PositionIDRequestReceived event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPositionIDRequestReceived(PositionIDRequestReceivedEventArgs e)
        {
            if (PositionIDRequestReceived != null)
            {
                PositionIDRequestReceived.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the BitmapReceived event
        /// (Does this need to be a separate function?)
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPDFReceived(PDFReceivedEventArgs e)
        {
            if (PDFReceived != null)
            {
                PDFReceived.Invoke(this, e);
            }
        }


        /// <summary>
        /// Raises the MarkerPlacementReceived event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMarkerPlacementReceived(MarkerPlacementReceivedEventArgs e)
        {
            if (MarkerPlacementReceived != null)
            {
                MarkerPlacementReceived.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the MarkerErasureReceived event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMarkerErasureReceived(MarkerErasureReceivedEventArgs e)
        {
            if (MarkerErasureReceived != null)
            {
                MarkerErasureReceived.Invoke(this, e);
            }
        }

        #endregion

        #region Fields

        /// <summary>
        /// Types of messages sent over the network connection
        /// </summary>
        private enum MessageType { Image = 1, PositionIDRequest = 2, MarkerPlacement = 3, MarkerErasure = 4, PDF = 5 }

        /// <summary>
        /// The singleton instance of this class
        /// </summary>
        private static ObjectReceiver _theInstance = null;

#if NETFX_CORE
        /// <summary>
        /// The listener for incoming connections
        /// </summary>
        private StreamSocketListener _socketListener;

        /// <summary>
        /// The current connection
        /// </summary>
        private StreamSocket _socket;
#endif

#endregion

    }

}