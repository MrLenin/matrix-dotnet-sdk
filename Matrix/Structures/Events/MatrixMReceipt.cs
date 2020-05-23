using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Matrix.Structures
{
    /// <summary>
    /// http://matrix.org/docs/spec/r0.0.1/client_server.html#m-receipt
    ///
    /// </summary>
    public class MatrixMReceipt : MatrixEventContent
    {
        public Dictionary<string, MatrixReceipts> Receipts { get; set; }

        public void ParseJObject(JObject obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            Receipts = new Dictionary<string, MatrixReceipts>();

            try
            {
                foreach (JProperty prop in obj.Children())
                {
                    var receipts = new MatrixReceipts();
                    receipts.ParseJObject((JObject) prop.Value);
                    Receipts.Add(prop.Name, receipts);
                }
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public class MatrixReceipts
    {
        public Dictionary<string, MatrixReceipt> ReadReceipts { get; private set; }

        public void ParseJObject(JObject obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            ReadReceipts = new Dictionary<string, MatrixReceipt>();

            try
            {
                foreach (JProperty prop in obj.GetValue("m.read", StringComparison.InvariantCulture).Children())
                {
                    var receipt = new MatrixReceipt
                    {
                        TimeStamp = ((JObject)prop.Value)["ts"].ToObject<long>()
                    };
                    ReadReceipts.Add(prop.Name, receipt);
                }
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public class MatrixReceipt
    {
        public long TimeStamp { get; set; }
    }
}