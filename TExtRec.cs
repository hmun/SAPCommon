using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace SAPCommon
{
    class TExtRec
    {
        private TField STRUCNAME;
        private TField FIELDNAME;
        private TField POSITION;
        private TField LENGTH;
        private TField VALUE;

        public TExtRec()
        {
            STRUCNAME = new TField();
            FIELDNAME = new TField();
            POSITION = new TField();
            LENGTH = new TField();
        }

        public TExtRec(string pSTRUCNAME, string pFIELDNAME, string pPOSITION, string pLENGTH)
        {
            STRUCNAME = new TField("STRUCNAME", pSTRUCNAME);
            FIELDNAME = new TField("FIELDNAME", pFIELDNAME);
            POSITION = new TField("POSITION ", pPOSITION);
            LENGTH = new TField("LENGTH ", pLENGTH);
        }

        public TExtRec(TStrRec pTStrRec)
        {
            if (pTStrRec.Strucname == "EXT")
            { 
                string[] parts = pTStrRec.Fieldname.Split('|');
                if (parts.Length == 3)
                {
                    STRUCNAME = new TField("STRUCNAME", parts[0]);
                    POSITION = new TField("POSITION", parts[1]);
                    FIELDNAME = new TField("FIELDNAME", parts[2]);
                    LENGTH = new TField("LENGTH ", pTStrRec.Value);
                }
            }
        }

        public void setValues(String pSTRUCNAME, String pFIELDNAME, String pPOSITION, String pLENGTH)
        {
            STRUCNAME = new TField("STRUCNAME", pSTRUCNAME);
            FIELDNAME = new TField("FIELDNAME", pFIELDNAME);
            POSITION = new TField("POSITION ", pPOSITION);
            LENGTH = new TField("LENGTH ", pLENGTH);
        }

        public string getKey()
        {
            return STRUCNAME.Value + "|" + POSITION.Value + "|" + FIELDNAME.Value;
        }

        public string getKeyR()
        {
            return STRUCNAME.Value + "|" + POSITION.Value + "|" + FIELDNAME.Value;
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

        public string Position
        {
            get { return this.POSITION.Value; }
            set { this.POSITION.Value = value; }
        }

        public int Pos
        {
            get { return Convert.ToUInt16(this.POSITION.Value, CultureInfo.CurrentCulture); }
            set { this.POSITION.Value = value.ToString(CultureInfo.CurrentCulture.NumberFormat); }
        }

        public int Len
        {
            get { return Convert.ToUInt16(this.LENGTH.Value, CultureInfo.CurrentCulture); }
            set { this.LENGTH.Value = value.ToString(CultureInfo.CurrentCulture.NumberFormat); }
        }

        public string Length
        {
            get { return this.LENGTH.Value; }
            set { this.LENGTH.Value = value; }
        }

    }

}
