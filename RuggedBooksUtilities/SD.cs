using System;
using System.Collections.Generic;
using System.Text;

namespace RuggedBooksUtilities
{
    // Static details, where we store all constant strings in app.
    public static class SD
    {
        public const string Procedure_CoverType_Create = "usp_CreateCoverType";
        public const string Procedure_CoverType_Get = "usp_GetCoverType";
        public const string Procedure_CoverType_GetAll = "usp_GetCoverTypes";
        public const string Procedure_CoverType_Update = "usp_UpdateCoverType";
        public const string Procedure_CoverType_Delete = "usp_DeleteCoverType";

        public const string Role_User_Individual = "Individual User";
        public const string Role_User_Company = "Company User";
        public const string Role_Administrator = "Administrator";
        public const string Role_Employee = "Employee";

        public const string Shopping_Cart_Session = "Shopping_Cart_Session";
    }
}
