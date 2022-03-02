namespace BlazorGameDotNet6.Shared;

public static class Constants
{
    public const int StartingCoin = 1000;
    
    public const string AuthToken = "authToken";
    public const string API = "api";
    public const string PATH_SEPARATOR = "/";

    public struct AuthMessages
    {
        public const string WrongPassword = "Wrong password.";
        public const string UserNotFound = "User not found.";
        public const string UserExistsEmail = "User with that email already exists.";
        public const string UserExistsUsername = "User with that username already exists.";
        public const string SuccessfulRegistration = "Registration successful.";
    }

    #region API calls
    public struct ApiRoute
    {
        public const string Login = "login";
        public const string Register = "register";
        public const string History = "history";
        public const string GetCoins = "getcoins";
        public const string AddCoins = "addcoins";
        public const string Leaderboard = "leaderboard";
        public const string Revive = "revive";
    }
    public struct Controller
    {
        public const string Auth = "auth";
        public const string Battle = "battle";
        public const string Unit = "unit";
        public const string User = "user";
        public const string UserUnit = "userunit";
    }
    public struct ApiEndpointPath
    {
        public const string AuthController_Post_Login = API + PATH_SEPARATOR + Controller.Auth + PATH_SEPARATOR + ApiRoute.Login;
        public const string AuthController_Post_Register = API + PATH_SEPARATOR + Controller.Auth + PATH_SEPARATOR + ApiRoute.Register;

        public const string BattleController_Get_History = API + PATH_SEPARATOR + Controller.Battle + PATH_SEPARATOR + ApiRoute.History;
        public const string BattleController_Post = API + PATH_SEPARATOR + Controller.Battle;

        public const string UnitController_Get = API + PATH_SEPARATOR + Controller.Unit;

        public const string UserController_Get_GetCoins = API + PATH_SEPARATOR + Controller.User + PATH_SEPARATOR + ApiRoute.GetCoins;
        public const string UserController_Get_Leaderboard = API + PATH_SEPARATOR + Controller.User + PATH_SEPARATOR + ApiRoute.Leaderboard;
        public const string UserController_Post_AddCoins = API + PATH_SEPARATOR + Controller.User + PATH_SEPARATOR + ApiRoute.AddCoins;

        public const string UserUnitController_Get = API + PATH_SEPARATOR + Controller.UserUnit;
        public const string UserUnitController_Delete = API + PATH_SEPARATOR + Controller.UserUnit + "/{0}";
        public const string UserUnitController_Post = API + PATH_SEPARATOR + Controller.UserUnit;
        public const string UserUnitController_Post_Revive = API + PATH_SEPARATOR + Controller.UserUnit + PATH_SEPARATOR + ApiRoute.Revive;
    }
    #endregion

    #region Razor pages
    public struct BattleLog
    {
        public const string DefeatKeyword = "kills";
        public const string DefeatedListGroupClass = "list-group-item list-group-danger";
        public const string GeneralListGroupClass = "list-group-item";
    }

    public struct History
    {
        public const string WinningMsg = "You won!";
        public const string LosingMsg = "You lost! :(";
        public const string WinningStyle = "color: green; font-weight: 600;";
    }

    public struct Leaderboard
    {
        public const string WinningMsg = "You won the battle!";
        public const string LosingMsg = "You have been destroyed!";
        public const string NoBattleMsg = "The battle did not take place.";
    }
    #endregion
}

public enum UnitTypeEnum
{
    Knight = 1,
    Archer = 2,
    Mage = 3,
}
