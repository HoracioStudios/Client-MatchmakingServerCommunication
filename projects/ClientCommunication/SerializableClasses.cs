using System;

[Serializable]
public class LoginInfo
{
    public string nick; // Nombre del usuario
    public string email; // Email del usuario
    public string password; // Contraseña del usuario
    public string version; // Version del sistema
}

[Serializable]
public class RoundResult
{
    public RoundResult(float res, float t) { result = res; time = t; } // Constructora para los resultados y tiempos

    public float result; // Resultado de la ronda
    public float time; // Tiempo de cada tonda
}

[Serializable]
public class GameData
{
    public RoundResult[] rounds; // Resultado de las rondas

    public string matchID; // ID de la partida

    public int rivalID = 0; // ID del rival

    public string rivalNick = ""; // Nick del rival

    public float rivalRating = 0; // Rating del rival

    public float rivalRD = 0; // Desviación del rival

    public float myRating = 0; // Rating del usuario

    public float myRD = 0; // Desciación del usuario

    public string playerChar; // Nombre del personaje jugador por el usuario

    public string rivalChar; // Nombre del personaje jugador por el rival

    public int shotsFired = 0; // Numero de disparos realizados

    public float dmgDealt = 0; // Numero de daño hecho

    public float accuracy = 0; // Precisión 

}

[Serializable]
public class Identifiers
{
    public int ID1; // ID del usuario
    public int ID2; // ID del rival
}

[Serializable]
public class RefreshData
{
    public string refreshToken; // Refreash token
}

[Serializable]
public class RefreshMessage : ServerMessage
{
    public string accessToken; // Access token
}


//Informacion sobre el puerto y el matchID que da el controlador
//de servidores de juego
[Serializable]
public class ServerMatchInfo : ServerMessage
{
    public string port; // Número de puerto del servidor de juego
    public string matchID; // ID de la partida
}

[Serializable]
public class REST_Error : ServerMessage
{
    public string message; // Mensaje de error
}

[Serializable]
public class Login : ServerMessage
{
    public int id; // ID del usuario
    public string accessToken; // Access token del usuario
    public string refreshToken; // Refresh token del usuario
}

[Serializable]
public class Available : ServerMessage
{
    public bool emailAvailable = false; // Disponibilidad del email
    public bool nickAvailable = false; // Disponibilidad del nick
}

[Serializable]
public class PairSearch : ServerMessage
{
    public bool found = false; // Marca si se ha encontrado un jugador
    public bool finished = false; // Marca si se han emparejado mutuamente
    public int rivalID = -1; // ID del rival
    public string rivalNick = ""; // Nick del rival
    public float bestRivalRating = 1500; // Rating del rival
    public float bestRivalRD = 0; // Desviación del rival
    public float myRating = 1500; // Rating del usuario
    public float myRD = 0; // Desviación del usuario
}

[Serializable]
public class GameEndMessage : ServerMessage
{
    public GameData results; // Resultados de la partida

}

[Serializable]
public class UserDataSmall : ServerMessage
{
    public int id; // ID del usuario
    public string nick; // Nick del usuario
    public string email; // Email del usuario 
    public float rating; // Rating del usuario
    public float RD; // Desviación del usuario
    public string creation; // Fecha de creación de la cuenta
    public int wins; // Cantidad de partidas ganadas
    public int draws; // Cantidad de partidas empatadas
    public int losses; // Cantidad de partidas perdidas
    public int totalGames; // Número total de partidas
}

[Serializable]
public class ServerMessage
{
    public int code = 0; // Código devuelto
}


