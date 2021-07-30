using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infra.CrossCutting.Identity.Models
{
    public class ApplicationRole: IdentityRole 
    {

        #region Variables

        private string _description;
        private string _systemCod;
        private string _codRoleRoot;

        #endregion

        #region Properties

        public string Description { get => _description; set => _description = value; }
        public string SystemCod { get => _systemCod; set => _systemCod = value; }
        public string CodRoleRoot { get => _codRoleRoot; set => _codRoleRoot = value; }

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #endregion

    }
}
