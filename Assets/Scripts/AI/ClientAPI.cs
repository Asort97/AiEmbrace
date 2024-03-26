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

public class Error
{
    public int code;
    public string message;
}

public class TokensCountResponse
{
    public int prompt_tokens; // tokens in prompt
    public int completion_tokens; // how many tokens were added by AI
    public int total_tokens; // tokens in prompt + AI response
}

public class MessageResponse
{
    public string text;
    public bool success;
    public List<Error> errors;
    public TokensCountResponse tokens;
}

public class MessageRequest
{
    public string prompt;
    public string preset;
    //todo: add character preset
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

public class SaveDataRequest
{
    public string game_data;
    public string version;
}
public class SaveDataResponse
{
    public bool success;
    public Dictionary<string, List<string>> errors;
}

public class LoadDataRequest
{
    public string version;
}
public class LoadDataResponse
{
    public bool success;
    public string game_data;
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
    const string SAVE_DATA_ENDPOINT = "/userdata/save/";
    const string LOAD_DATA_ENDPOINT = "/userdata/load/";

    public static Action<string, string, bool> OnResponcePrompt;
    public static Action<bool> OnStartResponce;
    private static ClientAPI _instance;
    [SerializeField] public string host = "http://127.0.0.1:8000";

    // токен с геттером
    public string token = null;

    // данные кеша
    private Dictionary<string, TokensCountResponse> countTokensCache = new Dictionary<string, TokensCountResponse>();
    private Dictionary<string, TextEmbeddingsVectorResponse> textEmbeddingsCache = new Dictionary<string, TextEmbeddingsVectorResponse>();
    private Dictionary<string, TextSimilarityResponse> textSimilarityCache = new Dictionary<string, TextSimilarityResponse>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static ClientAPI Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ClientAPI>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private async Task<string> SendPOST(string endpoint, string jsonString, bool authRequired = false)
    {
        string url = string.Format("{0}" + endpoint, host);

        using var uwr = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonString);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");

        if (authRequired)
        {
            uwr.SetRequestHeader("Authorization", $"Token {token}");
        }

        var asyncOperation = uwr.Send();

        // Ожидаем завершения операции
        while (!asyncOperation.isDone)
        {
            await Task.Yield();
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

    public async Task<MessageResponse> RunLLM(string prompt, string preset="character")
    {
        string data = JsonConvert.SerializeObject(new { prompt = prompt, preset = preset });
        var response = JsonConvert.DeserializeObject<MessageResponse>(await SendPOST(RUN_LLM_ENDPOINT, data));
        return response;
    }

    public async Task<TokensCountResponse> CountTokens(string text)
    {
        if (countTokensCache.ContainsKey(text))
        {
            return countTokensCache[text];
        }
        string data = JsonConvert.SerializeObject(new { prompt = text });
        var response = JsonConvert.DeserializeObject<TokensCountResponse>(await SendPOST(COUNT_TOKENS_ENDPOINT, data));
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

    public async Task<SaveDataResponse> SaveData(GameDataForStorage gameDataForStorage, string version)
    {
        var requestData = new SaveDataRequest
        {
            game_data = JsonConvert.SerializeObject(gameDataForStorage),
            version = version
        };

        string data = JsonConvert.SerializeObject(requestData);
        var strResponse = await SendPOST(SAVE_DATA_ENDPOINT, data, true);
        if (strResponse == null)
        {
            var response = new SaveDataResponse
            {
                success = false,
                errors = new Dictionary<string, List<string>>()
            };
            response.errors["non_field_errors"] = new List<string> { "Connection problem" };
            return response;
        }
        else
        {
            var response = JsonConvert.DeserializeObject<SaveDataResponse>(strResponse);
            return response;
        }
    }

    public async Task<GameDataForStorage> LoadData(string version)
    {
        var requestData = new LoadDataRequest
        {
            version = version
        };
        string data = JsonConvert.SerializeObject(requestData);
        var strResponse = await SendPOST(LOAD_DATA_ENDPOINT, data, true);
        if (strResponse == null)
        {
            return null;
        }
        else
        {
            Debug.Log("strResponse = " + strResponse);
            var response = JsonConvert.DeserializeObject<LoadDataResponse>(strResponse);
            if (response.success)
            {
                return JsonConvert.DeserializeObject<GameDataForStorage>(response.game_data);
            }
            else
            {
                return null;
            }
        }
    }

    public async Task<bool> Logout()
    {
        // todo: logout request

        token = null;

        return true;
    }

    public bool IsLoggedIn()
    {
        return !string.IsNullOrEmpty(token);
    }

}
