using System.Resources;
using System.Threading;

namespace RecycleBin
{
    /// <summary>
    /// 国际化 - 语言
    /// </summary>
    public class Language
    {
        public static string[] SupportedLanguages = {"简体中文", "English"};
        private string _local = "简体中文";
        private ResourceManager _resourceManager;

        public Language()
        {
            SetLocal(null);
        }

        public Language(string local)
        {
            SetLocal(local);
        }

        public void SetLocal(string local)
        {
            if (string.IsNullOrEmpty(local))
            {
                local = "English";
                if (Thread.CurrentThread.CurrentCulture.Name.ToLower().Contains("zh") ||
                    Thread.CurrentThread.CurrentUICulture.Name.ToLower().Contains("zh"))
                {
                    local = "简体中文";
                }
            }

            if (local == _local && _resourceManager != null) return;

            if (local == "简体中文")
            {
                _local = "简体中文";
                _resourceManager = Properties.ResourcesZhCN.ResourceManager;
            }
            else
            {
                _local = "English";
                _resourceManager = Properties.ResourcesEn.ResourceManager;
            }
        }

        public string Get(string key)
        {
            return _resourceManager?.GetString(key);
        }

        public string GetLocal()
        {
            return _local;
        }
    }
}