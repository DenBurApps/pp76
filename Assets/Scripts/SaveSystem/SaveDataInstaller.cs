using Firebase.Extensions;
using Firebase.RemoteConfig;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataInstaller : MonoBehaviour
{
    [SerializeField] private bool _fromTheBeginning;
    [SerializeField] private ConfigData _allConfigData;
    private bool _showTerms = true;

    private void Start()
    {
        InstallBindings();
    }

    private void InstallBindings()
    {
        BindFileNames();
        BindRegistration();
        BindRooms();
        BindGuests();
        BindSettings();
        StartLoading();
    }

    private void StartLoading()
    {
        string HtmlText = GetHtmlFromUri("http://google.com");

        if (HtmlText != "")
        {
            LoadFirebaseConfig();
        }

        else
        {
            LoadScene();
        }
    }

    public void LoadFirebaseConfig()
    {
        CheckRemoteConfigValues();
    }


    public Task CheckRemoteConfigValues()
    {
        Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            LoadScene();
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            LoadScene();
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync()
          .ContinueWithOnMainThread(
            task =>
            {
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");

                foreach (var item in remoteConfig.AllValues)
                {
                    switch (item.Key)
                    {
                        case "url":
                            {
                                _allConfigData.Url = item.Value.StringValue;
                                break;
                            }
                        case "showAgree":
                            {
                                _allConfigData.ShowAgree = item.Value.BooleanValue;
                                break;
                            }
                    }
                }

                _showTerms = _allConfigData.ShowAgree;
                Debug.Log(_showTerms + "/" + _allConfigData.ShowAgree);
                var reg = SaveSystem.LoadData<RegistrationSaveData>();
                reg.Link = _allConfigData.Url;
                SaveSystem.SaveData(reg);
                LoadScene();
            });

    }

    private void LoadScene()
    {
        if (_showTerms)
        {
            if (PlayerPrefs.HasKey("Onboarding"))
            {
                SceneManager.LoadScene("MainScene");
            }
            else
            {
                SceneManager.LoadScene("Onboarding");
            }
        }
        else
        {
            SceneManager.LoadScene("RoomScene");
        }
        
    }

    private void BindRegistration()
    {
        {
            var reg = SaveSystem.LoadData<RegistrationSaveData>();

#if UNITY_EDITOR
            if (_fromTheBeginning)
            {
                reg = null;
            }
#endif

            if (reg == null)
            {
                reg = new RegistrationSaveData("", false);
                SaveSystem.SaveData(reg);
            }

        }
    }

    private void BindSettings()
    {
        {
            var settings = SaveSystem.LoadData<SettingSaveData>();

#if UNITY_EDITOR
            if (_fromTheBeginning)
            {
                settings = null;
            }
#endif

            if (settings == null)
            {
                settings = new SettingSaveData(new List<bool> {false, false, false, false, false, false, false, false, false });
                SaveSystem.SaveData(settings);
            }

        }
    }

    private void BindRooms()
    {
        {
            var products = SaveSystem.LoadData<RoomSaveData>();

#if UNITY_EDITOR
            if (_fromTheBeginning)
            {
                products = null;
            }
#endif

            if (products == null)
            {
                products = new RoomSaveData(new List<Room>(), 0);
                SaveSystem.SaveData(products);
            }

        }
    }

    private void BindGuests()
    {
        {
            var guests = SaveSystem.LoadData<GuestsSaveData>();

#if UNITY_EDITOR
            if (_fromTheBeginning)
            {
                guests = null;
            }
#endif

            if (guests == null)
            {
                guests = new GuestsSaveData(new List<Guest>(), 0);
                SaveSystem.SaveData(guests);
            }

        }
    }


    private void BindFileNames()
    {
        FileNamesContainer.Add(typeof(RegistrationSaveData), FileNames.RegData);
        FileNamesContainer.Add(typeof(SettingSaveData), FileNames.SettingsData);
        FileNamesContainer.Add(typeof(RoomSaveData), FileNames.RoomsData);
        FileNamesContainer.Add(typeof(GuestsSaveData), FileNames.GuestsData);
    }

    public string GetHtmlFromUri(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        //We are limiting the array to 80 so we don't have
                        //to parse the entire html document feel free to 
                        //adjust (probably stay under 300)
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }

}

[Serializable]
public class ConfigData
{
    public string Url;
    public bool ShowAgree;
}