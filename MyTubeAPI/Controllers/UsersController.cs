using MyTube.DTO;
using MyTube.Repository;
using MyTubeAPI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestProject.Models;

namespace MyTubeAPI.Controllers
{
    public class UsersController : ApiController
    {

        private UsersRepository usersRepo;
        private readonly string BASIC_PROFILE_PICTURE = "https://avpn.asia/wp-content/uploads/2015/05/empty_profile.png";

        public UsersController()
        {
            this.usersRepo = new UsersRepository(new MyDBContext());
        }

        [Route("api/users/index")]
        [HttpGet]
        public HttpResponseMessage Top6MostSubbedUsers()
        {
            var users = usersRepo.GetNTopSubbedUsers(6);
            var usersDTO = UserDTO.ConvertCollectionUserToDTO(users);
            return Request.CreateResponse(HttpStatusCode.OK, usersDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/users/all")]
        [HttpGet]
        public HttpResponseMessage AllUsers()
        {
            var users = usersRepo.GetAllUsers();
            var usersDTO = UserDTO.ConvertCollectionUserToDTO(users);
            return Request.CreateResponse(HttpStatusCode.OK, usersDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/users/search_sort/{parameter}/{sortOrder?}")]
        [HttpGet]
        public HttpResponseMessage SearchAndSort(string parameter, string sortOrder = "")
        {
            if (parameter == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var users = usersRepo.SearchAndSortUsers(parameter, sortOrder);
            var usersDTO = UserDTO.ConvertCollectionUserToDTO(users);
            return Request.CreateResponse(HttpStatusCode.OK, usersDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/users/{username}")]
        [HttpGet]
        public HttpResponseMessage GetUserByUsername(string username)
        {
            var user = usersRepo.GetUserByUsername(username);
            var userDTO = UserDTO.ConvertUserToDTO(user);
            return Request.CreateResponse(HttpStatusCode.OK, userDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/users/users_subbed_to/{channelSubscribed}")]
        [HttpGet]
        public HttpResponseMessage GetUsersSubscribedTo(string channelSubscribed)
        {
            if (channelSubscribed == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var users = usersRepo.GetUsersSubscribedTo(channelSubscribed);
            var usersDTO = UserDTO.ConvertCollectionUserToDTO(users);
            return Request.CreateResponse(HttpStatusCode.OK, usersDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/users/subscriptions_by/{subscriber}")]
        [HttpGet]
        public HttpResponseMessage SubscriptionsBy(string subscriber)
        {
            if (subscriber == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var users = usersRepo.GetUsersSubscribedBy(subscriber);
            var usersDTO = UserDTO.ConvertCollectionUserToDTO(users);
            return Request.CreateResponse(HttpStatusCode.OK, usersDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/users/login")]
        [HttpPost]
        public HttpResponseMessage AllUsers(User user)
        {
            if (user.Username == null || user.Pass == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var status = usersRepo.Login(user);
            return Request.CreateResponse(HttpStatusCode.OK, status, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/users/create")]
        [HttpPost]
        public HttpResponseMessage CreateUser(User user)
        {
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            if (usersRepo.UsernameTaken(user.Username))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            user.RegistrationDate = DateTime.Now;
            if (user.ProfilePictureUrl == null)
            {
                user.ProfilePictureUrl = BASIC_PROFILE_PICTURE;
            }
            usersRepo.InsertUser(user);
            var userDTO = UserDTO.ConvertUserToDTO(user);
            return Request.CreateResponse(HttpStatusCode.Created, userDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/users/update/{username}")]
        [HttpGet]
        public HttpResponseMessage GetUserForUpdate(string username)
        {
            var userForEdit = usersRepo.GetUserByUsername(username);
            if (userForEdit == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            var userEUM = EditUserModel.EditUserModalFromUser(userForEdit);

            return Request.CreateResponse(HttpStatusCode.OK, userEUM, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/users/update/{username}")]
        [HttpPut]
        public HttpResponseMessage UpdateUser(EditUserModel user, string username)
        {
            if (user == null || username == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var userForEdit = usersRepo.GetUserByUsername(username);
            if (userForEdit == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            userForEdit.UpdateUserFromEditUserModel(user);
            usersRepo.UpdateUser(userForEdit);

            var userDTO = UserDTO.ConvertUserToDTO(userForEdit);

            return Request.CreateResponse(HttpStatusCode.OK, userDTO, Configuration.Formatters.JsonFormatter);
        }

        [Route("api/users/unblock/{username}")]
        [HttpPut]
        public HttpResponseMessage UnblockUser(string username)
        {
            if (username == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var userForEdit = usersRepo.GetUserByUsername(username);
            if (userForEdit == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            usersRepo.UnblockUser(username);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/users/block/{username}")]
        [HttpPut]
        public HttpResponseMessage BlockUser(string username)
        {
            if (username == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var userForEdit = usersRepo.GetUserByUsername(username);
            if (userForEdit == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            usersRepo.BlockUser(username);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/users/delete/{username}")]
        [HttpDelete]
        public HttpResponseMessage DeleteUser(string username)
        {
            if (username == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var userForDel = usersRepo.GetUserByUsername(username);
            if (userForDel == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            usersRepo.DeleteUser(username);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
