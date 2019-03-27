//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: For controlling in-game objects with tracked devices.
//
//=============================================================================

using UnityEngine;
using Valve.VR;

public class SteamVR_TrackedObject : MonoBehaviour
{
	public enum EIndex
	{
		None = -1,
		Hmd = (int)OpenVR.k_unTrackedDeviceIndex_Hmd,
		Device1,
		Device2,
		Device3,
		Device4,
		Device5,
		Device6,
		Device7,
		Device8,
		Device9,
		Device10,
		Device11,
		Device12,
		Device13,
		Device14,
		Device15
	}

	public EIndex index;

	[Tooltip("If not set, relative to parent")]
	public Transform origin;

    public bool isValid { get; private set; }
	private int caseSwitch = -1;
	private readonly SaveTablePos _saveTablePos = new SaveTablePos();
    private void Awake()
    {
        if (caseSwitch == -1 && gameObject.name == "Tracker")
        {
            Debug.Log("To Start calibrating new Table press 'Delete' ");
            caseSwitch++;
        }
    }

	private void Update()
    {
        if (gameObject.name == "Tracker")
        {

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                switch (caseSwitch)
                {
                    case 0:
                        Debug.Log(" Place the tracker on the following positions and press 'Delete'");
                        Debug.Log("1. Left front corner");
                        Debug.Log("2. Right front corner");
                        Debug.Log("3. Left Back corner");
                        Debug.Log("4. floor");
                        Debug.Log("5. Press 'Delete' again to save");
                        caseSwitch++;
                        break;
                    case 1:
                        Variables.TopFrontLeft = GameObject.Find("Tracker").transform.position;
                        Debug.Log("TopFrontLeft: " + Variables.TopFrontLeft);
                        caseSwitch++;
                        break;
                    case 2:
                        Variables.TopFrontRight = GameObject.Find("Tracker").transform.position;
                        Debug.Log("TopFrontRight: " + Variables.TopFrontRight);
                        caseSwitch++;
                        break;
                    case 3:
                        Variables.TopBackLeft = GameObject.Find("Tracker").transform.position;
                        Debug.Log("TopBackLeft: " + Variables.TopBackLeft);
                        caseSwitch++;
                        break;
                    case 4:
                        Variables.TableHeight = Variables.TopFrontLeft.y - GameObject.Find("Tracker").transform.position.y;
                        Debug.Log(GameObject.Find("Tracker").transform.position.y);
                        Debug.Log(Variables.TopFrontLeft.y);
                        Debug.Log("TableHeight: " + Variables.TableHeight);
                        caseSwitch++;
                        break;
                    case 5:
                        _saveTablePos.SaveFile();
                        Debug.Log("Data Saved. Restart to load new table");
                        caseSwitch = 0;
                        break;

                    default:
                        break;
                }

            }
        }
    }
    private void OnNewPoses(TrackedDevicePose_t[] poses)
	{
		if (index == EIndex.None)
			return;

		var i = (int)index;

        isValid = false;
		if (poses.Length <= i)
			return;

		if (!poses[i].bDeviceIsConnected)
			return;

		if (!poses[i].bPoseIsValid)
			return;

        isValid = true;

		var pose = new SteamVR_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);

		if (origin != null)
		{
			transform.position = origin.transform.TransformPoint(pose.pos);
			transform.rotation = origin.rotation * pose.rot;
		}
		else
		{
			transform.localPosition = pose.pos;
			transform.localRotation = pose.rot;
		}
	}

	private SteamVR_Events.Action newPosesAction;

	private SteamVR_TrackedObject()
	{
		newPosesAction = SteamVR_Events.NewPosesAction(OnNewPoses);
	}

	private void OnEnable()
	{
		var render = SteamVR_Render.instance;
		if (render == null)
		{
			enabled = false;
			return;
		}

		newPosesAction.enabled = true;
	}

	private void OnDisable()
	{
		newPosesAction.enabled = false;
		isValid = false;
	}

	public void SetDeviceIndex(int index)
	{
		if (System.Enum.IsDefined(typeof(EIndex), index))
			this.index = (EIndex)index;
	}
}

