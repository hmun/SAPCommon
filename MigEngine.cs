using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace SAPCommon
{
    public class MigEngine
    {
        private MigHelper mh;
        private SAPCommon.TStr intPar;
        private string nr;
        private TPostingData tPostingData;
        private CultureInfo cc = CultureInfo.CurrentCulture;

        public MigHelper aMh
        {
            get { return (MigHelper)this.mh; }
        }

        public TStr aIntPar
        {
            get { return (TStr)this.intPar; }
        }

        public string aNr
        {
            get { return (string)this.nr; }
        }

        public TPostingData aTPostingData
        {
            get { return (TPostingData)this.tPostingData; }
        }

        public MigEngine(ref MigHelper pMh, ref SAPCommon.TStr pPar, string pNr)
        {
            mh = pMh;
            intPar = pPar;
            nr = pNr;
            tPostingData = new TPostingData(ref intPar, nr);
        }

        public void migrate(ref object[,] pNamArray, ref object[,] pValArray)
        {
            Collection<Dictionary<string, TField>> aBasis = new Collection<Dictionary<string, TField>>();
            Dictionary<string, SAPCommon.TField> aPostingLine = new Dictionary<string, SAPCommon.TField>();
            Dictionary<string, SAPCommon.TField> aContraLine = new Dictionary<string, SAPCommon.TField>();
            aBasis = mh.makeCollectionForRules(ref pNamArray, ref pValArray);
            pNamArray = null;
            pValArray = null;
            TPostingDataRec aTPostingDataRec;
            string aTPostingDataRecKey;
            UInt64 aTPostingDataRecNum = 1;

            tPostingData = new TPostingData(ref intPar, nr);

            // should we compress posting lines?
            string aGenCompData = !String.IsNullOrEmpty(intPar.value("GEN" + nr, "COMP_DATA")) ? intPar.value("GEN" + nr, "COMP_DATA") : "";
            bool aCompress = aGenCompData == "X" ? true : false;
            // should we suppress line with zero values
            string aGenSupprZero = !String.IsNullOrEmpty(intPar.value("GEN" + nr, "SUPPR_ZERO")) ? intPar.value("GEN" + nr, "SUPPR_ZERO") : "";
            bool aSupprZero = aGenSupprZero == "X" ? true : false;
            string aEmptyChar = intPar.value("GEN" + nr, "CHAR_EMPTY");
            string aIgnoreEmpty = !String.IsNullOrEmpty(intPar.value("GEN" + nr, "IGNORE_EMPTY")) ? intPar.value("GEN" + nr, "IGNORE_EMPTY") : "X";
            bool aGenEmpty = aIgnoreEmpty == "X" ? false : true;

            // create the posting lines
            UInt32 k = 1;
            foreach (Dictionary<string, TField> aBasisLine in aBasis)
            {
                aPostingLine = mh.mig.ApplyRules(aBasisLine, "P");
                if (aPostingLine.Count > 0)
                {
                    aTPostingDataRec = tPostingData.newTPostingDataRec(pDic: ref aPostingLine, pEmpty: aGenEmpty, pEmptyChar: aEmptyChar);
                    if (aCompress)
                        aTPostingDataRecKey = aTPostingDataRec.getKey();
                    else
                        aTPostingDataRecKey = Convert.ToString(aTPostingDataRecNum, cc);
                    tPostingData.addTPostingDataRec(aTPostingDataRecKey, aTPostingDataRec);
                    aTPostingDataRecNum += 1;
                }
                aContraLine = mh.mig.ApplyRules(aBasisLine, "C");
                if (aContraLine.Count > 0)
                {
                    aTPostingDataRec = tPostingData.newTPostingDataRec(pDic: ref aContraLine, pEmpty: aGenEmpty, pEmptyChar: aEmptyChar);
                    if (aCompress)
                        aTPostingDataRecKey = aTPostingDataRec.getKey();
                    else
                        aTPostingDataRecKey = Convert.ToString(aTPostingDataRecNum, cc);
                    tPostingData.addTPostingDataRec(aTPostingDataRecKey, aTPostingDataRec);
                    aTPostingDataRecNum += 1;
                }
                k += 1;
            }
        }

        public void ToTOutData(ref object[,] pKeyArray, ref TOutData pValueColumns, ref TOutData pFormulaColumns)
        {
            UInt64 aCnt = 1;
            int jmax = pKeyArray.GetUpperBound(1);
            // should we suppress line with zero values
            string aTargetFilter = !String.IsNullOrEmpty(intPar.value("GEN" + nr, "TARGET_FILTER")) ? intPar.value("GEN" + nr, "TARGET_FILTER") : "";
            string aGenSupprZero = !String.IsNullOrEmpty(intPar.value("GEN" + nr, "SUPPR_ZERO")) ? intPar.value("GEN" + nr, "SUPPR_ZERO") : "";
            bool aSupprZero = aGenSupprZero == "X" ? true : false;
            bool aSuppressLine;
            string aKey;
            string aValue;
            foreach (KeyValuePair<string, TPostingDataRec> aKvb in tPostingData.aTPostingDataDic)
            {
                aSuppressLine = false;
                TPostingDataRec aTPostingDataRec = aKvb.Value;
                if (aSupprZero & aTPostingDataRec.isZero())
                    aSuppressLine = true;
                if (!string.IsNullOrEmpty(aTargetFilter))
                {
                    if (isTargetFiltered(aTargetFilter, aTPostingDataRec))
                        aSuppressLine = true;
                }
                if (!aSuppressLine)
                {
                    for (var j = 1; j <= jmax; j++)
                    {
                        if (!String.IsNullOrEmpty(Convert.ToString(pKeyArray[1, j], cc)))
                        {
                            aKey = Convert.ToString(pKeyArray[1, j], cc);
                            if (!aKey.Contains("-"))
                                aKey = "-" + aKey;
                            if (aTPostingDataRec.aTPostingDataRecDic.ContainsKey(aKey))
                            {
                                aValue = aTPostingDataRec.aTPostingDataRecDic[aKey].Value;
                                if (aTPostingDataRec.aTPostingDataRecDic[aKey].Format == "F")
                                    pFormulaColumns.addValue(Convert.ToString(j, cc), aCnt, "=" + aValue);
                                else if (aTPostingDataRec.isValue(aKey))
                                    pValueColumns.addValue(Convert.ToString(j, cc), aCnt, Convert.ToDouble(aValue, cc));
                                else
                                    pValueColumns.addValue(Convert.ToString(j, cc), aCnt, aValue);
                            }
                        }
                    }
                    aCnt += 1;
                }
            }
        }

        private bool isTargetFiltered(string pTargetFilterStr, TPostingDataRec pTPostingDataRec)
        {
            string aFilterField = "";
            string aFilterOperation = "";
            string aFilterCompare = "";
            if (!string.IsNullOrEmpty(pTargetFilterStr))
            {
                string[] aFilterStr = Array.Empty<string>();
                aFilterStr = pTargetFilterStr.Split(';');
                if (aFilterStr.Length == 3)
                {
                    aFilterField = aFilterStr[0];
                    aFilterOperation = aFilterStr[1];
                    aFilterCompare = aFilterStr[2];
                    if (aFilterCompare.ToUpper() == "NULL")
                        aFilterCompare = "";
                }
            }
            bool _isTargetFiltered = false;
            SAPCommon.TStrRec aTStrRec;
            if (pTPostingDataRec.aTPostingDataRecDic.ContainsKey("-" + aFilterField))
            {
                aTStrRec = pTPostingDataRec.aTPostingDataRecDic["-" + aFilterField];
                if (aFilterOperation == "EQ" & aTStrRec.Value == aFilterCompare)
                _isTargetFiltered = true;
                else if (aFilterOperation == "NE" & aTStrRec.Value != aFilterCompare)
                _isTargetFiltered = true;
            }
            else if (aFilterOperation == "NE" & (string.IsNullOrEmpty(aFilterCompare) | aFilterCompare == "#"))
            _isTargetFiltered = false;
            else if (aFilterOperation == "EQ" & (string.IsNullOrEmpty(aFilterCompare) | aFilterCompare == "#"))
            _isTargetFiltered = true;
        return _isTargetFiltered;
        }
    }
}
