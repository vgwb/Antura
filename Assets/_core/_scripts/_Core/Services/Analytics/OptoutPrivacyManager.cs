using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using System.Collections.Generic;

public class OptoutPrivacyManager : MonoBehaviour
{
    // Store whether opt in consent is required, and what consent ID to use
    string consentIdentifier;
    bool consentHasBeenChecked;

    // Start is called before the first frame update
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
            // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately
            Debug.Log(e.Reason);
        }
    }

    public void OptOut()
    {
        try
        {
            if (!consentHasBeenChecked)
            {
                // Show a GDPR/COPPA/other opt-out consent flow


                // If a user opts out
                AnalyticsService.Instance.OptOut();
            }
            // Record that we have checked a user's consent, so we don't repeat the flow unnecessarily.
            // In a real game, use PlayerPrefs or an equivalent to persist this state between sessions
            consentHasBeenChecked = true;
        }
        catch (ConsentCheckException e)
        {
            // Handle the exception by checking e.Reason
            Debug.Log(e.Reason);
        }
    }

    public void onShowPrivacyPageButtonPressed()
    {
        // Open the Privacy Policy in the system's default browser
        Application.OpenURL(AnalyticsService.Instance.PrivacyUrl);
    }
}
