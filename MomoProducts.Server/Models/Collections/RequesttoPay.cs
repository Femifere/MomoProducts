﻿using MomoProducts.Server.Models.Common;

namespace MomoProducts.Server.Models.Collections
{
    public class RequesttoPay
    {
        

        public string Amount { get; set; }

        public string Currency { get; set; }

        public string ExternalId { get; set; }

        public Payer Payer { get; set; }

        public string PayerMessage { get; set; }

        public string PayeeNote { get; set; }
    }

}
