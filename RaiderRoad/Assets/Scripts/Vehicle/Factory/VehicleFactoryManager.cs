using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class VehicleFactoryManager : MonoBehaviour {

	public enum vehicleTypes { light, medium, heavy, _null };

	public VehicleFactoryL l;
	public VehicleFactoryM m;
	public VehicleFactoryH h;

    public vehicleTypes getType;

	public void ConstructVehicle(vehicleTypes type, int mod) {
		if (type == vehicleTypes.light)
			l.AssembleVehicle(mod);
		else if (type == vehicleTypes.medium)
			m.AssembleVehicle(mod);
		else
			h.AssembleVehicle(mod);
	}

    public GameObject newConstructVehicle(vehicleTypes type, int mod)
    {
        if (type == vehicleTypes.light)
            return l.AssembleVehicle(mod);
        else if (type == vehicleTypes.medium)
            return m.AssembleVehicle(mod);
        else
            return h.AssembleVehicle(mod);
    }
}
