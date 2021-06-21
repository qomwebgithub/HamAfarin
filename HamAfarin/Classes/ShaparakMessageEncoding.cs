using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HamAfarin
{
    public class ShaparakMessageEncoding
    {
        //  {"Token":"AhVuYaudIyef/TBPTOXNve+6SIQ0gn9CKVl8r7lg4IY=","IsSuccess":true,"Message":"عمليات با موفقيت انجام شد"} 

        public ShaparakMessageEncoding()
        {

        }
        public bool Encode(string strShaparakResault, out string Token, out string Message)
        {
            Token = "";
            Message = "";
            strShaparakResault = strShaparakResault.Replace("\"", "");
            strShaparakResault = strShaparakResault.Replace("{", "");
            strShaparakResault = strShaparakResault.Replace("}", "");
            strShaparakResault = strShaparakResault.Replace("\\", "");
            string strFullToken = strShaparakResault.Split(',')[0];
            string strFullSuccess = strShaparakResault.Split(',')[1];
            string strFullMessage = strShaparakResault.Split(',')[2];
            if (strFullSuccess.Split(':')[1] == "false")
            {
                Message = strFullMessage.Split(':')[1];
                Message = Message.Replace("\"", "\"");
                return false;
            }
            else
            {
                Message = strFullMessage.Split(':')[1];
                Message = Message.Replace("\"", "");
                Token = strFullToken.Split(':')[1];
                Token = Token.Replace("\"", "\"");
                return true;
            }

        }
    }
}