using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Hubble.Core.Query
{
    public class DocRankRadixSortedList : IEnumerable<DocumentRank>
    {
        const int TableSize = 260;
        int _Count = 0;
        int _Top = int.MaxValue;

        int _MaxRadix = -1;
        int _MinRadix = 0;

        List<DocumentRank>[] _RadixTable = new List<DocumentRank>[TableSize];
        bool[] _RadixSortedTable = new bool[TableSize];

        public int Count
        {
            get
            {
                return _Count;
            }
        }

        public int Top
        {
            get
            {
                return _Top;
            }

            set
            {
                if (value <= 0)
                {
                    _Top = 1;
                }
                else
                {
                    _Top = value;
                }
            }
        }

        private void CalculateMinRadix()
        {
            int count = 0;
            for (int i = _MaxRadix; i >= _MinRadix; i--)
            {
                if (_RadixTable[i] != null)
                {
                    count += _RadixTable[i].Count;
                }

                if (count > Top)
                {
                    _MinRadix = i;
                    break;
                }
            }
        }

        public DocRankRadixSortedList(int top)
        {
            _Top = top;
        }

        public DocRankRadixSortedList()
        {
        }

        public void Add(DocumentRank docRank)
        {
            Debug.Assert(docRank.Rank >= 0);

            int radix;

            if (docRank.Rank < 65536)
            {
                radix = docRank.Rank / 256;
            }
            else if (docRank.Rank < 100000)
            {
                radix = 256;
            }
            else if (docRank.Rank < 1000000)
            {
                radix = 257;
            }
            else if (docRank.Rank < 10000000)
            {
                radix = 258;
            }
            else
            {
                radix = 259;
            }

            if (radix < _MinRadix)
            {
                _Count++;
                return;
            }

            if (_RadixTable[radix] == null)
            {
                _RadixTable[radix] = new List<DocumentRank>();

                if (_MaxRadix < radix)
                {
                    _MaxRadix = radix;
                }
            }

            _RadixTable[radix].Add(docRank);

            _Count++;

            if (_Top < int.MaxValue)
            {
                if (_Count % Top == 0)
                {
                    CalculateMinRadix();
                }
            }
        }


        #region IEnumerable<DocumentRank> Members

        public IEnumerator<DocumentRank> GetEnumerator()
        {
            int radix = 259;
            int curIndex = 0;
            int count = 0;

            while (radix >= 0)
            {
                if (_RadixTable[radix] != null)
                {
                    if (!_RadixSortedTable[radix])
                    {
                        _RadixTable[radix].Sort();
                        _RadixSortedTable[radix] = true;
                    }

                    yield return _RadixTable[radix][curIndex];
                    count++;
                    if (count >= Top)
                    {
                        yield break;
                    }

                    curIndex++;

                    if (curIndex >= _RadixTable[radix].Count)
                    {
                        curIndex = 0;
                        radix--;
                    }
                }
                else
                {
                    radix--;
                }
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
