namespace RemoteWorkAssistant.Server.Constants
{
    public enum Messages
    {
        // HTTP Status: 401
        [StringValue("Authentication error.")]
        AUTHENTICATION_ERROR,

        // HTTP Status: 404
        [StringValue("Not found e-mail address.")]
        EMAIL_NOT_FOUND,

        // HTTP Status: 404
        [StringValue("Not found Pc name.")]
        PC_NAME_NOT_FOUND,

        // HTTP Status: 409
        [StringValue("E-mail address is conflicted.")]
        EMAIL_CONFLICT,

        // HTTP Status: 409
        [StringValue("Pc name is conflicted.")]
        PC_NAME_CONFLICT
    }
}
