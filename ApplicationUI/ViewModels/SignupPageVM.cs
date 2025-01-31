using ApplicationUI.Pages;
using BLL.Interfaces;
using BLL.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ApplicationUI.ViewModels
{
    public class SignupPageVM
    {
        private IUserService<BookDTO, UserDTO> _userService;
        public SignupPageVM(IUserService<BookDTO, UserDTO> userService)
        {
            _userService = userService;
        }
    }
}
