/* Copyright 2016-2019 Hermann Mundprecht
 * This file is licensed under the terms of the license 'CC BY 4.0'. 
 * For a human readable version of the license, see https://creativecommons.org/licenses/by/4.0/
 */

using System;
using System.Configuration;

namespace SAPCommon
{
    public class SapConnectionsSection : ConfigurationSection
    {
        [ConfigurationProperty("connections", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(SapConnectionsCollection),
        AddItemName = "add",
        ClearItemsName = "clear",
        RemoveItemName = "remove")]

        public SapConnectionsCollection SapConnections
        {
            get
            {
                SapConnectionsCollection sapConnectionsCollection =
                    (SapConnectionsCollection)base["connections"];
                return sapConnectionsCollection;
            }

            set
            {
                SapConnectionsCollection sapConnectionsCollection = value;
            }

        }

        // Create a new instance of the SapConnectionsSection.
        // This constructor creates a configuration element 
        // using the SapConnectionConfigElement default values.
        // It assigns this element to the collection.
        public SapConnectionsSection()
        {
            SapConnectionConfigElement connection = new SapConnectionConfigElement();
            SapConnections.Add(connection);
        }
    }

    public class SapConnectionsCollection : ConfigurationElementCollection
    {

        public SapConnectionsCollection()
        {
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SapConnectionConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((SapConnectionConfigElement)element).Name;
        }

        public SapConnectionConfigElement this[int index]
        {
            get
            {
                return (SapConnectionConfigElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public SapConnectionConfigElement this[string Name]
        {
            get
            {
                return (SapConnectionConfigElement)BaseGet(Name);
            }
        }


        public int IndexOf(SapConnectionConfigElement sapConnection)
        {
            return BaseIndexOf(sapConnection);
        }

        public void Add(SapConnectionConfigElement sapConnection)
        {
            BaseAdd(sapConnection);
            // Your custom code goes here.
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
            // Your custom code goes here.
        }

        public void Remove(SapConnectionConfigElement sapConnection)
        {
            if (BaseIndexOf(sapConnection) >= 0)
            {
                BaseRemove(sapConnection.Name);
                // Your custom code goes here.
                Console.WriteLine("SapConnectionsCollection: {0}", "Removed collection element!");
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
            // Your custom code goes here.
        }

        public void Remove(string name)
        {
            BaseRemove(name);
            // Your custom code goes here.
        }

        public void Clear()
        {
            BaseClear();
            // Your custom code goes here.
            Console.WriteLine("SapConnectionsCollection: {0}", "Removed entire collection!");
        }

    }

    public class SapConnectionConfigElement : System.Configuration.ConfigurationElement
    {
        public SapConnectionConfigElement(String Name, string SystemID, string AppServerHost = "",  string SystemNumber = "",
            string MessageServerHost = "", string LogonGroup = "", string Trace = "", string Client = "", string Language = "", 
            string SncMode = "0", string SncPartnerName = "", string SncMyName = "", string SAPRouter = "")
        {
            this.Name = Name;
            this.SystemID = SystemID;
            this.AppServerHost = AppServerHost;
            this.SystemNumber = SystemNumber;
            this.MessageServerHost = MessageServerHost;
            this.LogonGroup = LogonGroup;
            this.Trace = Trace;
            this.Client = Client;
            this.Language = Language;
            this.SncMode = SncMode;
            this.SncPartnerName = SncPartnerName;
            this.SncMyName = SncMyName;
            this.SAPRouter = SAPRouter;
        }

        public SapConnectionConfigElement()
        {
        }

        [ConfigurationProperty("Name", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("SystemID", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string SystemID
        {
            get { return (string)this["SystemID"]; }
            set { this["SystemID"] = value; }
        }

        [ConfigurationProperty("AppServerHost", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string AppServerHost
        {
            get { return (string)this["AppServerHost"]; }
            set { this["AppServerHost"] = value; }
        }

        [ConfigurationProperty("SystemNumber", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string SystemNumber
        {
            get { return (string)this["SystemNumber"]; }
            set { this["SystemNumber"] = value; }
        }

        [ConfigurationProperty("MessageServerHost", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string MessageServerHost
        {
            get { return (string)this["MessageServerHost"]; }
            set { this["MessageServerHost"] = value; }
        }

        [ConfigurationProperty("LogonGroup", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string LogonGroup
        {
            get { return (string)this["LogonGroup"]; }
            set { this["LogonGroup"] = value; }
        }

        [ConfigurationProperty("Trace", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string Trace
        {
            get { return (string)this["Trace"]; }
            set { this["Trace"] = value; }
        }

        [ConfigurationProperty("Client", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string Client
        {
            get { return (string)this["Client"]; }
            set { this["Client"] = value; }
        }

        [ConfigurationProperty("Language", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string Language
        {
            get { return (string)this["Language"]; }
            set { this["Language"] = value; }
        }

        [ConfigurationProperty("SncMode", DefaultValue = "0", IsRequired = true, IsKey = false)]
        public string SncMode
        {
            get { return (string)this["SncMode"]; }
            set { this["SncMode"] = value; }
        }

        [ConfigurationProperty("SncPartnerName", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string SncPartnerName
        {
            get { return (string)this["SncPartnerName"]; }
            set { this["SncPartnerName"] = value; }
        }

        [ConfigurationProperty("SncMyName", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string SncMyName
        {
            get { return (string)this["SncMyName"]; }
            set { this["SncMyName"] = value; }
        }

        [ConfigurationProperty("SAPRouter", DefaultValue = "", IsRequired = false, IsKey = false)]
        public string SAPRouter
        {
            get { return (string)this["SAPRouter"]; }
            set { this["SAPRouter"] = value; }
        }
    }
}
