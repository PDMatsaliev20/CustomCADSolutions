namespace CustomCADs.API
{
    public static class ApiMessages
    {
        public const string IsRequired = "{0} is required.";
        public const string NotFound = "{0} not found.";
        public const string NoRefreshToken = "No Refresh Token provided.";
        public const string NoNeedForNewRT = "Refresh token still valid, no need to refresh.";
        public const string AccessTokenRenewed = "Access token renewed";
        public const string ForbiddenAccess = "Not allowed to modify another User's resources.";
        public const string ForbiddenPatch = "Not allowed to patch field {0}.";
        public const string ForbiddenRoleRegister = "You must apply to either be a Client or a Contributor.";
        public const string InvalidRole = "Invalid role - you must choose from [{0}].";
        public const string InvalidStatus = "Invalid status - you must choose from [{0}].";
        public const string InvalidLogin = "Invalid Username or Password.";
        public const string InvalidSize = "Size must not be 0";
        public const string InvalidZip = "Zip is not valid";
        public const string UnauthenticatedUser = "User not authenticated.";
        public const string LockedOutUser = "The max attempts for logging in has been reached. The account has been locked out for {0} seconds.";
        public const string UserHasNoRole = "User has no role.";
        public const string UserHasRoles = "User has more than one role.";
        public const string OrderHasNoCad = "Order has no associated Cad.";
        public const string ModifiedOrderNotPending = "You cannot modify an Order that isn't Pending";
        public const string CadPurchasedMessage = "3D Model from the Gallery with id: {0}";
        public const string SuccessfulPayment = "Payment was successful.";
        public const string CanceledPayment = "Payment was canceled. Please try again or use a different payment method.";
        public const string ProcessingPayment = "Payment is processing. Please wait for confirmation.";
        public const string SuccessfulPaymentCapture = "Payment captured successfully.";
        public const string FailedPaymentCapture = "Failed to capture payment.";
        public const string FailedPaymentMethod = "Payment failed. Please try another payment method.";
        public const string FailedPayment = "Payment requires further action.";
        public const string UnhandledPayment = "Unhandled payment status: {0}";
    }
}
