
using AutoMapper;
using ToDoService.DAL.Entities;

namespace ToDoService.NewFolder1
{
    public class AutoMapperProfiles  : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ToDo, ToDoResponseDTO>();
            CreateMap<ToDoResponseDTO, ToDo>();
            CreateMap<ToDo, UpdateTaskDTO>();
            CreateMap<UpdateTaskDTO, ToDo>();
        }
    }
}
