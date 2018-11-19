using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class VehicleFactoryManager : MonoBehaviour {

	public enum vehicleTypes { light, medium, heavy };

	public VehicleFactoryL l;
	public VehicleFactoryM m;
	public VehicleFactoryH h;

    void Start()
    {
        l = gameObject.AddComponent<VehicleFactoryL>() as VehicleFactoryL;
        m = gameObject.AddComponent<VehicleFactoryM>() as VehicleFactoryM;
        h = gameObject.AddComponent<VehicleFactoryH>() as VehicleFactoryH;
    }

	public void ConstructVehicle(vehicleTypes type) {
		if (type == vehicleTypes.light)
			l.AssembleVehicle();
		else if (type == vehicleTypes.medium)
			m.AssembleVehicle();
		else
			h.AssembleVehicle();
	}
}
