namespace ChicksGold.Test.Domain.AutoMapper;

using ChicksGold.Test.Domain.Contracts;
using global::AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Step, StepResultDto>();
        CreateMap<Solution, SolutionResultDto>();
    }
}
