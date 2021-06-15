using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

public class ClientCommunication
{
    static string IP = "localhost"; // IP donde esta alojado el servidor de matchmaking
    static string PORT = "25565";
    static string URL = "http://" + IP + ":" + PORT;
    static string URL_GAME = "http://" + IP + ":" + PORT_GAME_SERVER;

    const string PORT_GAME_SERVER = "25564";

    static string authToken = "";
    static string refreshToken = "";


    /// <summary>
    /// Inicializa la ip y el puerto con el que se va a conectar el cliente
    /// </summary>
    /// <param name="IP_"> IP de conexión </param>
    /// <param name="PORT_"> puerto de conexión </param>
    public static void Init(string IP_, string PORT_)
    {
        IP = IP_;
        PORT = PORT_;
        URL = "http://" + IP + ":" + PORT;
        URL_GAME = "http://" + IP + ":" + PORT_GAME_SERVER;
    }

    /// <summary>
    /// Inicia la sesión de un usuario en el servidor de matchmaking
    /// </summary>
    /// <param name="password"> contraseña (a poder ser encriptada) del usuario con el que se registró </param>
    /// <param name="username"> nombre del usuario con el que se registró </param>
    /// <param name="version"> versión en la que se encuentra el sistema. 1.0.0 por defecto </param>
    /// <returns> Devuelve un ServerMessage con posibilidad de casteo a Login. Casteo a REST_Error si el 
    /// código devuelto es distinto de 200 </returns>
    public static ServerMessage LogIn(string password, string username, string version)
    {
        string url = URL + "/accounts/sessions";

        LoginInfo user = new LoginInfo();
        user.nick = username;
        user.password = password;
        user.version = version;
        string json = JsonConvert.SerializeObject(user);

        int code;

        try
        {
            var reply = Post(json, url, out code);
            //var reply = Post(json, url, out code, out message);


            if (code != 200)
            {
                REST_Error message = new REST_Error();

                if (code < 0)
                    message.message = "Error de socket, no se puede abrir una conexión";
                else
                {
                    try
                    {
                        message = JsonConvert.DeserializeObject<REST_Error>(reply);
                    }
                    catch (Exception e)
                    {
                        message.message = reply;
                    }
                }

                message.code = code;

                return message;
            }
            else
            {
                Login message = new Login();

                message = JsonConvert.DeserializeObject<Login>(reply);

                authToken = message.accessToken;
                refreshToken = message.refreshToken;

                message.code = code;

                return message;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// Desconecta al usuario del servidor de matchmaking
    /// </summary>
    /// <returns> Devuelve un ServerMessage con casteo posible a REST_Error si el código es distinto a 200 </returns>
    public static ServerMessage LogOut()
    {
        string url = URL + "/accounts/sessions";

        RefreshData user = new RefreshData();
        user.refreshToken = refreshToken;
        string json = JsonConvert.SerializeObject(user);

        int code;

        try
        {
            var reply = Delete(json, url, out code, true);
            //var reply = Post(json, url, out code, out message);
            REST_Error message = new REST_Error();

            if (code < 0)
                message.message = "Error de socket, no se puede abrir una conexión";
            else
            {
                try
                {
                    message = JsonConvert.DeserializeObject<REST_Error>(reply);
                }
                catch (Exception e)
                {
                    message.message = reply;
                }
            }

            message.code = code;

            return message;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// Registra a un usuario en el servidor de matchmaking
    /// </summary>
    /// <param name="password"> Contraseña del usuario (a ser posible encriptada)</param>
    /// <param name="username"> Nick con el que se registra el usuario. Solo un usuario por nick</param>
    /// <param name="email"> Email de registro del usuario. Solo un usuario por email</param>
    /// <returns>Devuelve un ServerMessage con casteo posible a REST_Error si el código es distinto a 200 </returns>
    public static ServerMessage SignIn(string password, string username, string email)
    {
        string url = URL + "/accounts";

        LoginInfo user = new LoginInfo();
        user.nick = username;
        user.password = password;
        user.email = email;
        string json = JsonConvert.SerializeObject(user);

        int code;

        try
        {
            var reply = Post(json, url, out code);
            //var reply = Post(json, url, out code, out message);

            REST_Error message = new REST_Error();

            if (code < 0)
                message.message = "Error de socket, no se puede abrir una conexión";
            else if (code != 200)
            {
                try
                {
                    message = JsonConvert.DeserializeObject<REST_Error>(reply);
                }
                catch (Exception e)
                {
                    message.message = reply;
                }
            }

            message.code = code;

            return message;
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    /// <summary>
    /// Comprueba si el nick o el email no han sido utilizados ya
    /// </summary>
    /// <param name="nick"> Nick a comprobar </param>
    /// <param name="email"> Email a comprobar </param>
    /// <returns> Devuelve un ServerMessage con casteo posible a Available. Si el código es distinto a 200 es posible un casteo a REST_Error </returns>
    public static ServerMessage GetAvailable(string nick = "", string email = "")
    {
        string url = URL + "/accounts/check-availability/?";

        if (nick != "")
        {
            url += "nick=" + nick;

            if (email != "")
                url += "&";
        }

        if (email != "")
            url += "email=" + email;

        int code;

        try
        {
            var reply = Get(url, out code);
            //var reply = Post(json, url, out code, out message);

            if (code != 200)
            {
                REST_Error message = new REST_Error();

                if (code < 0)
                    message.message = "Error de socket, no se puede abrir una conexión";
                else
                {
                    try
                    {
                        message = JsonConvert.DeserializeObject<REST_Error>(reply);
                    }
                    catch (Exception e)
                    {
                        message.message = reply;
                    }
                }

                message.code = code;

                return message;
            }
            else
            {
                Available message = new Available();

                message = JsonConvert.DeserializeObject<Available>(reply);

                message.code = code;

                return message;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// Envío de datos necesarios para el servidor de amtchmaking
    /// </summary>
    /// <param name="gameData"> Clase con los datos necesarios </param>
    /// <returns> Devuelve un ServerMessage con casteo posible a REST_Error si el código es distinto a 200 </returns>
    public static ServerMessage SendRoundInfo(GameData gameData)
    {
        string url = URL + "/accounts/rounds";

        GameEndMessage msg = new GameEndMessage();
        msg.code = 0;
        msg.results = gameData;
        string json = JsonConvert.SerializeObject(msg);

        int code;

        try
        {
            var reply = Post(json, url, out code, true);
            //var reply = Post(json, url, out code, out message);

            REST_Error message = new REST_Error();

            if (code < 0)
                message.message = "Error de socket, no se puede abrir una conexión";
            else if (code != 200)
            {
                try
                {
                    message = JsonConvert.DeserializeObject<REST_Error>(reply);
                }
                catch (Exception e)
                {
                    message.message = reply;
                }
            }

            message.code = code;

            return message;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// Añade al usuario a la cola de matchmaking para inciar la busqueda
    /// </summary>
    /// <returns> Devuelve un ServerMessage con casteo posible a REST_Error si el código es distinto a 200 </returns>
    public static ServerMessage AddToQueue()
    {
        string url = URL + "/matchmaking";

        ServerMessage msg = new ServerMessage();
        msg.code = 0;
        string json = JsonConvert.SerializeObject(msg);

        int code;

        try
        {
            var reply = Post(json, url, out code, true);
            //var reply = Post(json, url, out code, out message);

            REST_Error message = new REST_Error();

            if (code < 0)
                message.message = "Error de socket, no se puede abrir una conexión";
            else if (code != 200)
            {
                try
                {
                    message = JsonConvert.DeserializeObject<REST_Error>(reply);
                }
                catch (Exception e)
                {
                    message.message = reply;
                }
            }

            message.code = code;

            return message;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// Busqueda del rival para una partida mas nivelada posible
    /// </summary>
    /// <param name="waitTime"> Tiempo que lleva el usuario buscando contrincante </param>
    /// <returns> Devuelve un ServerMessage con casteo posible a PairSearch. Si el código es distinto a 200 es posible un casteo a REST_Error  </returns>
    public static ServerMessage SearchPair(float waitTime)
    {
        string url = URL + "/matchmaking/?waitTime=" + waitTime;

        int code;

        try
        {
            var reply = Get(url, out code, true);
            //var reply = Post(json, url, out code, out message);

            if (code != 200)
            {
                REST_Error message = new REST_Error();

                if (code < 0)
                    message.message = "Error de socket, no se puede abrir una conexión";
                else
                {
                    try
                    {
                        message = JsonConvert.DeserializeObject<REST_Error>(reply);
                    }
                    catch (Exception e)
                    {
                        message.message = reply;
                    }
                }

                message.code = code;

                return message;
            }
            else
            {
                PairSearch message = new PairSearch();

                message = JsonConvert.DeserializeObject<PairSearch>(reply);

                message.code = code;

                return message;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// Saca al usuario de la cola de busqueda
    /// </summary>
    /// <returns> Devuelve un ServerMessage con casteo posible a REST_Error si el código es distinto a 200 </returns>
    public static ServerMessage LeaveQueue()
    {
        string url = URL + "/matchmaking";

        ServerMessage msg = new ServerMessage();
        msg.code = 0;
        string json = JsonConvert.SerializeObject(msg);

        int code;

        try
        {
            var reply = Delete(json, url, out code, true);
            //var reply = Post(json, url, out code, out message);

            REST_Error message = new REST_Error();

            if (code < 0)
                message.message = "Error de socket, no se puede abrir una conexión";
            else if (code != 200)
            {
                try
                {
                    message = JsonConvert.DeserializeObject<REST_Error>(reply);
                }
                catch (Exception e)
                {
                    message.message = reply;
                }
            }

            message.code = code;

            return message;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// Genera un nuevo accessToken
    /// </summary>
    /// <returns> Devuelve un ServerMessage con casteo posible a RefreshMessage. Si el código es distinto a 200 es posible un casteo a REST_Error </returns>
    public static ServerMessage Refresh()
    {
        string url = URL + "/accounts/sessions/refresh";

        RefreshData msg = new RefreshData();
        msg.refreshToken = refreshToken;
        string json = JsonConvert.SerializeObject(msg);

        int code;

        try
        {
            var reply = Post(json, url, out code);
            //var reply = Post(json, url, out code, out message);

            if (code != 200)
            {
                REST_Error message = new REST_Error();

                if (code < 0)
                    message.message = "Error de socket, no se puede abrir una conexión";
                else
                {
                    try
                    {
                        message = JsonConvert.DeserializeObject<REST_Error>(reply);
                    }
                    catch (Exception e)
                    {
                        message.message = reply;
                    }
                }

                message.code = code;

                return message;
            }
            else
            {
                RefreshMessage message = new RefreshMessage();

                message = JsonConvert.DeserializeObject<RefreshMessage>(reply);

                authToken = message.accessToken;

                message.code = code;

                return message;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// Busca un servidor de juego al que conectarse
    /// </summary>
    /// <param name="id1"> ID del usuario </param>
    /// <param name="id2"> ID del rival </param>
    /// <returns> Devuelve un ServerMessage con casteo posible a ServerMatchInfo. Si el código es distinto a 200 es posible un casteo a REST_Error </returns>
    public static ServerMessage FindServerInfo(int id1, int id2)
    {
        string url = URL_GAME + "/game-instances";

        Identifiers IDs = new Identifiers();
        IDs.ID1 = id1;
        IDs.ID2 = id2;
        string json = JsonConvert.SerializeObject(IDs);

        int code;

        try
        {
            var reply = Post(json, url, out code);
            //var reply = Post(json, url, out code, out message);

            if (code != 200)
            {
                REST_Error message = new REST_Error();

                if (code < 0)
                    message.message = "Error de socket, no se puede abrir una conexión";
                else
                {
                    try
                    {
                        message = JsonConvert.DeserializeObject<REST_Error>(reply);
                    }
                    catch (Exception e)
                    {
                        message.message = reply;
                    }
                }

                message.code = code;

                return message;
            }
            else
            {
                ServerMatchInfo message = new ServerMatchInfo();

                message = JsonConvert.DeserializeObject<ServerMatchInfo>(reply);

                message.code = code;

                return message;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// Finaliza el servidor al que estaba conectado el usuario
    /// </summary>
    /// <param name="id1"> ID del usuario </param>
    /// <param name="id2"> ID del rival </param>
    /// <returns> Devuelve un ServerMessage con casteo posible a REST_Error si el código es distinto a 200 </returns>
    public static ServerMessage FinishMatch(int id1, int id2)
    {
        string url = URL_GAME + "/game-instances";

        Identifiers IDs = new Identifiers();
        IDs.ID1 = id1;
        IDs.ID2 = id2;
        string json = JsonConvert.SerializeObject(IDs);

        int code;

        try
        {
            var reply = Delete(json, url, out code);
            //var reply = Post(json, url, out code, out message);
            REST_Error message = new REST_Error();

            if (code < 0)
                message.message = "Error de socket, no se puede abrir una conexión";
            else if (code != 200)
            {
                try
                {
                    message = JsonConvert.DeserializeObject<REST_Error>(reply);
                }
                catch (Exception e)
                {
                    message.message = reply;
                }
            }

            message.code = code;

            return message;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// Obtiene la información del usuario
    /// </summary>
    /// <param name="id"> ID del usuario </param>
    /// <returns> Devuelve un ServerMessage con casteo posible a UserDataSmall. Si el código es distinto a 200 es posible un casteo a REST_Error  </returns>
    public static ServerMessage GetInfo(int id)
    {
        string url = URL + "/accounts/by-id/" + id;

        int code;

        try
        {
            var reply = Get(url, out code);
            //var reply = Post(json, url, out code, out message);

            if (code != 200)
            {
                REST_Error message = new REST_Error();

                if (code < 0)
                    message.message = "Error de socket, no se puede abrir una conexión";
                else
                {
                    try
                    {
                        message = JsonConvert.DeserializeObject<REST_Error>(reply);
                    }
                    catch (Exception e)
                    {
                        message.message = reply;
                    }
                }

                message.code = code;

                return message;
            }
            else
            {
                UserDataSmall message = new UserDataSmall();

                message = JsonConvert.DeserializeObject<UserDataSmall>(reply);

                message.code = code;

                return message;
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    /// <summary>
    /// Maneja la conexión con el servidor
    /// </summary>
    /// <param name="request"> Tipo de request </param>
    /// <param name="code"> Código de salida </param>
    /// <returns> Devuelve el string recivido por el servidor de matchmaking. Esta en formato JSON </returns>
    private static string HandleRequest(HttpWebRequest request, out int code)
    {
        try
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                code = (int)response.StatusCode;
                using (Stream strReader = response.GetResponseStream())
                {
                    if (strReader == null) return "";
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        string responseBody = objReader.ReadToEnd();
                        return responseBody;
                    }
                }
            }
        }
        catch (WebException ex)
        {
            using (HttpWebResponse response = (HttpWebResponse)ex.Response)
            {
                if (response == null)
                {
                    code = 503;
                    return "";
                }

                code = (int)response.StatusCode;

                using (Stream strReader = response.GetResponseStream())
                {
                    if (strReader == null) return "";
                    using (StreamReader objReader = new StreamReader(strReader))
                    {
                        string responseBody = objReader.ReadToEnd();
                        return responseBody;
                    }
                }
            }
        }
        catch (SocketException e)
        {
            code = 503;
            return e.Message;
        }
    }

    /// <summary>
    /// Envio de tipo POST
    /// </summary>
    /// <param name="json"> JSON que se va a enviar </param>
    /// <param name="url"> Url al que se le va a enviar </param>
    /// <param name="code"> Código de respuesta </param>
    /// <param name="useAuth"> Añade el header de autenticación </param>
    /// <returns> Devuelve el string enviado desde el servidor de matchmaking </returns>
    private static string Post(string json, string url, out int code, bool useAuth = false)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/json";
        request.Accept = "application/json";

        if (useAuth) request.Headers.Add("Authorization", "Bearer " + authToken);

        code = 503;

        try
        {
            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
        catch (Exception ex)
        {
            return "";
        }

        return HandleRequest(request, out code);
    }

    /// <summary>
    /// Envio de tipo GET
    /// </summary>
    /// <param name="url"> Url al que se le va a enviar </param>
    /// <param name="code"> Código de respuesta </param>
    /// <param name="useAuth"> Añade el header de autenticación </param>
    /// <returns> Devuelve el string enviado desde el servidor de matchmaking </returns>
    private static string Get(string url, out int code, bool useAuth = false)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        request.ContentType = "application/json";
        request.Accept = "application/json";

        if (useAuth) request.Headers.Add("Authorization", "Bearer " + authToken);

        code = 503;

        return HandleRequest(request, out code);
    }

    /// <summary>
    /// Envio de tipo DELETE
    /// </summary>
    /// <param name="json"> JSON que se va a enviar </param>
    /// <param name="url"> Url al que se le va a enviar </param>
    /// <param name="code"> Código de respuesta </param>
    /// <param name="useAuth"> Añade el header de autenticación </param>
    /// <returns> Devuelve el string enviado desde el servidor de matchmaking </returns>
    private static string Delete(string json, string url, out int code, bool useAuth = false)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "DELETE";
        request.ContentType = "application/json";
        request.Accept = "application/json";

        if (useAuth) request.Headers.Add("Authorization", "Bearer " + authToken);

        code = 503;

        try
        {
            using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
        }
        catch (Exception ex)
        {
            return "";
        }

        return HandleRequest(request, out code);
    }
}


