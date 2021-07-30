using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infra.CrossCutting.Identity.Models.AccountViewModels
{
    public class RefreshLoginViewModel
    {

        [Required(ErrorMessage = "O campo usuário deve ser informado")]
        public string usuario { get; set; }

        [Required(ErrorMessage = "O token de renovação deve ser informado")]
        public string refresh_token;

    }
}
