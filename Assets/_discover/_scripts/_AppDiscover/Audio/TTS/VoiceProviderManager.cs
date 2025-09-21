using Antura.Discover;
using Antura.Utilities;
using UnityEngine;
using UnityEngine.Localization;

namespace Antura.Discover.Audio
{
    public class VoiceProviderManager : SingletonMonoBehaviour<VoiceProviderManager>
    {
        [SerializeField]
        private ScriptableObject _providerAsset;

        private IVoiceProvider _provider;
        protected override void Awake()
        {
            base.Awake();
            if (_providerAsset is IVoiceProvider vp)
            {
                _provider = vp;
            }
        }

        public IVoiceProvider Provider => _provider;

        public VoiceProfileData GetProfile(Locale locale, VoiceActors actor)
        {
            return _provider != null ? _provider.GetProfile(locale, actor) : null;
        }

        public VoiceProfileData GetProfile(string languageCode, VoiceActors actor)
        {
            return _provider != null ? _provider.GetProfile(languageCode, actor) : null;
        }
    }
}
