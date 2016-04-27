using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Zotnip {
	public class ZT_SpriteAnimator : MonoBehaviour {
		
		//when unity calls the awake method initialize this sprite or not.  If uninitialized, sprite will remain in state it was in when instantiated or was in the scene inspector.
		[SerializeField]
		bool initializeOnAwake = false;
		public void SetInitializeOnAwake(bool initialize) {initializeOnAwake = initialize;}
		
		bool initialized = false;
		
		//adds this ani to the sprite ani manager for quick retrieval by name.  Ani is added in initialization not awake unless set to initialize on awake.
		[SerializeField]
		bool addToMasterList = true;
		public void SetAddToMasterList(bool set) {addToMasterList = set;}
		
		//Use Unity Ui image instead of Sprite class
		[SerializeField]
		bool useUnityUiImage = false;
		public void SetUseUnityUiImage(bool use) {useUnityUiImage = use;} 
		
		//the name of animation to play on start, if blank, no animation will play.  This is mainly used in the Unity scene inspector.
		[SerializeField]
		string playAnimationOnStart = "";
		public void SetPlayAniOnStart(string aniName){playAnimationOnStart = aniName;}
		
		//The animation will play the group based off of the angle given to it.  (Right, Left, Up, Down, etc..)
		[SerializeField]
		float defaultAngle = -1;
		public void SetDefaultAngle(float angle){defaultAngle = angle;}
		
		//Animations available to play from
		Dictionary<string, ZT_SpriteAnimation> animations = new Dictionary<string, ZT_SpriteAnimation>();
		public ZT_SpriteAnimation getAni(string aniName){
			if(!animations.ContainsKey(aniName))
				return null;
			else
				return animations[aniName];
		}
		
		//Set this array with animations from the Unity IUnspector and they will be filled into the animations to be played when initialized.
		[SerializeField]
		ZT_SpriteAnimation[] loadAnimations = new ZT_SpriteAnimation[0];
		
		//currently playing animation.
		public ZT_SpriteAnimation currentAni {get; private set;}
		
		//If not set in the inspector, will try to auto get the sprite or image component in initialization
		[SerializeField]
		ZT_Sprite sp;
		[SerializeField]
		Image uiImage;

		//directory in Resources folder where animations should auto load from.
		[SerializeField]
		string ResourcesDir = "";
		public void SetResourcesDirectory(string dir){ResourcesDir = dir;}
		
		//Event for when this Animator is playing an animation and the frame has changed
		public delegate void AnimationDelegate(ZT_SpriteAnimation.DirectionGroup.AnimationFrame frame);
		public event AnimationDelegate FrameChanged;


/**
Editor
**/
	#if UNITY_EDITOR
		//this is useful for figuring out good animation speeds with walking speeds
		//speed set here will not be saved to the animation		
		public bool overrideCurrentAniSpeedInEditor = false;
		public float overrideAniSpeedInEditor = 0;

		void Update()
		{
			if(overrideCurrentAniSpeedInEditor && currentAni != null)
				currentAni.SetSpeed(overrideAniSpeedInEditor);
		}
	#endif
	
	
	/**
	Static Creation
	**/
	
	/// <summary>
	/// warning a sprite gameobject must already exist
	/// </summary>
	public static ZT_SpriteAnimator CreateSpriteAnimator(GameObject spriteGameObject, string overrideAnimName, bool setToInitializeOnLoad)
	{
		ZT_SpriteAnimator returnAnimObj;

		if(!Application.isEditor){
			returnAnimObj = ZT_SpriteMaster.GetSpriteAnim(spriteGameObject.name);
			if(returnAnimObj != null)
				return returnAnimObj;
		}

		returnAnimObj = spriteGameObject.AddComponent<ZT_SpriteAnimator>();			
		returnAnimObj.initializeOnAwake = setToInitializeOnLoad;

		if(!Application.isEditor && setToInitializeOnLoad)
			returnAnimObj.Initialize();

		return returnAnimObj;
	}
	
	
	
	
	/*****
	Initializatioon and Deletion
	*****/
		void Awake()
		{
			if(initializeOnAwake)
				Initialize();
		}

		void Start()
		{
			if(playAnimationOnStart != "" && initialized){
				PlayClip(playAnimationOnStart,defaultAngle);
			}
		}

		void OnEnable()
		{
			//make sure each animation will play from scratch.  Putting this in OnEnable allows us to make sure
			//animations are reset if the component or GameObject was disabled and reenabled.
			foreach(ZT_SpriteAnimation ani in animations.Values)
				ani.Reset();
		}

		void OnDisable()
		{			
			foreach(ZT_SpriteAnimation ani in animations.Values)
				ani.Stop();
		}

	    public void Initialize()
		{	
			if(addToMasterList)
				ZT_SpriteMaster.AddSpriteAnim(this);
						
			//Set Sprite Component						
			if(useUnityUiImage && uiImage == null) 
				uiImage = GetComponent<Image>();
			else if(sp == null)
				sp = GetComponent<ZT_Sprite>();
			
			//if no animations are on this object try to load from resources
			if(loadAnimations == null || loadAnimations.Length == 0){				
				loadAnimations = Resources.LoadAll<ZT_SpriteAnimation>(ResourcesDir);
			}
			
			if(loadAnimations == null)
				return;
			
			//load each animation. We use the loadAnimations array so we can set in the inspector
			foreach(ZT_SpriteAnimation anim in loadAnimations){
				LoadAnimation(anim);
			}

			//release loadanimations array from memory
			System.Array.Clear(loadAnimations,0,loadAnimations.Length);
			loadAnimations = null;
			
			initialized = true;
		}

		void OnDestroy()
		{
			ClearAnimations();
			
			if(addToMasterList && ZT_SpriteMaster.instance != null)
				ZT_SpriteMaster.RemoveSpriteAnim(name);
		}



		/**
		Removing Animations
		**/
		
		//must first unsub from each anis framechange event 
		public void ClearAnimations()
		{
			//temp list of ani names to be removed
			List<string> aniNames = new List<string>();
			
			//first fill aniNames list and also unsub from frame change events
			foreach(ZT_SpriteAnimation ani in animations.Values)
			{
				ani.FrameChanged -= FrameChange;
				aniNames.Add(ani.name);				
			}
			
			foreach(string aniName in aniNames)
				RemoveAnimation(aniName);
				
			animations.Clear();
			
		}		
		
		public void ClearCurrentAni()
		{
			Stop();
			//if streaming ani unload its current frame
			if(currentAni != null && currentAni.useResourcesLoad)
				currentAni.UnloadResourcesCurrentFrame();
			currentAni = null;
		}
		
		public void RemoveAnimation(string aniName)
		{
			if(animations.ContainsKey(aniName)){
				//if streaming ani, unload its current loaded frame if one is loaded
				if(animations[aniName].useResourcesLoad)
					animations[aniName].UnloadResourcesCurrentFrame();
				animations.Remove(aniName);
			}
		}


		

		/**
		Loading Animations
		**/
		public void LoadAnimation(ZT_SpriteAnimation anim)
		{
			LoadAnimation(anim,anim.name);
		}

		public void LoadAnimation(ZT_SpriteAnimation anim, string customName)
		{
			if(animations.ContainsKey(customName))
				return;
			
			ZT_SpriteAnimation newAni = ScriptableObject.CreateInstance<ZT_SpriteAnimation>();
			anim.Copy(newAni);
			newAni.name = customName;
			newAni.Setup(sp,ZTTimeManager.MainTimer(),this);
			newAni.SetUseUnityUiImage(useUnityUiImage);
			animations.Add(customName,newAni);
			newAni.FrameChanged += FrameChange;
		}


		
		/**
		Playing and Stop Sprite Animations
		**/
		
		void FrameChange(ZT_SpriteAnimation.DirectionGroup.AnimationFrame frame)
		{
			if(FrameChanged != null)
				FrameChanged(frame);
		}

		/// <summary>
		/// Changes the current ani. but does NOT play it.
		/// </summary>
		public void ChangeCurrentAni(string clipName) 
		{
			if(animations.ContainsKey(clipName)){
				currentAni = animations[clipName];
			}
		}

		

		//plays an animation that is on this animator and creates a coroutine that can be yielded to
		public IEnumerator PlayClipYield(string clipName, float angle)
		{
			if(!animations.ContainsKey(clipName)){
				yield break;
			}

			//if same direction is already playing break out of coroutine
			if(currentAni != null && currentAni.name == clipName && currentAni.isPlaying)
			{				
				if(currentAni.getCurrentDirGroup.direction == FaceDirection.GetDirectionFromAngle(angle,currentAni.use8wayDir))
					yield break;
			} 

			//stop whatever clip was already playing
			if(currentAni != null && currentAni.isPlaying && currentAni != animations[clipName]) 
				currentAni.Stop();

			currentAni = animations[clipName];

			yield return StartCoroutine(currentAni.PlayAnimationYield(angle));

		}

		public void PlayClip(string clipName, float angle)
		{
			StartCoroutine(PlayClipYield(clipName,angle));
		}

		//plays currently loaded currentani and specific angle
		public void Play(float angle)
		{
			if(currentAni != null)
				currentAni.PlayAnimation(angle);
		}
		
		public void Stop()
		{
			if(currentAni != null){
				currentAni.Stop();
			}
		}



	}
}