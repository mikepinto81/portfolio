//Mp3 player app object
function ZotnipMp3Player(elementID, playlistJSONString)
{
	/*
	//Json playlist format
	{
		"playlistItems": [
			{"Name":"Song1","url":"song1.mp3"},
			{"Name":"Song2","url":"song2.mp3"},
			{"Name":"Song3","url":"song3.mp3"},
			{"Name":"Song4","url":"song4.mp3"}
		]
	}
	*/
				
	//ref to this object
	var parent = this;
			
	//parse json string
	this.mp3PlayList = JSON.parse(playlistJSONString);				
	
	//get the html element of the mp3 player
	this.playerElement = document.getElementById(elementID);
	
	//is showing playlist or not
	this.showingPlaylist = true;
	
	//create html elements
	CreateHTMLElements();
	
	//currently playing track
	this.currentlyPlaying = "";
	
	//create html elements
	CreatePlayListElements();			
	
	//Audio object
	this.audio = null;
	
	//buttons
	this.playBtn = this.playerElement.getElementsByClassName("playbtn")[0];
	this.stopBtn = this.playerElement.getElementsByClassName("stopbtn")[0];
	this.prevBtn = this.playerElement.getElementsByClassName("prevbtn")[0];
	this.nextBtn = this.playerElement.getElementsByClassName("nextbtn")[0];
	this.pauseBtn = this.playerElement.getElementsByClassName("pausebtn")[0];
	
	//connect buttons
	//Play button
	this.playBtn.onclick = function Push(){parent.PlayButtonPush();}
	
	//Stop button
	this.stopBtn.onclick = function Push(){parent.Stop();}
	
	//Pause button
	this.pauseBtn.onclick = function Push(){parent.Pause();}
	
	//next button
	this.nextBtn.onclick = function Push(){parent.Next();}
	
	//prev button
	this.prevBtn.onclick = function Push(){parent.Prev();}
				
	//create sortable list for playlist (move this into a playlist object so we don't need a playlist)			
	this.sortablePlaylist = Sortable.create(document.getElementById(elementID+"_playlist"), {				
		handle: ".plyrDragHandle",
		animation: 150,				
		onEnd: function (evt) {									
			parent.SwapListItems(evt.oldIndex, evt.newIndex);
		},
	});	
	
	
	
	//***************************
	//Functions			
	
	//creates default html elements inside the provided html Element
	function CreateHTMLElements()
	{
		var htmlstring = ''+
		'<div class="mp3PlayerInnerWrap playlistshown">'+
		'<div class="currentlyPlaying">Now Playing: '+
			'</div><!--currentlyPlaying-->'+					
			'<div class="controls">'+
				'<li><button class="mp3playerBtn playbtn btn btn-default"><i class="fa fa-play"></i> Play</button></li>'+
				'<li><button class="mp3playerBtn pausebtn btn btn-default"><i class="fa fa-pause"></i> Pause</button></li>'+
				'<li><button class="mp3playerBtn stopbtn btn btn-default"><i class="fa fa-stop"></i> Stop</button></li>'+
				'<li><button class="mp3playerBtn prevbtn btn btn-default"><i class="fa fa-step-backward"></i> Prev</button></li>'+
				'<li><button class="mp3playerBtn nextbtn btn btn-default"><i class="fa fa-step-forward"></i> Next</button></li>'+				
				'<li><i class="nowPlayingIcon fa fa-circle-o-notch fa-spin fa-2x fa-fw"></i></li>'+
			'</div><!--controls-->'+
			'<div class="hidePlaylistToggle">Hide/Show Playlist</div>'+
			'<ul id="player_playlist" class="playlist block__list">'+
				'<li mediaurl=""><div class="plyrDragHandle"><i class="fa fa-bars"></i></div><div class="playlistTitle">Song1</div></li>'+ 
				'<li mediaurl=""><div class="plyrDragHandle"><i class="fa fa-bars"></i></div><div class="playlistTitle">Song2</div></li>'+
				'<li mediaurl=""><div class="plyrDragHandle"><i class="fa fa-bars"></i></div><div class="playlistTitle">Song3</div></li>'+
				'<li mediaurl=""><div class="plyrDragHandle"><i class="fa fa-bars"></i></div><div class="playlistTitle">Song4</div></li>'+
				'<li mediaurl=""><div class="plyrDragHandle"><i class="fa fa-bars"></i></div><div class="playlistTitle">Song5</div></li>'+
			'</ul><!--.playlist-->'+
			'</div><!--mp3PlayerInnerWrap-->';
			parent.playerElement.innerHTML = htmlstring;
	}
	
	//swaps internal playlist items by index
	this.SwapListItems = function(item1, item2)
	{				
		var temp = parent.mp3PlayList.playlistItems[item2];
		parent.mp3PlayList.playlistItems[item2] = parent.mp3PlayList.playlistItems[item1];
		parent.mp3PlayList.playlistItems[item1] = temp;				
	}
	
	//Get playlist item by name from playlist
	this.GetPlayListItemByName = function(name)
	{
		parent.mp3PlayList.playlistItems.forEach(function(item,index){
			if(item.Name == name)
			 return item;
		});
		
		return null;
	}
	
	//Get Playlist item by index
	this.GetPlayListItemByIndex = function(index)
	{		
		if(index > parent.mp3PlayList.playlistItems.length - 1 || index < 0)
			return null;
		else
			return parent.mp3PlayList.playlistItems[index];
	}
	
	//Get Playlist item by name
	this.GetIndexOfItemByName = function(name)
	{
		for(i = 0; i < parent.mp3PlayList.playlistItems.length; i++)
		{
			if(parent.mp3PlayList.playlistItems[i].Name == name)
				return i;
		}
		
		return -1;
	}
	
	//Creates HTML elements for playlist items
	function CreatePlayListElements()
	{			
		//build play list html string
		var newInnerHtml = "";	
		
		//use list on player object to build html elements
		parent.mp3PlayList.playlistItems.forEach(function(item,index){				
			newInnerHtml += "<li class='singlePlyListItem' songName='"+item.Name+"' mediaurl='"+item.url+"'><div class='plyrDragHandle'>::</div><div class='playlistTitle'>"+item.Name+"</div></li>";
		});
		
		//apply play list html string to div innerHTML
		var playListEl = parent.playerElement.getElementsByClassName("playlist")[0];
		playListEl.innerHTML = newInnerHtml;
		
		//add play functionality to each item
		var itemElements = playListEl.getElementsByClassName("singlePlyListItem");
		[].forEach.call(itemElements,function(item,index){
			item.onclick = function PlayItem(){parent.Play(item.getAttribute("mediaurl"), item.getAttribute("songName"));}
		});			
		
		//link show hide button
		var toggleElem = parent.playerElement.getElementsByClassName("hidePlaylistToggle")[0];		
		if(typeof jQuery == 'undefined')
		{	
			//no jquery so just hide show
			toggleElem.onclick = function Toggle(){
				if(!parent.showingPlaylist){
					playListEl.style.display = "inherit";					
					parent.showingPlaylist = true;
				  }
				  else{
					playListEl.style.display = "none";					
					parent.showingPlaylist = false;
				  }
			};
		} else {
			//use jquery
			toggleElem.onclick = function Toggle(){
				if(parent.showingPlaylist)
					parent.HidePlayList();
				else
					parent.ShowPlaylist();				
			};
		}
	}	
	
	//Play a track
	this.Play = function(url, trackName)
	{				
		if(trackName == "")
			return;
			
		//always stop currently playing audio since we create a new audio object so 
		//that the current one is garbage collected
		if(parent.audio != null)
			parent.Stop();
		
		parent.currentlyPlaying = trackName;
		parent.audio = new Audio(url);
		parent.audio.play();
		
		parent.UpdateNowPlaying();
	}
	
	//play an item from the play list
	this.PlayPlayListItem = function(item)
	{
		parent.Play(item.url,item.Name);
	}
	
	//when pushing play button 
	this.PlayButtonPush = function()
	{
		//if paused resume play
		if(parent.audio != null && parent.audio.paused)
		{
			parent.audio.play();
			return;
		}
		
		//check if already playing
		if(parent.currentlyPlaying != "" && !parent.audio.paused)
			return;
		
		//get item to play
		var playItem;
		
		if(parent.currentlyPlaying == "") 
		{		
			//no item has been played yet so grab first item in playlist
			playItem = this.GetPlayListItemByIndex(0);
		} else {
			//player stopped so replay last played		
			playItem = this.GetPlayListItemByName(parent.currentlyPlaying); 		
		}
		
		if(playItem == null)
				return;
				
		parent.PlayPlayListItem(playItem);
	}
	
	
	
	
}

//show playlist
ZotnipMp3Player.prototype.ShowPlaylist = function()
{
	this.showingPlaylist = true;	
	jQuery("#"+this.playerElement.id + " .playlist" ).show("fast");
	jQuery("#"+this.playerElement.id).addClass("playlistShowing");
	this.OnPlayListToggled(true);
}

//hide playlist
ZotnipMp3Player.prototype.HidePlayList = function()
{
	this.showingPlaylist = false;
	jQuery("#"+this.playerElement.id + " .playlist" ).hide("fast");
	jQuery("#"+this.playerElement.id).removeClass("playlistShowing");
	this.OnPlayListToggled(false);
}

//event for when playlist is toggled into view
ZotnipMp3Player.prototype.OnPlayListToggled = function(state)
{	
	var evt = new CustomEvent('playlisttoggled',{detail: state});
	window.dispatchEvent(evt);
}

//will update the text for the now playing HTML element
ZotnipMp3Player.prototype.UpdateNowPlaying = function()
{
	this.playerElement.getElementsByClassName("currentlyPlaying")[0].innerHTML = "Now Playing: " + this.currentlyPlaying;
	if(this.currentlyPlaying == null || this.currentlyPlaying == "")
	{
		jQuery("#"+this.playerElement.id).removeClass("isPlaying");
	} else {
		jQuery("#"+this.playerElement.id).addClass("isPlaying");
	}
}

ZotnipMp3Player.prototype.Pause = function()
{
	this.audio.pause();
}
	
ZotnipMp3Player.prototype.Stop = function()
{		
	this.currentlyPlaying = "";
	this.UpdateNowPlaying();
	
	if(this.audio == null)
		return;
	
	//pause audio object first so it stops and it is garbage collected.
	this.audio.pause();	
	this.audio = null;
}

//Play next file in play list
ZotnipMp3Player.prototype.Next = function()
{
	//Play first item if no items currently playing.
	if(this.currentlyPlaying == "")
	{
		this.PlayPlayListItem(this.mp3PlayList.playlistItems[0]);
		return;
	}
	
	//get current index
	var currentIndex = this.GetIndexOfItemByName(this.currentlyPlaying);
	
	//check if on last song in list
	if(currentIndex == this.mp3PlayList.playlistItems.length - 1)
		currentIndex = -1;
	
	//Play next item
	this.PlayPlayListItem(this.mp3PlayList.playlistItems[currentIndex + 1]);	
}

//Play previous file in play list
ZotnipMp3Player.prototype.Prev = function()
{
	//Play last item if no items currently playing.
	if(this.currentlyPlaying == "")
	{
		this.PlayPlayListItem(this.mp3PlayList.playlistItems[this.mp3PlayList.playlistItems.length - 1]);
		return;
	}
	
	//get current index
	var currentIndex = this.GetIndexOfItemByName(this.currentlyPlaying);
	
	//check if on last song in list
	if(currentIndex == 0)
		currentIndex = this.mp3PlayList.playlistItems.length;
	
	//Play next item
	this.PlayPlayListItem(this.mp3PlayList.playlistItems[currentIndex - 1]);	
}

