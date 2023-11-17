using Kel25_Mod9_TugasAPI.Data;
using Kel25_Mod9_TugasAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Kel25_Mod9_TugasAPI.Controllers

{

    [Route("api/UserAPI")]
    [ApiController]

    public class UserAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<UserDTO>> GetVillas()
        {
            return Ok(UserStore.userList);
        }

        [HttpGet("{Id:int}", Name = "GetUser")]
        [ProducesResponseType(200, Type = typeof(UserDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]

        public ActionResult<UserDTO> GetUser(int id)
        {
            if (id == 0) return BadRequest();
            var acc = UserStore.userList.FirstOrDefault(u => u.Id == id);
            if (acc == null) return NotFound();
            return Ok(acc);
        }

        [HttpPost]
        public ActionResult<UserDTO> CreateAcc([FromBody] UserDTO userDTO)
        {
            if (UserStore.userList.FirstOrDefault(u => u.Username.ToLower() == userDTO.Username.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Account already exists");
                return BadRequest(ModelState);
            }

            if (userDTO == null)
            {
                return BadRequest(userDTO);
            }

            //if (userDTO.Id != 0)
            //{
               // return StatusCode(StatusCodes.Status500InternalServerError);
            //}
            userDTO.Id = UserStore.userList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            UserStore.userList.Add(userDTO);
            string response = "Sukses menambahkan data Akun" + "\nNama : " + userDTO.Username;
            return CreatedAtRoute("GetUser", new { id = userDTO.Id }, response);
        }

        [HttpPost("/login")]
        public ActionResult<UserDTO> LoginAcc([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest("Username/Password Invalid");
            }

            var user = UserStore.userList.FirstOrDefault(u => u.Username == userDTO.Username);
            if (user == null)
            {
                return NotFound("Username tidak ditemukan");
            }

            if (user.Password != userDTO.Password)
            {
                return Unauthorized("Password Salah");
            }
            var data = new
            {
                Id = user.Id,
                Username = user.Username
            };
            
            return Ok(JsonSerializer.Serialize(data));
        }

        [HttpDelete("{id:int}", Name = "DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var acc = UserStore.userList.FirstOrDefault(u => u.Id == id);
            if (acc == null)
            {
                return NotFound();
            }
            UserStore.userList.Remove(acc);
            return Ok("Berhasil Dihapus");
        }

        [HttpPut("{id:int}", Name = "UpdateUser")]
        public IActionResult UpdateUser(int id, [FromBody] UserDTO userDTO)
        {
            if (userDTO == null || id != userDTO.Id)
            {
                return BadRequest("Gagal diedit");
            }
            var acc = UserStore.userList.FirstOrDefault(u => u.Id == id);
            acc.Username = userDTO.Username;
            acc.Password = userDTO.Password;
            return Ok("Berhasil diedit");
        }
    }
}
