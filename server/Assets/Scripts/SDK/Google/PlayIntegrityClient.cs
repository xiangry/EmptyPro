using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SDK.Google.PlayIntegrity;
using UnityEngine;

public class PlayIntegrityClient : MonoBehaviour
{

    private static PlayIntegrityClient _instance;
    public static PlayIntegrityClient Instance => _instance;
    
    // Start is called before the first frame update
    async void Start()
    {
        await PlayIntegrityManager.Instance.Init();
        _instance = this;
    }



    public async Task<bool> DecodeToken(string token)
    { 
        return await PlayIntegrityManager.Instance.DecodeIntegrityToken(token);
    }
    
}
