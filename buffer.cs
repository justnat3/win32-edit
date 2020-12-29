using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace win32editor
{
    struct _Buffer
    {
        private string   bufferName;
        private int      numChars;
        private int      numlines;
        private DateTime fileTime;
        private bool     _isModified;

        public _Buffer(string bufferName, int numChars, int numlines, DateTime fileTime, bool _isModified)
        {
            this.bufferName  = bufferName;
            this.numChars    = numChars;
            this.numlines    = numlines;
            this.fileTime    = fileTime;
            this._isModified = _isModified;
        }
    }

}
