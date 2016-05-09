using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;

namespace Obscured.Azure.DynDNS.Service.Installer
{
    [RunInstaller(true)]
    public partial class ServiceInstaller : System.Configuration.Install.Installer
    {
        public ServiceInstaller()
        {
            InitializeComponent();
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);

            try
            {
                var parameterList = new Dictionary<string, string>
                {
                    {"ClientId", Context.Parameters["ClientId"]},
                    {"ClientSecret", Context.Parameters["ClientSecret"]},
                    {"SubscriptionId", Context.Parameters["SubscriptionId"]},
                    {"ResourceGroup", Context.Parameters["ResourceGroup"]},
                    {"ZoneName", Context.Parameters["ZoneName"]},
                    {"RecordName", Context.Parameters["RecordName"]},
                    {"RecordType", Context.Parameters["RecordType"]},
                    {"Providers", Context.Parameters["Providers"]}
                };

                // Get the path to the executable file that is being installed on the target computer
                var assemblypath = Context.Parameters["assemblypath"];
                var appConfigPath = assemblypath + ".config";

                // Write the path to the app.config file
                var doc = new XmlDocument();
                doc.Load(appConfigPath);

                XmlNode configuration = null;
                foreach (XmlNode node in doc.ChildNodes)
                    if (node.Name == "configuration")
                        configuration = node;

                if (configuration != null)
                {
                    XmlNode settingNode = null;
                    foreach (XmlNode node in configuration.ChildNodes)
                        if (node.Name == "appSettings")
                            settingNode = node;

                    if (settingNode != null)
                    {
                        foreach (var parameter in parameterList)
                        {
                            XmlNode numNode = null;
                            foreach (XmlNode node in settingNode.ChildNodes)
                            {
                                if (node.Attributes != null && node.Attributes["key"] == null) continue;
                                if (node.Attributes != null && node.Attributes["key"].Value == parameter.Key)
                                    numNode = node;
                            }

                            if (numNode?.Attributes == null) continue;
                            var att = numNode.Attributes["value"];
                            att.Value = parameter.Value;
                        }
                        doc.Save(appConfigPath);
                    }
                }
            }
            catch (FormatException e)
            {
                var s = e.Message;
            }
        }
    }
}
