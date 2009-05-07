using System;
using System.Collections.Generic;
using System.Text;
using Hubble.Core.Data;

namespace Hubble.Core.Query
{
    public interface IQuery
    {
        //Input parameters
        string FieldName { get; set;}
        int TabIndex { get; set; }

        string Command { get; }

        DBProvider DBProvider { get; set; }

        IList<Entity.WordInfo> QueryWords { get; set; }

        //Inner parameters
        //need not set by caller
        Index.InvertedIndex InvertedIndex { get; set;}
        //Analysis.IAnalyzer Analyzer { get; set;}

        //output
        Dictionary<long, DocumentRank> Search();
        IEnumerable<DocumentRank> GetRankEnumerable();
        //IList<Entity.WordInfo> GetQueryWords();
        //IList<Entity.WordInfo> GetNextHitWords(out long docId);
    }
}
