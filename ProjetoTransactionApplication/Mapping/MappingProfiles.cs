

using AutoMapper;
using ProjetoTransactionApplication.Dtos;
using ProjetoTransactionDomain.Entities;

namespace ProjetoTransactionApplication.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Transaction, FundTransferRequestDto>().ReverseMap();
            CreateMap<Transaction, FundTransferResponseDto>().ReverseMap();
        }
    }
}
