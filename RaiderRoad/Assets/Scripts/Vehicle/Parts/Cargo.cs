﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Cargo : DestructiblePart {

	public GameObject payloadNode;

	protected abstract override float GetMaxHealth();
}
