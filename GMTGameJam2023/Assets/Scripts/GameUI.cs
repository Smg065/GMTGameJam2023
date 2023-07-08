using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Camera lastGuardCam;
    public GuardScript[] allGuards;
    public Button[] guardButtons;
    public Image[] guardWarning;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OfficerSelected(int currentOfficer)
    {
        //Switch guards
        GuardScript currentGuard = allGuards[currentOfficer];
        //Clear the last cam
        if (lastGuardCam != null) lastGuardCam.enabled = false;
        lastGuardCam = currentGuard.viewCam;

        //If the guard is down, showcase that now
        if (!currentGuard.gameObject.activeInHierarchy)
        {
            lastGuardCam = null;
            guardWarning[currentOfficer].enabled = false;
            guardButtons[currentOfficer].interactable = false;
            return;
        }

        //Turn on this guard's camera
        lastGuardCam.enabled = true;
    }
}
