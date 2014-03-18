using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Platform : MonoBehaviour 
{	
	public List<WheelPair> wheelPairs;
	public List<List<WheelPair>> wheelLines;
	
	public float steerCoeff = 1;

	//используется для работы с WheelCollider
	public bool isReversed;
	
	void Awake ()
	{
		wheelPairs = new List<WheelPair>(transform.GetComponentsInChildren<WheelPair>());
		wheelPairs.ForEach(a => a.platform = this);
		
		wheelLines = new List<List<WheelPair>>();
		List<WheelPair> tmp = new List<WheelPair>(wheelPairs);
		if (Mathf.Approximately(transform.localRotation.eulerAngles.y, 180))
		{
			tmp = tmp.OrderByDescending(a => a.transform.localPosition.y).ToList();
		}
		else
		{
			tmp = tmp.OrderBy(a => a.transform.localPosition.y).ToList();
		}
		int curLineInd = 0;
		float curY = tmp[0].transform.localPosition.y;
		foreach (WheelPair curPair in tmp)
		{
			if (Mathf.Abs(curPair.transform.localPosition.y - curY) > 0.05f)
			{
				curLineInd++;
				curY = curPair.transform.localPosition.y;
			}
			
			if (wheelLines.Count - 1 < curLineInd)
			{
				wheelLines.Add(new List<WheelPair>());
			}
			wheelLines[curLineInd].Add(curPair);
			
			wheelLines[curLineInd] = wheelLines[curLineInd].OrderBy(a => transform.parent.InverseTransformPoint(transform.TransformPoint(a.transform.localPosition)).x).ToList();
		}
	}
}
