using System;
using System.Collections.Generic;
using System.Text;

namespace LibreriaStorageTest.Library.Helper
{
    public class ResponseMessage
    {
        public string Message {get;}
        public ResponseMessage(string message)
        {
            this.Message = message;
        }
    }
}
