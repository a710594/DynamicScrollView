using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicScrollView
{
    public class ScrollViewData
    {
        public Color OriginalColor;

        public virtual int GetHeight() 
        {
            return 1;
        }
    }
}
