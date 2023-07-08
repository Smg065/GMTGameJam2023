using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Camera securityCamera;
    public LayerMask clickables;
    public int lastGuard;
    public GuardScript[] allGuards;
    public Button[] guardButtons;
    public Image[] guardWarning;
    public RectTransform thisRect;
    public int patrolSetMode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool leftClick = Input.GetMouseButtonUp(0);
        bool rightClick = Input.GetMouseButtonUp(1);
        if (leftClick || rightClick)
        {
            //Find what area the mouse is in
            Vector2 viewportMouse = securityCamera.ScreenToViewportPoint(Input.mousePosition);

            //Guard Selection
            if (viewportMouse.y < .2f && viewportMouse.x > .4f) return;

            //Project from the guard cam, not the security cam
            Camera rayProjection = securityCamera;
            if (lastGuard != -1)
            {
                if (allGuards[lastGuard].viewCam.gameObject.activeInHierarchy)
                {
                    //Check that it's in the box
                    if (viewportMouse.y >= .2f && viewportMouse.y < .4f && viewportMouse.x >= .7f && viewportMouse.x < .95f)
                    {
                        rayProjection = allGuards[lastGuard].viewCam;
                    }
                }
            }

            RaycastHit clickedItem = ClickedData(rayProjection);
            if (clickedItem.transform != null)
            {
                if (lastGuard != -1)
                {
                    if (leftClick)
                    {
                        //Pathing Mode
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            switch (clickedItem.transform.gameObject.layer)
                            {
                                //Follow guards, civilians, or theives if they're clicked
                                case int n when n >= 8 && n <= 10:
                                    allGuards[lastGuard].guardChaseTarget = clickedItem.transform;
                                    break;
                                //Change Patrol Type
                                default:
                                    ChangeLastGuardPriorities();
                                    allGuards[lastGuard].guardWaypoint1.position = clickedItem.point;
                                    allGuards[lastGuard].patrolFirstPoint = true;
                                    break;
                            }
                        }
                        //Search Override the guard
                        else
                        {
                            allGuards[lastGuard].searchOverride = true;
                            allGuards[lastGuard].goalTransform.position = clickedItem.point;
                        }
                    }
                    if (rightClick)
                    {
                        ChangeLastGuardPriorities();
                        allGuards[lastGuard].guardWaypoint2.position = clickedItem.point;
                        allGuards[lastGuard].patrolFirstPoint = false;
                    }
                }
            }
        }
    }
    public void ChangeLastGuardPriorities()
    {
        allGuards[lastGuard].searchOverride = false;
        allGuards[lastGuard].currentGoalPos = allGuards[lastGuard].transform.position;
        allGuards[lastGuard].guardMode = patrolSetMode;
        allGuards[lastGuard].guardChaseTarget = null;
        allGuards[lastGuard].waitTime = .2f;
    }
    public void OfficerSelected(int currentOfficer)
    {
        //Switch guards
        GuardScript currentGuard = allGuards[currentOfficer];
        //Clear the last guard info
        if (lastGuard != -1)
        {
            //Clear the last warning
            guardWarning[lastGuard].color = Color.red;
            guardWarning[lastGuard].gameObject.SetActive(false);
            allGuards[lastGuard].viewCam.enabled = false;
        }
        lastGuard = currentOfficer;
        patrolSetMode = currentGuard.guardMode;
        //If the guard is down, showcase that now
        if (!currentGuard.gameObject.activeInHierarchy)
        {
            lastGuard = -1;
            guardWarning[currentOfficer].gameObject.SetActive(false);
            guardButtons[currentOfficer].interactable = false;
            return;
        }

        //Turn on this guard's camera
        allGuards[currentOfficer].viewCam.enabled = true;
        guardWarning[currentOfficer].gameObject.SetActive(true);
        guardWarning[currentOfficer].color = Color.white;
    }
    public RaycastHit ClickedData(Camera currentCamera)
    {
        Physics.Raycast(currentCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit clickedItem, 30, clickables, QueryTriggerInteraction.Ignore);
        return clickedItem;
    }
}
