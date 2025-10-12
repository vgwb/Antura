using Antura.Discover;
using UnityEngine;

namespace Antura.Discover.Activities
{
    /// <summary>
    /// Minimal contract that every activity must expose to be managed consistently.
    /// </summary>
    public interface IActivity
    {
        // ActivityData ActivityData { get; set; }
        // string ActivityCode { get; set; }
        // ActivitySettingsAbstract ConfiguredSettings { get; }
        void ConfigureSettings(ActivitySettingsAbstract settings);
        // void OpenFresh();
        // void HidePanel();
        // void SetActivityLabels(string activityName, string topic);
        // void Pulse(Transform target);
        void InitActivity();

        void OnResetActivity();

        bool DoValidate();

    }
}
