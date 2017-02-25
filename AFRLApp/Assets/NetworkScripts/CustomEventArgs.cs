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

    public class ArrowPlacementReceivedEventArgs : EventArgs
    {

        public ArrowPlacementReceivedEventArgs(int id, int width, int height, int x, int y)
        {
            this.id = id;
            this.width = width;
            this.height = height;
            this.x = x;
            this.y = y;
        }

        public int id { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }
    }
}