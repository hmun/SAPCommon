using System;
using System.Collections.Generic;
using System.Text;

namespace SAPCommon
{
    class TExt
    {

        private Dictionary<string, TExtRec> data;

        public TExt()
        {
            data = new Dictionary<string, TExtRec>();
        }
        public void add(string pSTRUCNAME, string pFIELDNAME, string pPOSITION, string pLENGTH)
        {
            string aKey = pSTRUCNAME + "-" + pFIELDNAME;
            TExtRec aTExtRec;
            if (!data.TryGetValue(aKey, out aTExtRec))
                aTExtRec = new TExtRec();
            aTExtRec.setValues(pSTRUCNAME, pFIELDNAME, pPOSITION, pLENGTH);
            data[aKey] = aTExtRec;
        }

        public void add(TStrRec pTStrRec)
        {
            TExtRec aTExtRec = new TExtRec(pTStrRec);
            if (!String.IsNullOrEmpty(aTExtRec.Strucname) && !String.IsNullOrEmpty(aTExtRec.Fieldname))
            {
                string aKey = aTExtRec.Strucname + "-" + aTExtRec.Fieldname;
                data[aKey] = aTExtRec;
            }
        }

        public TExtRec get(String pSTRUCNAME, String pFIELDNAME)
        {
            string aKey = String.IsNullOrEmpty(pSTRUCNAME) ? pFIELDNAME : pSTRUCNAME + "-" + pFIELDNAME;
            TExtRec aTExtRec;
            if (!data.TryGetValue(aKey, out aTExtRec))
                aTExtRec = new TExtRec();
            return aTExtRec;
        }

        public TExtRec get(String pName)
        {
            string aKey = pName;
            TExtRec aTExtRec;
            if (!data.TryGetValue(aKey, out aTExtRec))
                aTExtRec = new TExtRec();
            return aTExtRec;
        }

        public Dictionary<string, TExtRec> getData()
        {
            return this.data;
        }

        public Dictionary<string, TExtRec> getData(string pSTRUCNAME)
        {
            Dictionary<string, TExtRec> aRet = new Dictionary<string, TExtRec>();
            foreach (KeyValuePair<string, TExtRec> kvp in this.data)
            {
                if (kvp.Value.Strucname == pSTRUCNAME)
                    aRet.Add(kvp.Key, kvp.Value);
            }
            return aRet;
        }

        public void setData(Dictionary<string, TExtRec> pData)
        {
            this.data = pData;
        }

    }
}
