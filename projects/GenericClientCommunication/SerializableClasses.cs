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
    public RoundResult(float res) { result = res;} // Constructora para los resultados

    public float result; // Resultado de la ronda
}

[Serializable]
public class GameData
{
    public RoundResult[] rounds; // Resultado de las rondas

    public int rivalID = 0; // ID del rival

    public float rivalRating = 0; // Rating del rival

    public float rivalRD = 0; // Desviación del rival

    public float myRating = 0; // Rating del usuario

    public float myRD = 0; // Desviación del usuario

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


