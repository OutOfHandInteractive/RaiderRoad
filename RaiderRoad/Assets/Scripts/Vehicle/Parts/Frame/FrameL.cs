using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameL : Frame {

	public List<GameObject> wheelNodes;

	public FrameL() {
		numWheels = 2;
	}

	// ---------- Getters and Setters ----------
	public int getNumWheels() {
		return numWheels;
	}
}
