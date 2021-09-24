using System;
using System.Collections.Generic;
using System.Text;

namespace SAPCommon
{ 
    public class TStr
    {
        private Dictionary<string, TStrRec> data;

        public TStr()
        {
            data = new Dictionary<string, TStrRec>();
        }
        public void add(String pSTRUCNAME, String pFIELDNAME, String pVALUE, String pCURRENCY = "", String pFORMAT = "")
        {
            string aKey = pSTRUCNAME + "-" + pFIELDNAME;
            TStrRec aTStrRec;
            if (!data.TryGetValue(aKey, out aTStrRec))
                aTStrRec = new TStrRec();
            aTStrRec.setValues(pSTRUCNAME, pFIELDNAME, pVALUE, pCURRENCY, pFORMAT);
            data[aKey] = aTStrRec;
        }

        public void add(String pName, String pVALUE, String pCURRENCY = "", String pFORMAT = "")
        {
            if (String.IsNullOrEmpty(pName))
                return;
            string aKey = pName;
            string aSTRUCNAME = "";
            string aFIELDNAME = "";
            TStrRec aTStrRec;
            String[] spearator = { "-", "__" };
            String[] strlist = pName.Split(spearator, 2, StringSplitOptions.RemoveEmptyEntries);
            if (strlist.Length == 2)
            { 
                aSTRUCNAME = strlist[0];
                aFIELDNAME = strlist[1];
            }
            else
            { 
                aSTRUCNAME = "";
                aFIELDNAME = pName;
            }
            if (!data.TryGetValue(aKey, out aTStrRec))
                aTStrRec = new TStrRec();
            aTStrRec.setValues(aSTRUCNAME, aFIELDNAME, pVALUE, pCURRENCY, pFORMAT);
            data[aKey] = aTStrRec;
        }

        public string value(String pSTRUCNAME, String pFIELDNAME)
        {
            string aKey = String.IsNullOrEmpty(pSTRUCNAME) ? pFIELDNAME : pSTRUCNAME + "-" + pFIELDNAME;
            TStrRec aTStrRec;
            if (!data.TryGetValue(aKey, out aTStrRec))
                return "";
            return aTStrRec.Value;
        }

        public string value(String pName)
        {
            string aKey = pName;
            TStrRec aTStrRec;
            if (!data.TryGetValue(aKey, out aTStrRec))
                return "";
            return aTStrRec.Value;
        }

        public Dictionary<string, TStrRec> getData()
        {
            return this.data;
        }

        public Dictionary<string, TStrRec> getData(string pSTRUCNAME)
        {
            Dictionary<string, TStrRec> aRet = new Dictionary<string, TStrRec>();
            foreach (KeyValuePair<string, TStrRec> kvp in this.data)
            {
                if (kvp.Value.Strucname == pSTRUCNAME)
                    aRet.Add(kvp.Key, kvp.Value);
            }
            return aRet;
        }

        public void setData(Dictionary<string, TStrRec> pData)
        {
           this.data = pData;
        }

    }
}
