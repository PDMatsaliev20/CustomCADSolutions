namespace CustomCADs.API
{
    public static class ApiMessages
    {
        public const string IsRequired = "{0} is required.";
        public const string NotFound = "{0} not found.";
        public const string ForbiddenAccess = "Not allowed to modify another User's resources.";
        public const string ForbiddenPatch = "Not allowed to patch field {0}.";
        public const string ForbiddenRoleRegister = "You must apply to either be a Client or a Contributor.";
        public const string InvalidLogin = "Invalid Username or Password.";
        public const string UnauthenticatedUser = "User not authenticated.";
        public const string UserHasNoRole = "User has no role.";
        public const string UserHasRoles = "User has more than one role.";
        public const string OrderHasNoCad = "Order has no associated Cad.";
    }
}
