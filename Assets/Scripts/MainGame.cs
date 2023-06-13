using Assets.Scripts.Controller.Loader;
using Assets.Scripts.Grid.Controller;
using Assets.Scripts.Grid.Model;
using UnityEngine;

public sealed class MainGame : MonoBehaviour
{
    public GridGenerator gridGenerator;

    public AssetsData assetsData;

    private ResourcesLoader _resourcesLoader;


    private static MainGame _instance;

    public static MainGame Instance
    {
        get
        {
            if (_instance == null)
            {
                // If no _instance exists, find or create the MainGame in the scene
                _instance = FindObjectOfType<MainGame>();

                // If no MainGame exists in the scene, create a new one
                if (_instance == null)
                {
                    GameObject obj = new GameObject("MainGame");
                    _instance = obj.AddComponent<MainGame>();
                }

#if !UNITY_EDITOR
                // Don't destroy the MainGame when loading new scenes
                DontDestroyOnLoad(_instance.gameObject);
#endif
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            // Destroy duplicate MainGame instances
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        Init();
    }

    private void Init()
    {
        assetsData = new AssetsData();

        _resourcesLoader = ScriptableObject.CreateInstance<ResourcesLoader>();
        _resourcesLoader.PreloadAssets();
    }

    private void OnApplicationQuit()
    {
        // clear models and controllers
    }
}
