using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SAPCommon
{
    public class TOutData
    {
        private Dictionary<string, SortedDictionary<UInt64, object>> tDataDic;
        private SAPCommon.TStr aPar;

        public Dictionary<string, SortedDictionary<UInt64, object>> aTDataDic
        {
            get { return (Dictionary<string, SortedDictionary<UInt64, object>>)this.tDataDic; }
            //            set { this.tPostingDataRecDic = aTPostingDataRecDic; }
        }

        public TOutData()
        {
            tDataDic = new Dictionary<string, SortedDictionary<UInt64, object>>();
        }

        public void addValue(string pKey, UInt64 pLine, object pValue)
        {
            SortedDictionary<UInt64, object> aTDataList;
            if (aTDataDic.ContainsKey(pKey))
            {
                aTDataList = aTDataDic[pKey];
                aTDataList.Add(pLine, pValue);
            }
            else
            {
                aTDataList = new SortedDictionary<UInt64, object>();
                aTDataList.Add(pLine, pValue);
                aTDataDic.Add(pKey, aTDataList);
            }
        }

        public object[,] toArray(string pKey)
        {
            SortedDictionary<UInt64, object> aTDataList;
            object[,] _toArray = null;
            if (aTDataDic.ContainsKey(pKey))
            {
                aTDataList = aTDataDic[pKey];
                if (aTDataList.Keys.Count > 0)
                {
                    UInt64 aLastKeyVal = aTDataList.Keys.Last();
                    object[,] aRes = new object[aLastKeyVal - 1 + 1, 1];
                    foreach (var itemKey in aTDataList.Keys)
                        aRes[itemKey - 1, 0] = aTDataList[itemKey];
                    _toArray = aRes;
                }
            }
            return _toArray;
        }
    }
}
