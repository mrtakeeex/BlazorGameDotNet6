namespace BlazorGameDotNet6.Shared;

public static class Constants
{
    public const int StartingCoin = 1000;
    public const string AuthToken = "authToken";

    public struct ApiEndpointPath
    {
        public const string AuthController_Post_Login = "api/auth/login";
        public const string AuthController_Post_Register = "api/auth/register";

        public const string BattleController_Get_History = "api/battle/history";
        public const string BattleController_Post = "api/battle";

        public const string UnitController_Get = "api/unit";

        public const string UserController_Get_GetCoins = "api/user/getcoins";
        public const string UserController_Get_Leaderboard = "api/user/leaderboard";
        public const string UserController_Post_AddCoins = "api/user/addcoins";

        public const string UserUnitController_Get = "api/userunit";
        public const string UserUnitController_Delete = "api/userunit/{0}";
        public const string UserUnitController_Post = "api/userunit";
        public const string UserUnitController_Post_Revive = "api/userunit/revive";
    }

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
}

public enum UnitTypeEnum
{
    Knight = 1,
    Archer = 2,
    Mage = 3,
}
