using System;

namespace HLNetwork
{
    /// <summary>
    /// Event arguments object for a bitmap received from the network
    /// </summary>
    public class JpegReceivedEventArgs : EventArgs
    {

        public JpegReceivedEventArgs(byte[] incomingJpeg)
        {
            Image = incomingJpeg;
        }

        public byte[] Image { get; private set; }

    }
    public class PositionIDRequestReceivedEventArgs : EventArgs
    {

        public PositionIDRequestReceivedEventArgs()
        {
            
        }
    }
}