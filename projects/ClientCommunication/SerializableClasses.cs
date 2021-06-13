using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class LoginInfo
{
    public string nick;
    public string email;
    public string password;
    public string version;
}

[Serializable]
public class RoundResult
{
    public RoundResult(float res) { result = res;}

    public float result;
}

[Serializable]
public class GameData
{
    public RoundResult[] rounds;

    public int rivalID = 0;

    public float rivalRating = 0;

    public float rivalRD = 0;

    public float myRating = 0;

    public float myRD = 0;

}


[Serializable]
public class RefreshData
{
    public string refreshToken;
}

[Serializable]
public class RefreshMessage : ServerMessage
{
    public string accessToken;
}

[Serializable]
public class REST_Error : ServerMessage
{
    public string message;
}

[Serializable]
public class Login : ServerMessage
{
    public int id;
    public string accessToken;
    public string refreshToken;
}

[Serializable]
public class Available : ServerMessage
{
    public bool emailAvailable = false;
    public bool nickAvailable = false;
}

[Serializable]
public class PairSearch : ServerMessage
{
    public bool found = false;
    public bool finished = false;
    public int rivalID = -1;
    public float bestRivalRating = 1500;
    public float bestRivalRD = 0;
    public float myRating = 1500;
    public float myRD = 0;
}

[Serializable]
public class GameEndMessage : ServerMessage
{
    public GameData results;

}

[Serializable]
public class UserDataSmall : ServerMessage
{
    public int id;
    public string nick;
    public string email;
    public float rating;
    public float RD;
    public string creation;
    public int wins;
    public int draws;
    public int losses;
    public int totalGames;
}

[Serializable]
public class ServerMessage
{
    public int code = 0;
}


