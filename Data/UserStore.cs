using Kel25_Mod9_TugasAPI.Models.Dto;

namespace Kel25_Mod9_TugasAPI.Data
{
    public static class UserStore
    {
        public static List<UserDTO> userList = new List<UserDTO>
        {
             new UserDTO{Id=1, Username="farhan", Password="iloveqiqih"},
             new UserDTO{Id=2, Username="ivan", Password="duyureditulus"}
        };
    }
}
