﻿namespace MomoProducts.Server.Models.Collections
{
    using MomoProducts.Server.Models.Common;

    public class PreApproval
    {
        public string ReferenceId { get; set; }

        public Payer Payer { get; set; }

        public string PayerCurrency { get; set; }

        public string PayerMessage { get; set; }

        public int ValidityTime { get; set; }
    }

}
