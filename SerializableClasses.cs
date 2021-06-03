﻿using System;
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
}

[Serializable]
public class RoundResult
{
    public RoundResult(float res, float t) { result = res; time = t; }

    public float result;
    public float time;
}

[Serializable]
public class GameData
{
    public RoundResult[] rounds;

    public string matchID;

    public int rivalID = 0;

    public string rivalNick = "";

    public string playerChar;

    public string rivalChar;

    public int shotsFired = 0;

    public float dmgDealt = 0;

    public float accuracy = 0;

}

[Serializable]
public class Identifiers
{
    public int ID1;
    public int ID2;
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
//Informacion sobre el puerto y el matchID que da el controlador
//de servidores de juego
public class ServerMatchInfo : ServerMessage
{
    public string port;
    public string matchID;
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
    public string rivalNick = "";
}

[Serializable]
public class GameEndMessage : ServerMessage
{
    public GameData results;

}

[Serializable]
public class ServerMessage
{
    public int code = 0;
}

