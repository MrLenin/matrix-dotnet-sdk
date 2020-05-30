using System;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

namespace Matrix.Api.ClientServer.Events
{
    public class ReceiptEventContent : IEventContent
    {
        public Dictionary<string, ReceiptedEvent> ReceiptedEvents { get; set; }

        public void ParseJObject(JToken obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            ReceiptedEvents = new Dictionary<string, ReceiptedEvent>();

            try
            {
                foreach (JProperty prop in obj.Children())
                {
                    var receipts = new ReceiptedEvent();
                    receipts.ParseJObject((JObject) prop.Value);
                    ReceiptedEvents.Add(prop.Name, receipts);
                }
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public class ReceiptedEvent
    {
        public Dictionary<string, Receipt> ReceiptedUsers { get; private set; }

        public void ParseJObject(JObject obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            ReceiptedUsers = new Dictionary<string, Receipt>();

            try
            {
                foreach (JProperty prop in obj.GetValue("m.read", StringComparison.InvariantCulture).Children())
                {
                    var receipt = new Receipt
                    {
                        TimeStamp = ((JToken)prop.Value)["ts"].ToObject<long>()
                    };
                    ReceiptedUsers.Add(prop.Name, receipt);
                }
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public class Receipt
    {
        public long TimeStamp { get; set; }
    }
}