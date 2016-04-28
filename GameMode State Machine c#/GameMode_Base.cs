using UnityEngine;
using System.Collections;


public abstract class GameMode_Base {
    
    //Called prior to SetupStart.
    public abstract void SetupAwake();
            
    public abstract IEnumerator EnterMode();
    public abstract IEnumerator ExitMode();

    public virtual void SetupStart() { }

    //will be run every update frame while this mode is active
    public virtual void ModeUpdate() { }

    
	
}
