using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Threading;
using System.IO;
using Asmodat.Abbreviate;
using Asmodat.Types;

using Asmodat.Networking;
using Asmodat.Extensions.Drawing;
using Asmodat.Debugging;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Extensions;

namespace Asmodat.Types
{
    public partial class TickTimeoutMatrix<T> where T : ICloneable
    {
        public int Length
        {
            get
            {
                return (Width * Height);
            }
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        private T[,] _Matrix = null;
        public T[,] Matrix
        {
            get
            {
                return _Matrix;
            }
            set
            {
                if (value == null)
                {
                    this.Clear();
                    return;
                }

                if (!value.EqualSize(this._Matrix))
                {
                    this.Width = value.Width();
                    this.Height = value.Height();
                    this._Timeout = new TickTimeout[this.Width, this.Height];
                }

                this._Matrix = value;
            }
        }

        public TickTimeout[,] _Timeout = null;
        public TickTimeout[,] Timeout
        {
            get
            {
                return _Timeout;
            }
            set
            {
                if (value == null)
                {
                    this.Clear();
                    return;
                }

                if (!value.EqualSize(this._Timeout))
                {
                    this.Width = value.Width();
                    this.Height = value.Height();
                    this._Matrix = new T[this.Width, this.Height];
                }

                _Timeout = value;
            }
        }
        
        /// <summary>
        /// refreshes non null elements
        /// </summary>
        /// <param name="matrix"></param>
        public bool Refresh(T[,] matrix)
        {
            if (matrix.IsNullOrEmpty())
                return false;

            if (!matrix.EqualSize(this.Matrix))
                return false;

            int xParts = matrix.Width();
            int yParts = matrix.Height();

            int x = 0, y;
            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    if (_Timeout[x, y] != null && matrix[x,y] != null)
                    {
                        _Matrix[x, y] = matrix[x, y];
                        _Timeout[x, y].Reset();
                    }
                }
            }

            return true;
        }


        public TickTimeout GetLastTimeout(long min = 0, long max = long.MaxValue)
        {
            if (_Timeout.IsNullOrEmpty())
                return null;
           
            int xParts = _Timeout.Width();
            int yParts = _Timeout.Height();

            decimal select = max;
            TickTimeout result = null;
            int x = 0, y;
            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    TickTimeout tt = _Timeout[x, y];
                    if (!tt.IsEnabled())
                        continue;

                    if (tt.Span.InClosedInterval(min, max) && tt.Span <= select)
                    {
                        select = tt.Span;
                        result = tt.Copy();
                    }
                }
            }

            return result;
        }

        public TickTimeout GetFirstTimeout(long min = 0, long max = long.MaxValue)
        {
            if (_Timeout.IsNullOrEmpty())
                return null;

            int xParts = _Timeout.Width();
            int yParts = _Timeout.Height();

            decimal select = min;
            int sx = -1, sy = -1;

            int x = 0, y;
            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    TickTimeout tt = _Timeout[x, y];

                    if (!tt.IsEnabled())
                        continue;

                    if (tt.Span.InClosedInterval(min, max) && tt.Span >= select)
                    {
                        select = tt.Span;
                        sx = x;
                        sy = y;
                    }
                }
            }

            if(sx >= 0 && sy >= 0)
                return _Timeout[sx, sy].Copy();
            

            return null;
        }

        public void SetTimeouts(TickTimeout value)
        {
            this._Timeout.PopulateClone(value);
        }

        public void Clear()
        {
            Width = 0;
            Height = 0;
            this._Timeout = null;
            this._Matrix = null;
        }

        public TickTimeoutMatrix<T> Copy()
        {
            TickTimeoutMatrix<T> tbm = new TickTimeoutMatrix<T>();
            tbm.Matrix = this.Matrix.Copy();
            tbm.Timeout = this.Timeout.Copy();
            return tbm;
        }


    }

}
