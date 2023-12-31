using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text securityText;
    public Camera[] securityCameras;
    public string[] securityCameraName;
    public int currentCamera;
    public LayerMask clickables;
    public int lastGuard;
    public GuardScript[] allGuards;
    public Button[] guardButtons;
    public Image[] guardWarning;
    public int patrolSetMode;
    public Image[] patrolModeHighlight;
    public Transform wanderRadius;
    public LineRenderer patrolLine;
    public BillboardRenderer guardStand;
    public Material billboardMat;
    public Transform guardPointIcon;
    public Transform guardPointSubicon;
    public Transform alertIcon;
    public Transform alertSubicon;
    // Start is called before the first frame update
    void Start()
    {
        securityText.text = securityCameraName[currentCamera];
    }

    // Update is called once per frame
    void Update()
    {
        //Input
        bool leftClick = Input.GetMouseButtonUp(0);
        bool rightClick = Input.GetMouseButtonUp(1);
        if (leftClick || rightClick)
        {
            //Find what area the mouse is in
            Vector2 viewportMouse = securityCameras[currentCamera].ScreenToViewportPoint(Input.mousePosition);

            //Guard Selection
            if (viewportMouse.y < .2f && viewportMouse.x > .4f) return;
            //Camera Selection
            if (viewportMouse.y < .1f && viewportMouse.x <= .4f) return;
            //Patrol Type
            if (viewportMouse.y >= .8f && viewportMouse.x <= .4f) return;

            //Project from the guard cam, not the security cam
            Camera rayProjection = securityCameras[currentCamera];
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
                                    if (clickedItem.transform != allGuards[lastGuard].transform) allGuards[lastGuard].guardChaseTarget = clickedItem.transform;
                                    else allGuards[lastGuard].guardChaseTarget = null;
                                    break;
                                //Change Patrol Type
                                default:
                                    ChangeLastGuardPriorities();
                                    allGuards[lastGuard].guardWaypoint1.position = clickedItem.point;
                                    allGuards[lastGuard].patrolFirstPoint = true;
                                    allGuards[lastGuard].goalTransform.position = clickedItem.point;
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
                        allGuards[lastGuard].goalTransform.position = clickedItem.point;
                    }
                }
            }
        }

        //Switch guard display mode
        DisplayGuardMode();

        //Make the icons face the right cams
        guardPointIcon.LookAt(securityCameras[currentCamera].transform.position, Vector3.up);
        alertIcon.LookAt(securityCameras[currentCamera].transform.position, Vector3.up);
        if (lastGuard != -1)
        {
            guardPointSubicon.LookAt(allGuards[lastGuard].transform.position, Vector3.up);
            alertSubicon.LookAt(allGuards[lastGuard].transform.position, Vector3.up);
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
    public void SwitchCamera(int camMoveDir)
    {
        securityCameras[currentCamera].enabled = false;
        currentCamera = (currentCamera + camMoveDir + 10) % 10;
        securityCameras[currentCamera].enabled = true;
        securityText.text = securityCameraName[currentCamera];
    }
    public RaycastHit ClickedData(Camera currentCamera)
    {
        Physics.Raycast(currentCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit clickedItem, 30, clickables, QueryTriggerInteraction.Ignore);
        return clickedItem;
    }
    public void DisplayGuardMode()
    {
        //Switch guard mode display
        int effectivePatrol = patrolSetMode;
        //It's 1 if you're chasing a guy
        if (lastGuard != -1)
        {
            if (allGuards[lastGuard].guardChaseTarget != null) effectivePatrol = 1;
            alertIcon.gameObject.SetActive(allGuards[lastGuard].searchOverride);
            if (allGuards[lastGuard].searchOverride)
            {
                alertIcon.transform.position = allGuards[lastGuard].currentGoalPos + Vector3.up;
            }
        }
        else
        {
            effectivePatrol = -1;
            alertIcon.gameObject.SetActive(false);
        }
        //Show the active elements
        for (int eachHighlight = 0; eachHighlight < 4; eachHighlight++)
        {
            patrolModeHighlight[eachHighlight].gameObject.SetActive(effectivePatrol == eachHighlight);
        }
        patrolLine.gameObject.SetActive(effectivePatrol == 2 || effectivePatrol == 1);
        wanderRadius.gameObject.SetActive(effectivePatrol == 3);
        guardPointIcon.gameObject.SetActive(effectivePatrol == 0);
        switch (effectivePatrol)
        {
            case 0:
                guardPointIcon.transform.position = allGuards[lastGuard].guardWaypoint1.position + Vector3.up;
                break;
            case 1:
                DrawPathLine(allGuards[lastGuard].transform.position, allGuards[lastGuard].guardChaseTarget.position);
                break;
            case 2:
                DrawPathLine(allGuards[lastGuard].guardWaypoint1.position, allGuards[lastGuard].guardWaypoint2.position);
                break;
            case 3:
                wanderRadius.transform.position = allGuards[lastGuard].guardWaypoint1.position;
                float pointDistance = (Vector3.Distance(allGuards[lastGuard].guardWaypoint1.position, allGuards[lastGuard].guardWaypoint2.position) * 2) + .5f;
                wanderRadius.localScale = new Vector3(pointDistance, 1, pointDistance);
                break;
        }
    }
    public void ChangePatrolMode(int newMode)
    {
        if (lastGuard == -1) return;
        patrolSetMode = newMode;
        allGuards[lastGuard].guardChaseTarget = null;
        allGuards[lastGuard].guardMode = newMode;
    }
    public void DrawPathLine(Vector3 pos1, Vector3 pos2)
    {
        NavMeshPath currentPath = new NavMeshPath();
        NavMesh.CalculatePath(pos1, pos2, 1, currentPath);
        patrolLine.numCornerVertices = currentPath.corners.Length;
        patrolLine.positionCount = currentPath.corners.Length;
        //Corners
        for (int eachCorner = 0; eachCorner < currentPath.corners.Length; eachCorner++)
        {
            patrolLine.SetPosition(eachCorner, currentPath.corners[eachCorner] + Vector3.up);
        }
    }
}
