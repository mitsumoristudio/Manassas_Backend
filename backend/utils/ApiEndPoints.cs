namespace Project_Manassas;

public class ApiEndPoints
{
    private const string ApiBase = "api";

    public static class Projects
    {
        private const string Base = $"{ApiBase}/projects";

        public const string CREATE_URL_PROJECTS_CONSTANT = Base;
        
        public const string GET_URL_PROJECTS = $"{{id:guid}}";

        public const string GETALL_URL_PROJECTS_CONSTANT = Base;
        
        public const string UPDATE_URL_PROJECTS = $"{{id:guid}}";
        
        public const string GET_URL_MYPROJECT = $"{user}/{{userId:guid}}";
        
        public const string DELETE_URL_PROJECTS = $"{{id:guid}}";

        private const string user = "user";
    }

    public static class Users
    {
        public const string Base = $"{ApiBase}/users";
        
        public const string REGISTER_URL_USER_CONSTANT = $"register";
        
        public const string LOGIN_URL_USER_CONSTANT = $"login";
        
        public const string GET_URL_USER_CONSTANT = $"{{id:guid}}";
        
        public const string DELETE_URL_USER_CONSTANT = $"{{id:guid}}";
        
        public const string UPDATE_URL_USER_CONSTANT = $"{{id:guid}}";

       
        
        public const string LOGOUT_URL_USER_CONSTANT = $"logout";
    }
    
    public static class Equipments
    {
        private const string Base = $"{ApiBase}/equipments";
        
        public const string CREATE_URL_EQUIPMENT_CONSTANT = Base;
        
        public const string GET_URL_EQUIPMENT = $"{{id:guid}}";
        
        public const string DELETE_URL_EQUIPMENT = $"{{id:guid}}";
        
        public const string UPDATE_URL_EQUIPMENT = $"{{id:guid}}";
        
      //  public const string GETALL_URL_EQUIPMENT = Base;
      
      public const string GET_URL_MYEQUIPMENT = $"{user}/{{userId:guid}}";

      private const string user = "user";
    }

    public static class SendGridEmail
    {
        public const string Base = $"{ApiBase}/users";

        public const string REGISTER_URL_SENDGRID = $"registerSendGrid";
        
        public const string VERIFY_URL_SENDGRID = $"verifyEmail";
        
        public const string FORGOT_URL_PASSWORD = $"forgotPassword";
        
        public const string RESET_URL_PASSWORD = $"resetPassword";
    }
}