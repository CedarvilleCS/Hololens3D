using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UIScripts.ImageGallery
{
    public class PanoImage
    {
        public HLNetwork.ImagePosition Position;
        public byte[] Image;

        public PanoImage(byte[] image, HLNetwork.ImagePosition position)
        {
            this.Image = image;
            this.Position = position;
        }

        public byte[] ToByteArray()
        {
            byte[] finalBytes = new byte[Image.Length + 44];
            byte[] positionBytes = Position.ToByteArray();
            Buffer.BlockCopy(positionBytes, 0, finalBytes, 0, 44);
            Buffer.BlockCopy(Image, 0, finalBytes, 44, Image.Length);
            return finalBytes;
        }

        public static PanoImage FromByteArray(byte[] bytes)
        {
            byte[] positionBytes = new byte[44];
            byte[] imageBytes = new byte[bytes.Length - 44];
            Buffer.BlockCopy(bytes, 0, positionBytes, 0, 44);
            Buffer.BlockCopy(bytes, 44, imageBytes, 0, bytes.Length - 44);
            HLNetwork.ImagePosition imagePosition = HLNetwork.ImagePosition.FromByteArray(positionBytes);
            return new PanoImage(imageBytes, imagePosition);
        }
    }
}
