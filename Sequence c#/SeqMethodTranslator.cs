using System.Collections;
using System.Collections.Generic;
using Zotnip;
using UnityEngine;
using System.IO;
using System;

/***
Note the name Method here technically refers to a class that runs a predefined set of events based on strings stored in XML
***/
namespace Zotnip {

	public static class SeqMethodTranslator {

		//Convert seq node information into usable methods
		//the Method class takes a seqitem and also a reference to the sequence that it belongs to

		//available data types
		public static string[] valueTypes = new string[]{"none","string","int","float","vector3","vector2"};

		//a dictionary of all possible methods that can be run.
		//The xml passes along a method by string and we choose the actual method to run here.
		static Dictionary<string,Method> MethodDictionary = new Dictionary<string, Method>();
		public static Dictionary<string,Method> getMethodDictionary {
			get{
				return MethodDictionary;
			}
		}

		//so we don't rerun the setup since this is a static class it will always live
		public static bool oneTimeSetup = false;

		//sets up the method dictionary with string names....could be loaded from a text file or wherever
		public static void Setup()
		{
			#if UNITY_EDITOR
				//REMOVED FOR BREVITY OF EXAMPLE
			#else
				if(oneTimeSetup) 
					return;
			#endif

			//make sure we start fresh.
			if(MethodDictionary != null)
				MethodDictionary.Clear();
			
			//load available method names from resources	
			TextAsset textFile = Resources.Load("MethodNames") as TextAsset;
			
			//split the string by , and remove blank spaces then add to array and add to the available methods dictionary
			string[] defaultMethodNames = textFile.text.Replace(" ","").Split(',');
			foreach(string methodName in defaultMethodNames)
			{
				AddMethod(methodName);
			}

			//add user added method names
			textFile = Resources.Load("UserMethodNames") as TextAsset;
			defaultMethodNames = textFile.text.Replace(" ","").Split(',');
			if(textFile != null)
			{	
				foreach(string methodName in defaultMethodNames)
				{
					AddMethod(methodName);
				}
			}
			#if UNITY_EDITOR
						
			#else
					oneTimeSetup = true;
			#endif
		}

		//Add instance of given methodname class to dictionary using reflection from a string
		public static void AddMethod(string methodName)
		{
			if(methodName == "")
				return;
			
			//NOTE** should make sure the Activator class is accessable in all Unity build types
			//first get the type
			var type = System.Type.GetType("Zotnip."+methodName);
			if(type == null)
				return;
				
			//create instance of object from found type
			var myObject = (Method)System.Activator.CreateInstance(type);
			
			//add to available methods dictionary
			MethodDictionary.Add(methodName,myObject as Method);
		}

		//Identifies what type of method this is by using the first node which MUST be the type node
		static public IEnumerator RunMethod(Sequence.SequenceItem seqItem, Sequence sequence)
		{
			//string nodeType = seqItem.itemNodes[0].nodeInnerText;
			if(!sequence.sequenceRunning || seqItem.skip)
				yield break;
			
			//create a coroutine and yield while it runs
			sequence.methodJob = new Job(getMethodDictionary[seqItem.type].RunMethod(seqItem,sequence),true);
			while(sequence.methodJob.running)
				yield return null;
		}
		
		

		//Base abstract class for all Method types.  A Method object provides an action or event that will happen and is 
		//triggered by Class name from XML Sequences.  For example.  In a sequence the game needs to fade out.  There would be a Fadeout Method that 
		//has a time field option, and when it is run and sent the xml data, it will trigger a fadeout over the given time.
		public abstract class Method
		{			
			public abstract IEnumerator RunMethod(Sequence.SequenceItem seqItem, Sequence sequence);

			//a Node is one piece of data...float, int, bool, string, etc...
			public Sequence.SequenceItem.SeqNode GetNode(int index,Sequence.SequenceItem sequenceItem)
			{
				if(index < sequenceItem.itemNodes.Count)
					return sequenceItem.itemNodes[index];
				else{
					Debug.Log("Error in sequence type: " + sequenceItem.type);
					return new Sequence.SequenceItem.SeqNode("","","","","string");
				}
			}

			//get the node by node name instead of index since index could change it isn't always wise to use index.
			public Sequence.SequenceItem.SeqNode GetNode(string typeName, Sequence.SequenceItem sequenceItem)
			{
				foreach(Sequence.SequenceItem.SeqNode node in sequenceItem.itemNodes)
				{
					if(node.nodeType == typeName)
						return node;
				}

				return null;
			}

			public bool nodeExists(int index, Sequence.SequenceItem seqItem)
			{
				if(index < seqItem.itemNodes.Count)
					return true;
				else
					return false;
			}
		}

	}


}