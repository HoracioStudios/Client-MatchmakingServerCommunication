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

    public static void Init(string IP_, string PORT_)
    {
        IP = IP_;
        PORT = PORT_;
        URL = "http://" + IP + ":" + PORT;
        URL_GAME = "http://" + IP + ":" + PORT_GAME_SERVER;
    }

    //Devuelve el id del usuario
    public static ServerMessage LogIn(string password, string username)
    {
        string url = URL + "/accounts/sessions";

        LoginInfo user = new LoginInfo();
        user.nick = username;
        user.password = password;
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

    //Devuelve el id del usuario
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

    // 1 si esta disponible 0 si no lo esta
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

    //id1: id de este jugador id2: id del rival
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


