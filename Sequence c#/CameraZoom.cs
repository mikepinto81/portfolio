using UnityEngine;
using System.Collections;

//Sequence Method that Zooms the Camera by amount over time.
namespace Zotnip {
	public class CameraZoom : SeqMethodTranslator.Method {

		public override IEnumerator RunMethod (Sequence.SequenceItem seqItem, Sequence sequence)
		{			
			string cameraName = GetNode(5,seqItem).nodeInnerText;
						
			ZT_Camera cam = ZT_CameraController.i.GetCamera(cameraName);
			if(cam == null)
				yield break;
			
			bool zoomCam;
			float zoomTo;
			bool resetZoom;
			float zoomTime;

			zoomCam = GetNode(1,seqItem).boolData;
			zoomTo = GetNode(2,seqItem).floatData;
			resetZoom = GetNode(3,seqItem).boolData;
			zoomTime = GetNode(4,seqItem).floatData;
			
			if(zoomCam){
				if(resetZoom){
					cam.ResetZoom(zoomTime);
				} else {
					cam.Zoom(zoomTo,zoomTime);
				}
			}

			yield return null;
		}
	
	}
}