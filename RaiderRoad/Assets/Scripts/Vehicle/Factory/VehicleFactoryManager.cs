using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class VehicleFactoryManager : MonoBehaviour {

	public enum vehicleTypes { light, medium, heavy, _null };

	public VehicleFactoryL l;
	public VehicleFactoryM m;
	public VehicleFactoryH h;

    public vehicleTypes getType;

	public void ConstructVehicle(vehicleTypes type) {
		if (type == vehicleTypes.light)
			l.AssembleVehicle();
		else if (type == vehicleTypes.medium)
			m.AssembleVehicle();
		else
			h.AssembleVehicle();
	}

    public GameObject newConstructVehicle(vehicleTypes type)
    {
        if (type == vehicleTypes.light)
            return l.AssembleVehicle();
        else if (type == vehicleTypes.medium)
            return m.AssembleVehicle();
        else
            return h.AssembleVehicle();
    }
}
