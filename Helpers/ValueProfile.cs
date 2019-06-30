using AutoMapper;
using soapApi.Models;
using soapApi.ViewModels;

namespace soapApi.Helpers
{
    public class ValueProfile : Profile
    {
        public ValueProfile()
        {
            this.CreateMap<Value,ValueViewModel>();
        }
    }
}