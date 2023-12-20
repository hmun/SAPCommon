using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;

namespace SAPCommon
{
    public class MigHelper
    {
        public Migration mig;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string aFilterField = "";
        private string aFilterOperatiion = "";
        private string aFilterCompare = "";
        private CultureInfo cc = CultureInfo.CurrentCulture; 

        public MigHelper(ref Dictionary<string, object[,]> pConfigDic, ref TStr pIntPar, string pNr, bool pUselocal = false)
        {
            string configFile = "";
            string aBaseFilter = !String.IsNullOrEmpty(pIntPar.value("GEN" + pNr, "BASE_FILTER")) ? pIntPar.value("GEN" + pNr, "BASE_FILTER") : "";
            // set Filter Fields
            if (!string.IsNullOrEmpty(aBaseFilter))
            {
                string[] aFilterStr = Array.Empty<string>();
                aFilterStr = aBaseFilter.Split(';');
                if (aFilterStr.Length == 3)
                {
                    aFilterField = aFilterStr[0];
                    aFilterOperatiion = aFilterStr[1];
                    aFilterCompare = aFilterStr[2];
                    if (aFilterCompare.ToUpper() == "NULL")
                        aFilterCompare = "";
                }
            }
            // Check for local rules first
            if (!pUselocal)
            {
                log.Debug("MigHelper.New - using XML rules");
                System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                string assembly = assemblyName.Name;
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                configFile = Uri.UnescapeDataString(appData + @"\SapExcel\" + assembly + @"\mig_rules" + pNr + ".config");
                log.Debug("New - " + "looking for config file=" + configFile);
                if (!File.Exists(configFile))
                {
                    appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    configFile = Uri.UnescapeDataString(appData + @"\SapExcel\" + assembly + @"\mig_rules" + pNr + ".config");
                    log.Debug("New - " + "looking for config file=" + configFile);
                    if (!File.Exists(configFile))
                    {
                        appData = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).AbsolutePath;
                        appData = Path.GetDirectoryName(appData);
                        configFile = Uri.UnescapeDataString(appData + @"\mig_rules" + pNr + ".config");
                        log.Debug("New - " + "looking for config file=" + configFile);
                        if (!File.Exists(configFile))
                            configFile = "";
                    }
                }
            }
            // setup the migration engine
            if (!String.IsNullOrEmpty(configFile))
            {
                log.Debug("New - " + "found config file=" + configFile);
                mig = new Migration(configFile);
            }
            else
            {
                log.Debug("New - " + "No config file found looking for config dictionary");
                string aRwsName = !String.IsNullOrEmpty(pIntPar.value("GEN" + pNr, "WS_RULES")) ? pIntPar.value("GEN" + pNr, "WS_RULES") : "Rules";
                string aPwsName = !String.IsNullOrEmpty(pIntPar.value("GEN" + pNr, "WS_PATTERN")) ? pIntPar.value("GEN" + pNr, "WS_PATTERN") : "Pattern";
                string aCwsName = !String.IsNullOrEmpty(pIntPar.value("GEN" + pNr, "WS_CONSTANT")) ? pIntPar.value("GEN" + pNr, "WS_CONSTANT") : "Constant";
                string aMwsName = !String.IsNullOrEmpty(pIntPar.value("GEN" + pNr, "WS_MAPPING")) ? pIntPar.value("GEN" + pNr, "WS_MAPPING") : "Mapping";
                string aFwsName = !String.IsNullOrEmpty(pIntPar.value("GEN" + pNr, "WS_FORMULA")) ? pIntPar.value("GEN" + pNr, "WS_FORMULA") : "Formula";
                mig = new Migration();
                // try to read the rules from the excel workbook
                int i;
                try
                {
                    object[,] aArray = pConfigDic[aRwsName];
                    for (i = aArray.GetLowerBound(0); i <= aArray.GetUpperBound(0); i++)
                    {
                        int j = aArray.GetLowerBound(1);
                        mig.AddRule(Convert.ToString(aArray[i, j],cc), Convert.ToString(aArray[i, j + 1],cc), Convert.ToString(aArray[i, j + 2],cc), Convert.ToString(aArray[i, j + 3],cc));
                        if (Convert.ToString(aArray[i, j + 2],cc) == "C" & !string.IsNullOrEmpty(Convert.ToString(aArray[i, j + 4],cc)))
                            mig.AddConstant(Convert.ToString(aArray[i, j],cc), Convert.ToString(aArray[i, j + 1],cc), Convert.ToString(aArray[i, j + 4],cc));
                        if (Convert.ToString(aArray[i, j + 2],cc) == "P" & !string.IsNullOrEmpty(Convert.ToString(aArray[i, j + 4],cc)))
                            mig.AddPattern(Convert.ToString(aArray[i, j],cc), Convert.ToString(aArray[i, j + 1],cc), Convert.ToString(aArray[i, j + 4],cc));
                        if (Convert.ToString(aArray[i, j + 2],cc) == "F" & !string.IsNullOrEmpty(Convert.ToString(aArray[i, j + 4],cc)))
                            mig.AddFormula(Convert.ToString(aArray[i, j],cc), Convert.ToString(aArray[i, j + 1],cc), Convert.ToString(aArray[i, j + 4],cc));
                    }
                }
                catch (Exception Exc)
                {
                    log.Debug("No " + aRwsName + " in config dictionary.");
                }
                try
                {
                    object[,] aArray = pConfigDic[aPwsName];
                    for (i = aArray.GetLowerBound(0); i <= aArray.GetUpperBound(0); i++)
                    {
                        int j = aArray.GetLowerBound(1);
                        mig.AddPattern(Convert.ToString(aArray[i, j],cc), Convert.ToString(aArray[i, j + 1],cc), Convert.ToString(aArray[i, j + 2],cc));
                    }
                }
                catch (Exception Exc)
                {
                    log.Debug("No " + aPwsName + " in config dictionary.");
                }
                try
                {
                    object[,] aArray = pConfigDic[aCwsName];
                    for (i = aArray.GetLowerBound(0); i <= aArray.GetUpperBound(0); i++)
                    {
                        int j = aArray.GetLowerBound(1);
                        mig.AddConstant(Convert.ToString(aArray[i, j],cc), Convert.ToString(aArray[i, j + 1],cc), Convert.ToString(aArray[i, j + 2],cc));
                    }
                }
                catch (Exception Exc)
                {
                    log.Debug("No " + aCwsName + " in config dictionary.");
                }
                try
                {
                    object[,] aArray = pConfigDic[aFwsName];
                    for (i = aArray.GetLowerBound(0); i <= aArray.GetUpperBound(0); i++)
                    {
                        int j = aArray.GetLowerBound(1);
                        mig.AddFormula(Convert.ToString(aArray[i, j],cc), Convert.ToString(aArray[i, j + 1],cc), Convert.ToString(aArray[i, j + 2],cc));
                    }
                }
                catch (Exception Exc)
                {
                    log.Debug("No " + aFwsName + " in config dictionary.");
                }
                try
                {
                    object[,] aArray = pConfigDic[aMwsName];
                    for (i = aArray.GetLowerBound(0); i <= aArray.GetUpperBound(0); i++)
                    {
                        int j = aArray.GetLowerBound(1);
                        mig.AddMapping(Convert.ToString(aArray[i, j + 1],cc), Convert.ToString(aArray[i, j],cc), Convert.ToString(aArray[i, j + 2],cc), Convert.ToString(aArray[i, j + 3],cc));
                    }
                }
                catch (Exception Exc)
                {
                    log.Debug("No " + aMwsName + " in config dictionary.");
                }
            }
        }

        public void saveToConfig(string pNr)
        {
            System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            string assembly = assemblyName.Name;
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string configFile = appData + @"\SapExcel\" + assembly + @"\mig_rules" + pNr + ".config";
            Configuration config;
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = configFile;
            config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None) as Configuration;
            config.Sections.Add("MigRules", mig.MRS);
            mig.MRS.SectionInformation.ForceSave = true;
            config.Save(ConfigurationSaveMode.Full);
        }

        public bool isFiltered(ref Dictionary<string, TField> pBaseRecord)
        {
            TField aField;
            bool _isFiltered = false;
            if (pBaseRecord.ContainsKey(aFilterField))
            {
                aField = pBaseRecord[aFilterField];
                if (aFilterOperatiion == "EQ" & aField.Value == aFilterCompare)
                    _isFiltered = true;
                else if (aFilterOperatiion == "NE" & aField.Value != aFilterCompare)
                    _isFiltered = true;
            }
            else if (aFilterOperatiion == "NE" & (string.IsNullOrEmpty(aFilterCompare) | aFilterCompare == "#"))
                _isFiltered = false;
            return _isFiltered;
        }

        public Dictionary<string, TField> makeDictForRules(ref object[,] pNamArray, ref object[,] pValArray, int pFromCol, int pToCol)
        {
            Dictionary<string, TField> retDict = new Dictionary<string, TField>();
            TField tfield;
            for (var j = pFromCol; j <= pToCol; j++)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(pNamArray[1, j],cc)))
                {
                    if (mig.ContainsSource("P", Convert.ToString(pNamArray[1, j],cc)) | mig.ContainsSource("C", Convert.ToString(pNamArray[1, j],cc)))
                    {
                        tfield = new TField(Convert.ToString(pNamArray[1, j],cc), Convert.ToString(pValArray[1, j],cc));
                        retDict.Add(tfield.Name, tfield);
                    }
                }
            }
            return retDict;
        }

        public Collection<Dictionary<string, TField>> makeCollectionForRules(ref object[,] pNamArray, ref object[,] pValArray)
        {
            UInt64 aRows = (ulong)pValArray.GetLength(0);
            UInt64 aCols = (ulong)pValArray.GetLength(1);
            Collection<Dictionary<string, TField>> aRetCol = new Collection<Dictionary<string, TField>>();
            for (UInt64 i = 1; i <= aRows; i++)
            {
                Dictionary<string, TField> aRetDict = new Dictionary<string, TField>();
                for (UInt64 j = 1; j <= aCols; j++)
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(pNamArray[1, j],cc)))
                    {
                        if (mig.ContainsSource("P", Convert.ToString(pNamArray[1, j],cc)) | mig.ContainsSource("C", Convert.ToString(pNamArray[1, j],cc)))
                        {
                            TField tfield = new TField(Convert.ToString(pNamArray[1, j],cc), Convert.ToString(pValArray[i, j],cc));
                            aRetDict.Add(tfield.Name, tfield);
                        }
                    }
                }
                if (!isFiltered(ref aRetDict))
                    aRetCol.Add(aRetDict);
            }
            return aRetCol;
        }


        public Dictionary<string, TField> makeDict(ref object[,] pNamArray, ref object[,] pValArray, int pRow, int pFromCol, int pToCol)
        {
            Dictionary<string, TField> retDict = new Dictionary<string, TField>();
            TField tfield;
            for (var j = pFromCol; j <= pToCol; j++)
            {
                if (!String.IsNullOrEmpty(Convert.ToString(pNamArray[1, j],cc)))
                {
                    tfield = new TField(Convert.ToString(pNamArray[1, j],cc), Convert.ToString(pValArray[pRow, j],cc));
                    retDict.Add(tfield.Name, tfield);
                }
            }
            return retDict;
        }
    }
}
