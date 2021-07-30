using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Infra.CrossCutting.Identity.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [JsonProperty("codigoEmpresa")]
        [Required(ErrorMessage = "O campo código da empresa deve ser informado")]        
        public int CodigoEmpresa { get; set; }

        [JsonProperty("usuario")]
        [Required(ErrorMessage = "O campo usuário deve ser informado")]
        public string Usuario { get; set; }

        [JsonProperty("senha")]
        [Required(ErrorMessage = "O campo senha deve ser informado")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [JsonProperty("sistema")]
        public string Sistema { get; set; }

        [JsonProperty("tipoAutenticacao")]
        public string TipoAutenticacao { get; set; }

        public LoginViewModel()
        {
            TipoAutenticacao = "username";
        }

    }
}
