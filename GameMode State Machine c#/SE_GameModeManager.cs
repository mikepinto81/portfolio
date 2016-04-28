using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SE_GameModeManager : MonoBehaviour {
    
    //Singleton for the manager
    static SE_GameModeManager _i;
    public static SE_GameModeManager i {
        get {
            return _i;
        }
    }
    
    //Enum contains all available modes.
	public enum Mode { None, MainMenu, SpaceShipBridge, LandMode}
    
    //static current mode since this class is a singleton
    static Mode _CurrentMode = Mode.None;
    public static Mode CurrentMode { get { return _CurrentMode; } }

    //Mode changing Events and delegate function
    public delegate void ModeChange(Mode oldMode, Mode newMode);
    public event ModeChange ChangedMode;
    public event ModeChange ChangingModeStarted;

    //allows a mode change to pause since a mode change is a coroutine.
    bool delaychangingMode = false;
    
    //Are we currently changing modes
    bool _changingMode = false;
    public bool changingMode { get { return _changingMode; } }
   
    //available GameMode objects
    Dictionary<string, GameMode_Base> gameModes = new Dictionary<string, GameMode_Base>();
    
    GameMode_Base GetMode(string id)
    {
	if(!gameModes.ContainsKey(id))
            return null;
        else
            return gameModes[id];
    }

    /**
    *   Initialization
    **/
    void Awake()
    {
        //set the singleton accessor
        _i = this;

        //add game modes to list
        gameModes.Add("LandMode", new GameMode_LandMode());
        gameModes.Add("SpaceShipBridge", new GameMode_SpaceShipBridge());
        gameModes.Add("MainMenu", new GameMode_MainMenu());
        
        //Run each modes Awake method
        foreach (GameMode_Base mode in gameModes.Values)
            mode.SetupAwake();
    }

    void Start()
    {
        //run each modes Start method
        foreach (GameMode_Base mode in gameModes.Values)
            mode.SetupStart();
    }

    void OnDestroy()
    {
        //remove singleton accessor
        _i = null;
    }
    
   /***
   * Access Methods
   ***/
   public void SwitchMode(Mode newMode)
    {
        if (CurrentMode == newMode)
            return;
            
        StartCoroutine(ChangingMode(newMode));
    }

    //during the exit phase of the previous mode this will pause the mode change.    
    public void DelayChangingMode()
    {
        delaychangingMode = true;
    }

    IEnumerator ChangingMode(Mode newMode)
    {
        _changingMode = true;

        SE_FadeOut.i.FadeOut();
        while (SE_FadeOut.i.fading)
            yield return null;

        //set off event saying we started a mode change
        if (ChangingModeStarted != null)
            ChangingModeStarted(_CurrentMode, newMode);

        //exit old mode
        GameMode_Base tempMode = GetMode(_CurrentMode.ToString());
        if (tempMode != null)
            yield return StartCoroutine(tempMode.ExitMode());

        //allow to delay changing
        while (delaychangingMode)
            yield return null;

        //enter new mode
        tempMode = GetMode(newMode.ToString());
        if (tempMode != null)
            yield return StartCoroutine(tempMode.EnterMode());

        //event saying we finished changing modes
        if (ChangedMode != null)
            ChangedMode(_CurrentMode, newMode);

        _CurrentMode = newMode;   

        _changingMode = false;  
    }
}
