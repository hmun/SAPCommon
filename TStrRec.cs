using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace SAPCommon
{
    public class TStrRec
    {
        private SAPFormat sapFormat;

        private TField STRUCNAME;
        private TField FIELDNAME;
        private TField VALUE;
        private TField CURRENCY;
        private TField FORMAT;
        public TStrRec()
        {
            STRUCNAME = new TField();
            FIELDNAME = new TField();
            VALUE = new TField();
            CURRENCY = new TField();
            FORMAT = new TField();
        }

        public TStrRec(string pSTRUCNAME, string pFIELDNAME, string pVALUE, string pCURRENCY = "", string pFORMAT = "")
        {
            STRUCNAME = new TField("STRUCNAME", pSTRUCNAME);
            FIELDNAME = new TField("FIELDNAME", pFIELDNAME);
            if (String.IsNullOrEmpty(pCURRENCY))
                VALUE = new TField("VALUE", pVALUE,"S");
            else
                VALUE = new TField("VALUE", pVALUE,"F");
            CURRENCY = new TField("CURRENCY ", pCURRENCY);
            FORMAT = new TField("FORMAT ", pFORMAT);
        }

        public void setValues(String pSTRUCNAME, String pFIELDNAME, String pVALUE, String pCURRENCY = "", String pFORMAT = "")
        {
            STRUCNAME = new TField("STRUCNAME", pSTRUCNAME);
            FIELDNAME = new TField("FIELDNAME", pFIELDNAME);
            if (String.IsNullOrEmpty(pCURRENCY))
                VALUE = new TField("VALUE", pVALUE, "S");
            else
                VALUE = new TField("VALUE", pVALUE, "F");
            CURRENCY = new TField("CURRENCY ", pCURRENCY);
            FORMAT = new TField("FORMAT ", pFORMAT);
        }

        public void addValues(String pSTRUCNAME, String pFIELDNAME, String pVALUE, String pCURRENCY = "", String pFORMAT = "")
        {
            STRUCNAME = new TField("STRUCNAME", pSTRUCNAME);
            FIELDNAME = new TField("FIELDNAME", pFIELDNAME);
            VALUE.Add(Convert.ToDouble(pVALUE, CultureInfo.CurrentCulture));
            CURRENCY = new TField("CURRENCY ", pCURRENCY);
            FORMAT = new TField("FORMAT ", pFORMAT);
        }

        public void subValues(String pSTRUCNAME, String pFIELDNAME, String pVALUE, String pCURRENCY = "", String pFORMAT = "")
        {
            STRUCNAME = new TField("STRUCNAME", pSTRUCNAME);
            FIELDNAME = new TField("FIELDNAME", pFIELDNAME);
            VALUE.Sub(Convert.ToDouble(pVALUE, CultureInfo.CurrentCulture));
            CURRENCY = new TField("CURRENCY ", pCURRENCY);
            FORMAT = new TField("FORMAT ", pFORMAT);
        }

        public void mulValues(String pSTRUCNAME, String pFIELDNAME, String pVALUE, String pCURRENCY = "", String pFORMAT = "")
        {
            STRUCNAME = new TField("STRUCNAME", pSTRUCNAME);
            FIELDNAME = new TField("FIELDNAME", pFIELDNAME);
            VALUE.Mul(Convert.ToDouble(pVALUE, CultureInfo.CurrentCulture));
            CURRENCY = new TField("CURRENCY ", pCURRENCY);
            FORMAT = new TField("FORMAT ", pFORMAT);
        }

        public void divValues(String pSTRUCNAME, String pFIELDNAME, String pVALUE, String pCURRENCY = "", String pFORMAT = "")
        {
            STRUCNAME = new TField("STRUCNAME", pSTRUCNAME);
            FIELDNAME = new TField("FIELDNAME", pFIELDNAME);
            VALUE.Div(Convert.ToDouble(pVALUE, CultureInfo.CurrentCulture));
            CURRENCY = new TField("CURRENCY ", pCURRENCY);
            FORMAT = new TField("FORMAT ", pFORMAT);
        }

        public string formated()
        {
            sapFormat = new SAPFormat();
            return formated_int();
        }

        public string formated(TStr intPar)
        {
            sapFormat = new SAPFormat(intPar);
            return formated_int();
        }

        public string formated_int()
        {
            string aRet = "";
            int aDec = 2;
            string aDecFormat = "F";

            if (!String.IsNullOrEmpty(Currency))
            {
                if (!String.IsNullOrEmpty(Format) && Format.Substring(0, 1) == "D")
                    aDec = Convert.ToInt32(Format.Substring(1), CultureInfo.CurrentCulture);
                aDecFormat += aDec.ToString(CultureInfo.CurrentCulture);
                if (!String.IsNullOrEmpty(Value))
                    // aRet = Convert.ToDouble(Value).ToString(aDecFormat, CultureInfo.CreateSpecificCulture("en-US"));
                    aRet = Convert.ToDouble(Value, CultureInfo.CurrentCulture).ToString(aDecFormat, CultureInfo.CurrentCulture);
                else
                    // aRet = Convert.ToDouble("0").ToString(aDecFormat, CultureInfo.CreateSpecificCulture("en-US"));
                    aRet = Convert.ToDouble("0", CultureInfo.CurrentCulture).ToString(aDecFormat, CultureInfo.CurrentCulture);
            }
            else
            {
                switch (Format)
                {
                    case "DATE":
                        aRet = Convert.ToDateTime(Value, CultureInfo.CurrentCulture).ToString("yyyyMMdd", CultureInfo.CurrentCulture);
                        break;
                    case "PERIO":
                        char aDot = '.';
                        int idx = Value.IndexOf(aDot);
                        if (idx >= 0)
                            aRet = Value.Substring(idx+1) + (Convert.ToInt32(Value.Substring(0,idx), CultureInfo.CurrentCulture)).ToString("D3", CultureInfo.CurrentCulture);
                        else
                            aRet = Value;
                       break;
                    default:
                        if (!String.IsNullOrEmpty(Format) && Format.Substring(0, 1) == "U")
                            aRet = sapFormat.unpack(Value, Convert.ToInt32(Format.Substring(1), CultureInfo.CurrentCulture));
                        else if (!String.IsNullOrEmpty(Format) && Format.Substring(0, 1) == "T")
                            aRet = sapFormat.text(Value, Convert.ToInt32(Format.Substring(1), CultureInfo.CurrentCulture));
                        else if (!String.IsNullOrEmpty(Format) && Format.Substring(0, 1) == "R")
                            aRet = sapFormat.text_right(Value, Convert.ToInt32(Format.Substring(1), CultureInfo.CurrentCulture));
                        else if (!String.IsNullOrEmpty(Format) && Format.Substring(0, 1) == "P")
                            aRet = sapFormat.pspid(Value, Convert.ToInt32(Format.Substring(1), CultureInfo.CurrentCulture));
                        else
                            aRet = Value;
                        break;
                }
            }
            return aRet;
        }

        public string getKey()
        {
            return STRUCNAME.Value + "-" + FIELDNAME.Value;
        }

        public string getKeyR()
        {
            return STRUCNAME.Value + "-" + FIELDNAME.Value;
        }


        public string Strucname
        {
            get { return this.STRUCNAME.Value; }
            set { this.STRUCNAME.Value = value; }
        }

        public string Fieldname
        {
            get { return this.FIELDNAME.Value; }
            set { this.FIELDNAME.Value = value; }
        }

        public string Value
        {
            get { return this.VALUE.Value; }
            set { this.VALUE.Value = value; }
        }

        public string Currency
        {
            get { return this.CURRENCY.Value; }
            set { this.CURRENCY.Value = value; }
        }

        public string Format
        {
            get { return this.FORMAT.Value; }
            set { this.FORMAT.Value = value; }
        }

    }

}
