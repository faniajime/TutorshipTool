﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication156456.Models
{
    public static class Roles
    {
        public const string Tutor = "Tutor";
        public const string Admin = "Administrador";
        public const string Estudiante = "Estudiante";

        public static bool RoleExists(string rol)
        {
            if(Equals(rol, Roles.Tutor)|| Equals(rol, Roles.Admin) || Equals(rol, Roles.Estudiante))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}