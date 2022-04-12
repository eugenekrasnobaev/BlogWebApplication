using BlogBLL.DTO;
using System;
using System.Collections.Generic;

namespace BlogBLL.Interfaces
{
    public interface IAccountService
    {
        bool DoesTheUserValid(string email, string password);
        bool DoesTheUserExist(string email);
        UserDto CreateUser(RegisterUserDto model);
        UserDto GetUserById(int userId);
        UserDto GetUserByEmail(string email);
        IEnumerable<UserDto> UsersList();
        void DeleteUser(int userId);
        void EditUserPassword(EditUserDto model, int userId);
        Array GetRoles();
        void EditUserRole(EditUserDto model, int userId);
    }
}
