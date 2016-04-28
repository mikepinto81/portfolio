using UnityEngine;
using System.Collections;
using System;

public class GameMode_SpaceShipBridge : GameMode_Base {

    public override void SetupAwake()
    {
       //
    }

    public override IEnumerator EnterMode()
    {
        
        //Set the 3D cam to project to the viewscreen rendertexture
        SE_CameraControl.i.Set3DCamToViewScreen();
        
        //switch camera to show the bridge area
        SE_CameraControl.i.Center2DCamOnBridge();           
       
        //Turn on Bridge controls
        SE_InputManager.i.TurnOnBridgeController();
        
        //Turn on Overlay UI for the bridge
        SE_GameManager.i.bridgeManager.TurnOnBridgeOverlayButtons();
        
        yield return null;
    }

    public override IEnumerator ExitMode()
    {
        //turn off ui overlay for bridge
        SE_GameManager.i.bridgeManager.TurnOffBridgeOverlayButtons();
        
        //turn off bridge controls
        SE_InputManager.i.TurnOffBridgeController();
        
        yield return null;        
    }
}
