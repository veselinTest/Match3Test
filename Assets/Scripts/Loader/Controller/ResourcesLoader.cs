using Assets.Scripts.Loader.Constants;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controller.Loader
{
    public sealed class ResourcesLoader : ScriptableObject
    {
        public void PreloadAssets()
        {
            for (int i = 0; i < ConstantsLoadPaths.ASSETS_NAMES.Length; i++)
            {
                string path = $"{ConstantsLoadPaths.ASSETS_PATH}/{ConstantsLoadPaths.ASSETS_NAMES[i]}/";
                Sprite imageAsset = Resources.Load<Sprite>(path);

                MainGame.Instance.assetsData.assetsNamesToImagesMap[ConstantsLoadPaths.ASSETS_NAMES[i]] = imageAsset;
            }
        }
    }
}
