using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine;

public class UIProfileSelectionController : MonoBehaviour
{
    public static UIProfileSelectionController Instance;


    public AudioClip TitleSound;

    public UIProfileSelectionButton[] profiles;

    //[HideInInspector]
    //public GameObject NextScreen;

    [HideInInspector]
    public string NextScene;

    [HideInInspector]
    public bool skipConfirmationPopup;

    [HideInInspector]
    public bool allowToSelectCurrentProfile = false;

    [HideInInspector]
    public Transform popupsHolder;


    UIProfileSelectionButton currentButton;

    //bool isFirstUse = true;

    void Awake()
    {
        Instance = this;
    }


    void OnDisable()
    {
        //NextScreen = null;
        allowToSelectCurrentProfile = false;
    }

    void OnEnable()
    {
        if (AudioController.Instance)
        {
            AudioController.Instance.PlaySound(TitleSound);
        }
        UpdateAllProfiles();
    }


    void UpdateAllProfiles()
    {
        foreach (UIProfileSelectionButton profile in profiles)
        {
            if (profile != null)
            {
                if (profile.ProfileId.Equals(UsersController.Instance.CurrentProfileId) && allowToSelectCurrentProfile)
                {
                    profile.MarkSelected();
                }
                else
                {
                    profile.MarkUnSelected();
                }
            }
        }

        //isFirstUse = false;
    }


    public void OnCloseClick()
    {
        Destroy(gameObject);
    }

    public void OnChangeProfile(UIProfileSelectionButton button)
    {
        currentButton = button;

        if (UsersController.Instance.CurrentProfileId.Equals(button.ProfileId) || skipConfirmationPopup)
        {
            ChangeProfile();
        }
        else
        {
            showConfirmaction();
        }
        skipConfirmationPopup = false;
    }

    void ChangeProfile()
    {
        UsersController.Instance.CurrentProfileId = currentButton.ProfileId;

        UsersController.Instance.userData().addFirstFriendsToCollection();

        Analytics.Instance.TrackEvent(FirebaseCustomEventNames.EventSelectProfile, 
            new Parameter(FirebaseCustomParameterNames.ParameterProfile, currentButton.ProfileId));

        SceneController.Instance.LoadScene(NextScene);
    }

    void showConfirmaction()
    {
        //UIController.Instance.ConfirmationPopup.onConfirm = ChangeProfile;
        //UIController.Instance.ShowPopup (UIController.Instance.ConfirmationPopup.gameObject);

        GameObject popup = Instantiate(Resources.Load("Gameplay/Popups/Popup Panel - Confirmation") as GameObject, popupsHolder);


        UIConfirmationPopup psc = popup.GetComponent<UIConfirmationPopup>();
        psc.onConfirm = ChangeProfile;
    }
}
