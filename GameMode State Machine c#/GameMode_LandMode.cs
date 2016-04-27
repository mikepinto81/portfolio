using UnityEngine;
using System.Collections;
using System;

public class GameMode_LandMode : GameMode_Base {

    public override void SetupAwake()
    {
        ID = "LandMode";
    }

    public override IEnumerator EnterMode()
    {
        //Set the camera to view land area        
        SE_CameraControl.i.Center2DCamOnLand();

        //turn on landUi
        SE_GameManager.i.landManager.landUi.gameObject.SetActive(true);        
        
        //turn on land controls
        SE_InputManager.i.TurnOnLandController();

        yield return null;
    }

    public override IEnumerator ExitMode()
    {
        //disable land ui
        SE_GameManager.i.landManager.landUi.gameObject.SetActive(false);

        //disable controls
        SE_InputManager.i.TurnOffLandController();
        
        yield return null;
    }
}
