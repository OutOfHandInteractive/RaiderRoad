using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class VehicleFactoryManager : MonoBehaviour {

	public enum vehicleTypes { light, medium, heavy, _null };

	public VehicleFactoryL l;
	public VehicleFactoryM m;
	public VehicleFactoryH h;

    public vehicleTypes getType;

    public GameObject newConstructVehicle(vehicleTypes type, int mod, Vector3 position)
    {
        if (type == vehicleTypes.light)
            return l.AssembleVehicle(mod, position);
        else if (type == vehicleTypes.medium)
            return m.AssembleVehicle(mod, position);
        else
            return h.AssembleVehicle(mod, position);
    }
}
