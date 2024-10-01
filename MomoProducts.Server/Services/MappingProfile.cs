using AutoMapper;
using MomoProducts.Server.Models;
using MomoProducts.Server.Dtos.AuthDataDto;
using MomoProducts.Server.Dtos.CollectionsDto;
using MomoProducts.Server.Dtos.CommonDto;
using MomoProducts.Server.Dtos.DisbursementsDto;
using MomoProducts.Server.Dtos.RemittanceDto;
using MomoProducts.Server.Dtos.AuthDataDto;
using MomoProducts.Server.Dtos.CollectionsDto;
using MomoProducts.Server.Dtos.CommonDto;
using MomoProducts.Server.Dtos.DisbursementsDto;
using MomoProducts.Server.Dtos.RemittanceDto;
using MomoProducts.Server.Models.AuthData;
using MomoProducts.Server.Models.Collections;
using MomoProducts.Server.Models.Common;
using MomoProducts.Server.Models.Disbursements;
using MomoProducts.Server.Models.Remittance;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MomoProducts.Server.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // AuthData Mappings
            CreateMap<ApiKey, ApiKeyDto>().ReverseMap();
            CreateMap<ApiUser, ApiUserDto>().ReverseMap();

            // Collections Mappings
            CreateMap<Invoice, InvoiceDto>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<PreApproval, PreApprovalDto>().ReverseMap();
            CreateMap<RequesttoPay, RequestToPayDto>().ReverseMap();
            CreateMap<RequestToWithdraw, RequestToWithdrawDto>().ReverseMap();

            // Common Mappings
            CreateMap<AccessToken, AccessTokenDto>().ReverseMap();
            CreateMap<Money, MoneyDto>().ReverseMap();
            CreateMap<Oauth2Token, Oauth2TokenDto>().ReverseMap();
            CreateMap<Payee, PayeeDto>().ReverseMap();
            CreateMap<Payer, PayerDto>().ReverseMap();
            CreateMap<Transfer, TransferDto>().ReverseMap();

            // Disbursements Mappings
            CreateMap<Deposit, DepositDto>().ReverseMap();
            CreateMap<Refund, RefundDto>().ReverseMap();

            // Remittance Mappings
            CreateMap<CashTransfer, CashTransferDto>().ReverseMap();
        }
    }
}
