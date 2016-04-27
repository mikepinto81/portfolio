using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

	namespace Zotnip {
	[System.Serializable]
	public class ZT_SpriteAnimation : ScriptableObject{
				
		[SerializeField]
		float speed = 1;
		
		//temp store the original speed to reset after temp increasing speed.
		float originalSpeed = 1;
		
		[SerializeField]
		bool loop = true;
		[SerializeField]
		bool pingPong = false;
		
		//use unity Ui Image class instead of Sprite class
		[SerializeField]
		bool useUnityUiImage = false;
		public void SetUseUnityUiImage (bool set){useUnityUiImage = set;}
		
		//image will auto flip if facing left and play the right side animation
		[SerializeField]
		bool useFlip = false;
		
		//is set to true to cancel out of an animation		
		bool interruptCurrentFrame = false;
		
		[SerializeField]
		bool playReverse = false;

		//will stream animations from resource directory
		[SerializeField]
		bool useResourcesLoad = false;
		
		//Frame groups contain frames of sprites.  Groups could be directions (up, down, left, right, etc)
		[SerializeField]
		List<DirectionGroup> frameGroups = new List<DirectionGroup>();
		
		DirectionGroup currentGroup;
		public DirectionGroup getCurrentDirGroup {get{return currentGroup;}}
		
		//associated animator controlling this animation
		[SerializeField]
		ZT_SpriteAnimator ztAnimator;
		
		//sprite this animation will play out on
		ZT_Sprite sp;
		
		ZTTimeManager.ZTTime timeObject;

		int currentFrame = 0;
		public int getCurrentFrame{get{return currentFrame;}}

		//if 8 way, then there are diagonal directions. (upper left, upper right, etc...)
		[SerializeField]
		bool use8wayDir = false;
		
		//Frme change event.  Could be used to sync other animations or collision movements, etc..
		public delegate void AnimationDelegate(ZT_SpriteAnimation.DirectionGroup.AnimationFrame frame);
		public event AnimationDelegate FrameChanged;
		
		//copy this animation to another object
		public void Copy(ZT_SpriteAnimation toAnim)
		{
			//Note that useUnityImage needs to be set from the animator, we are not copying that value from another animation!
			toAnim.speed = speed;
			toAnim.loop = loop;
			toAnim.pingPong = pingPong;
			toAnim.useFlip = useFlip;
			toAnim.frameGroups = new List<DirectionGroup>();
			toAnim.useResourcesLoad = useResourcesLoad;
			toAnim.playReverse = playReverse;
			
			for(int i = 0; i < frameGroups.Count; i++)
			{
				toAnim.frameGroups.Add(new DirectionGroup());
				frameGroups[i].Copy(toAnim.frameGroups[i]);
			}

		}

		public void Setup(ZT_Sprite newZtSprite, ZTTimeManager.ZTTime timeCon,ZT_SpriteAnimator newAnimator)
		{
			sp = newZtSprite;
			timeObject = timeCon;
			ztAnimator = newAnimator;

			originalSpeed = speed;

			//run through each frame group and decide if we can show 8 directions or just 4
			int extraAngles = 0;
			foreach(DirectionGroup frameGroup in frameGroups)
			{
				switch(frameGroup.direction){
				case(FaceDirection.Direction.LowerLeft):
					extraAngles++;
					break;
				case(FaceDirection.Direction.LowerRight):
					extraAngles++;
					break;
				case(FaceDirection.Direction.UpperLeft):
					extraAngles++;
					break;
				case(FaceDirection.Direction.UpperRight):
					extraAngles++;
					break;
				}
			}

			if (extraAngles == 4)
				use8wayDir = true;
			else
				use8wayDir = false;

		}

		/**
		SPEED
		**/
		public void SetSpeed(float newSpeed)
		{
			speed = newSpeed;
		}

		public void RestoreOriginalSpeed()
		{
			speed = originalSpeed;
		}
		
		
		/******
		ANGLES
		*****/
		//gets the proper direction based on the given angle and returns the direction group
		DirectionGroup frameGroupFromAngle(float angle)
		{
			FaceDirection.Direction direction;
			
			if(angle < 0)
				direction = FaceDirection.Direction.None;
			else {
				direction = FaceDirection.GetDirectionFromAngle(angle,use8wayDir);
			}
			
			DirectionGroup groupToPlay = null;
			
			foreach(DirectionGroup frameGroup in frameGroups)
			{
				if(frameGroup.direction == direction){
					groupToPlay = frameGroup;
					break;
				}
			}

			return groupToPlay;
		}
		
		//feed this an angle and the current animation group (or direction) will switch to the appropriate group.
		void SetCurrentGroupFromAngle(float angle)
		{
			DirectionGroup newGroup = frameGroupFromAngle(angle);
			if(newGroup == null)
				newGroup = frameGroupFromAngle(-1);
			if(currentGroup != newGroup)
			{
				if(currentGroup != null)
					interruptCurrentFrame = true;
				onePassIndex = 0;
				currentGroup = newGroup;
			}
		}


		/**********************
		Playing controls
		****/
		bool playing = false;
		public bool isPlaying {get{return playing;}}

		public IEnumerator PlayAnimationYield(float angle)
		{
			SetCurrentGroupFromAngle(angle);
			yield return ztAnimator.StartCoroutine(PlayFrames());		
		}

		public IEnumerator PlayAnimationYield()
		{
			yield return ztAnimator.StartCoroutine(PlayAnimationYield(-1));
		}

		public void PlayAnimation(float angle)
		{
			ztAnimator.StartCoroutine(PlayAnimationYield(angle));
		}

		public void PlayAnimation()
		{
			ztAnimator.StartCoroutine(PlayAnimationYield());
		}


		/**
		Sprite Changing
		**/
		void SetSprite(int index)
		{			
			//if we are streaming from resources, switch methods
			if(useResourcesLoad){
				SetSpriteFromResource(index);
				return;
			}

			//change sprite from list of sprites in current group (check if Ui Image or Sprite class)
			if(!useUnityUiImage)
				sp.SetSprite(currentGroup.frames[index].sprite);
			else {
				ztAnimator.uiImage.sprite = currentGroup.frames[index].sprite;
			}
			
			//set off event
			if(FrameChanged != null)
				FrameChanged(currentGroup.frames[index]);
		}


		void SetSpriteFromResource(int index)
		{
			//var to hold the current sprite being used or to be set
			Sprite workingSp;

			//unload previous used sprite from memory
			UnloadResourcesCurrentFrame();
			
			//resources parent directory
			string parentDir = currentGroup.frames[index].parentDir;
//			if(parentDir != "")
//				parentDir = parentDir + "/";

			//load new sprite from resources
			workingSp = Resources.Load<Sprite>("StreamingAnimations/"+parentDir+name+"/"+currentGroup.frames[index].spriteName);
			
			//set the sprite that was loaded
			if(!useUnityUiImage){
				sp.SetSprite(workingSp);
			}
			else {
				ztAnimator.uiImage.sprite = workingSp;
			}
			
			//set off event
			if(FrameChanged != null)
				FrameChanged(currentGroup.frames[index]);
		}

		public void UnloadResourcesCurrentFrame()
		{
			//var to hold the current sprite being used or to be set
			Sprite workingSp;
			
			//unload previous used sprite so all references are lost
			if(!useUnityUiImage){
				workingSp = sp.getSpRenderer.sprite;
				sp.getSpRenderer.sprite = null;
			} else {
				workingSp = ztAnimator.uiImage.sprite;
				ztAnimator.uiImage.sprite = null;
			}
			
			//unload from memory the sprite since it is no longer used
			if(workingSp != null){
				Resources.UnloadAsset(workingSp.texture);
				Resources.UnloadAsset(workingSp);
			}
		}
		
		/****
		Frame changing
		****/
		
		//Sets the sprite to single frame and yields while it shows it
		bool playingFrame = false;
		IEnumerator PlayFrame(int index)
		{
			if(playingFrame)
				yield break;
				
			playingFrame = true;
			
			///get info about the frame
			DirectionGroup.AnimationFrame frameObj = currentGroup.frames[index];

			//set the sprite
			if(playing) 
				SetFrame(index);

			float timeToWait = timeObject.time + ((1/speed) * frameObj.frameLength);
			
			//wait the length of frame unless interrupted and then stop waiting
			while(timeObject.time <= timeToWait && playing && !interruptCurrentFrame)
				yield return null;		

			if(interruptCurrentFrame)
				interruptCurrentFrame = false;
				
			playingFrame = false;
		}

		void SetFrame(int frame)
		{
			SetSprite(frame);
			currentFrame = frame;
		}


		int onePassIndex = 0;
		//Play each frame in the current group through one time
		IEnumerator PlayOnePass()
		{
			//Play in reverse
			if(playReverse)
			{
				bool reverse = false;
				if(currentFrame == 0){
					if(pingPong)
						reverse = true;
					else
						currentFrame = currentGroup.frames.Count - 1;
				}
				if(!reverse){
					for(onePassIndex = currentFrame; onePassIndex > -1; onePassIndex--){
						if(!playing)
							yield break;
						yield return ztAnimator.StartCoroutine(PlayFrame(onePassIndex));
					}
					if(!loop)
						Stop();
				} else {
					for(onePassIndex = currentFrame; onePassIndex < currentGroup.frames.Count; onePassIndex++)
					{
						if(!playing)
							yield break;
						yield return ztAnimator.StartCoroutine(PlayFrame(onePassIndex));
					}

				}
			}
			
			//Play forward
			else {
				bool reverse = false;
				if(currentFrame >= currentGroup.frames.Count -1){
					if(pingPong)
						reverse = true;
					else
						currentFrame = 0;
				}
				if(reverse){
					for(onePassIndex = currentFrame; onePassIndex > -1; onePassIndex--){
						if(!playing)
							yield break;
						yield return ztAnimator.StartCoroutine(PlayFrame(onePassIndex));
					}
				} else {
					for(onePassIndex = currentFrame; onePassIndex < currentGroup.frames.Count; onePassIndex++)
					{
						if(!playing)
							yield break;
						yield return ztAnimator.StartCoroutine(PlayFrame(onePassIndex));
					}
					if(!loop)
						Stop();
				}
			}
		}
		
		//Can enter here if you want to yield to the play
		IEnumerator PlayFrames()
		{
			if(playing)
				yield break;
			playing = true;
			while(playing){
				yield return ztAnimator.StartCoroutine(PlayOnePass());
			}
			onePassIndex = 0;
		}

		public void Resume()
		{
			ztAnimator.StartCoroutine(PlayFrames());
		}

		public void Stop()
		{
			playing = false;
			onePassIndex = 0; //reset since we don't want to start in middle of a pass through a frame group
			playingFrame = false;
		}

		public void Reset()
		{
			playing = false;
			onePassIndex = 0;
			currentFrame = 0;
			interruptCurrentFrame = false;
			playingFrame = false;
		}

		public void SkipToFrame(int frame)
		{
			if(currentGroup == null)
				return;
			if(frame < currentGroup.frames.Count && frame > -1){
				onePassIndex = frame;
				currentFrame = frame;
				SetFrame(frame);
			}
		}


	}
}
