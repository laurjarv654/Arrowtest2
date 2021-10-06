using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arrowtest2
{
    class Arrow
    {
        public int x, y, size, arrowNum;
        Image image;

        public Arrow(int _x, int _y, int _size, int _arrowNum, Image _image)
        {
            x = _x;
            y = _y;
            size = _size;
            arrowNum = _arrowNum;
            image = _image;
        }

       public Image getImage()
        {
            return image;
        }
    }
}
