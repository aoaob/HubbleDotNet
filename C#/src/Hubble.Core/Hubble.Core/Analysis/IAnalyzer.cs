using System;
using System.Collections.Generic;
using System.Text;
using Hubble.Core.Entity;

namespace Hubble.Core.Analysis
{
    public interface IAnalyzer
    {
        IEnumerable<WordInfo> Tokenize(string text);
    }
}