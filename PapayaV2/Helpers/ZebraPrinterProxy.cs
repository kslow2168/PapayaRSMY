using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CookComputing.XmlRpc;

namespace PapayaX2.Helpers
{
    public class ZebraPrinterProxy : XmlRpcClientProtocol
    {
        // [XmlRpcMethod("Printer.print")]
        // Boolean print(String serverIp, int serverPort, String templateName, XmlRpcStruct param);
        // for the param please use string as key and object
        // XmlRpcStruct param = new XmlRpcStruct();
        // param["darkness"] = numDarkness.Value.ToString();

        // public boolean print(String serverIp, int serverPort, String code, Map<String, String> params)

        [XmlRpcMethod("Printer.print")]
        public bool Print(String serverIp, int serverPort, String code, XmlRpcStruct param)
        {
            return (bool)Invoke("Print", new Object[] { serverIp, serverPort, code, param });
        }
    }
}