using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentL : Attachment {
	protected override float GetMaxHealth() {
		return Constants.VEHICLE_LIGHT_PART_BASE_HEALTH;
	}
}
