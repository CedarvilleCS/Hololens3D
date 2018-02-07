using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.UIScripts.ImageGallery
{
    public class PanoImage
    {
        public HLNetwork.ImagePosition position;
        public byte[] image;

        public PanoImage(byte[] image, HLNetwork.ImagePosition position)
        {
            this.image = image;
            this.position = position;
        }

        public byte[] toByteArray()
        {
            return null;
        }
    }
}
