using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using UnityEditor.PackageManager;

public class MessageResponse
{
    public string text;
}

public class TextTokensResponse
{
    public int tokens;
}

public class TextEmbeddingsVectorResponse
{
    public List<double> vector;
}

public class TextSimilarityRequest
{
    public List<double> vector1;
    public List<double> vector2;
}

public class TextSimilarityResponse
{
    public double value; // 0..1
}

public class LoginRequest
{
    public string username;
    public string password;
}
public class RegisterRequest
{
    public string username;
    public string password1;
    public string password2;
}

public class RegisterResponse
{
    public bool success;
    public Dictionary<string, List<string>> errors;
}

public class LoginResponse
{
    public bool success;
    public string token;
    public Dictionary<string, List<string>> errors;
}


public class ClientAPI : MonoBehaviour
{
    const string RUN_LLM_ENDPOINT = "/llm/generate/";
    const string COUNT_TOKENS_ENDPOINT = "/llm/tokens/";
    const string TEXT_EMBEDDINGS_ENDPOINT = "/embeddings/embed/";
    const string TEXT_SIMILARITY_ENDPOINT = "/embeddings/compare/";
    const string LOGIN_ENDPOINT = "/userdata/login/";
    const string REGISTER_ENDPOINT = "/userdata/register/";

    public static Action<string, string, bool> OnResponcePrompt;
    public static Action<bool> OnStartResponce;
    public static ClientAPI instance;
    [SerializeField] public string host = "http://127.0.0.1:8000";

    // токен с геттером
    public string token = null;

    // данные кеша
    private Dictionary<string, TextTokensResponse> countTokensCache = new Dictionary<string, TextTokensResponse>();
    private Dictionary<string, TextEmbeddingsVectorResponse> textEmbeddingsCache = new Dictionary<string, TextEmbeddingsVectorResponse>();
    private Dictionary<string, TextSimilarityResponse> textSimilarityCache = new Dictionary<string, TextSimilarityResponse>();

    private void Start()
    {
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        DialogManager.OnSendPromptAI += Listen;
    }

    private void OnDisable()
    {
        DialogManager.OnSendPromptAI -= Listen;
    }

    private async Task<string> SendPOST(string endpoint, string jsonString)
    {
        OnStartResponce?.Invoke(true);

        string url = string.Format("{0}" + endpoint, host);

        using var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonString);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        var asyncOperation = uwr.Send();

        // Ожидаем завершения операции
        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }

        if (asyncOperation.isDone)
        {
            OnStartResponce?.Invoke(false);
        }

        if (uwr.result != UnityWebRequest.Result.Success)
        {

            Debug.Log("Error While Sending: " + uwr.error);
            return null;
        }
        else
        {

            return uwr.downloadHandler.text;
        }
    }

    //todo: в этой функции только возвращать значение
    public async void Listen(string prompt)
    {
        Debug.Log($"Listen Message");

        string data = JsonConvert.SerializeObject(new { prompt = prompt });
        var response = JsonConvert.DeserializeObject<MessageResponse>(await SendPOST(RUN_LLM_ENDPOINT, data));
        string responseText = response.text.Trim();
        // Вызываем событие когда ИИ дает ответ
        // отрисовка сообщения ИИ в UI чата
        OnResponcePrompt?.Invoke(DialogManager.instance.CurrentNPC.AIData().characterName, responseText, true);
        // добавление ответа ИИ в историю диалога
        Reply lastReply = DialogManager.instance.CurrentNPC.AIData().chatHistory.LastReply();
        lastReply.message = responseText;
    }

    public async Task<MessageResponse> RunLLM(string prompt)
    {
        string data = JsonConvert.SerializeObject(new { prompt = prompt });
        var response = JsonConvert.DeserializeObject<MessageResponse>(await SendPOST(RUN_LLM_ENDPOINT, data));
        return response;
    }

    public async Task<TextTokensResponse> CountTokens(string text)
    {
        if (countTokensCache.ContainsKey(text))
        {
            return countTokensCache[text];
        }
        string data = JsonConvert.SerializeObject(new { prompt = text });
        var response = JsonConvert.DeserializeObject<TextTokensResponse>(await SendPOST(COUNT_TOKENS_ENDPOINT, data));
        countTokensCache[text] = response;
        return response;
    }

    public async Task<TextEmbeddingsVectorResponse> TextEmbeddings(string text)
    {
        if (textEmbeddingsCache.ContainsKey(text))
        {
            return textEmbeddingsCache[text];
        }
        string data = JsonConvert.SerializeObject(new { prompt = text });
        var response = JsonConvert.DeserializeObject<TextEmbeddingsVectorResponse>(await SendPOST(TEXT_EMBEDDINGS_ENDPOINT, data));
        textEmbeddingsCache[text] = response;
        return response;
    }

    public async Task<TextSimilarityResponse> TextSimilarity(List<double> vector1, List<double> vector2)
    {
        string cacheKey = vector1.GetHashCode().ToString() + vector2.GetHashCode().ToString();
        if (textSimilarityCache.ContainsKey(cacheKey))
        {
            return textSimilarityCache[cacheKey];
        }
        var requestData = new TextSimilarityRequest
        {
            vector1 = vector1,
            vector2 = vector2
        };
        string data = JsonConvert.SerializeObject(requestData);
        var response = JsonConvert.DeserializeObject<TextSimilarityResponse>(await SendPOST(TEXT_SIMILARITY_ENDPOINT, data));
        textSimilarityCache[cacheKey] = response;
        return response;
    }

    public async Task<RegisterResponse> Register(string name, string password)
    {
        var requestData = new RegisterRequest
        {
            username = name,
            password1 = password,
            password2 = password
        };
        string data = JsonConvert.SerializeObject(requestData);
        var strResponse = await SendPOST(REGISTER_ENDPOINT, data);
        if (strResponse == null)
        {
            var response = new RegisterResponse
            {
                success = false,
                errors = new Dictionary<string, List<string>>()
            };
            response.errors["non_field_errors"] = new List<string> { "Connection problem" };
            return response;
        }
        else
        {
            var response = JsonConvert.DeserializeObject<RegisterResponse>(strResponse);
            return response;
        }

    }

    public async Task<LoginResponse> Login(string name, string password)
    {
        var requestData = new LoginRequest
        {
            username = name,
            password = password
        };
        string data = JsonConvert.SerializeObject(requestData);
        var strResponse = await SendPOST(LOGIN_ENDPOINT, data);
        if (strResponse == null)
        {
            var response = new LoginResponse
            {
                success = false,
                errors = new Dictionary<string, List<string>>()
            };
            response.errors["non_field_errors"] = new List<string> { "Connection problem" };
            return response;
        }
        else
        {
            var response = JsonConvert.DeserializeObject<LoginResponse>(strResponse);
            token = response.token;
            return response;
        }
    }

    public async Task<bool> Logout()
    {
        // todo: logout request

        token = null;

        return true;
    }

    public async void LoadPlayerData()
    {
        token = null;
    }

    public async void SavePlayerData()
    {
        token = null;
    }

    public bool IsLoggedIn()
    {
        return !string.IsNullOrEmpty(token);
    }

}
