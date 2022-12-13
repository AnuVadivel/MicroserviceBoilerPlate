using AutoMapper;

namespace Payment.Persistence.Mapper      

{
    public class BankProfile : Profile

    {
        public  BankProfile()
        {
            CreateMap<Domain.Core.Bank,DAO.Bank>();
        }
    }
}