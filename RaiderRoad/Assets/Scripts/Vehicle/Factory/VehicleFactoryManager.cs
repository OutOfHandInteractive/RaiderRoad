using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleFactoryManager : MonoBehaviour {

	public enum vehicleTypes { light, medium, heavy, _null };

	#region variable declarations
	public VehicleFactoryL lVehicleFactory;
	public VehicleFactoryM mVehicleFactory;
	public VehicleFactoryH hVehicleFactory;
    public vehicleTypes getType;
    public GameObject RV;
    private rvHealth rvRef;
	#endregion

    void Start()
    {
        rvRef = RV.GetComponent<rvHealth>();
    }

	public GameObject NewConstructVehicle(vehicleTypes type, int mod, Vector3 position, float wChance)
    {
        int batteries = getBatteries();
        if (type == vehicleTypes.light)
            return lVehicleFactory.AssembleVehicle(mod, position, wChance, batteries);
        else if (type == vehicleTypes.medium)
            return mVehicleFactory.AssembleVehicle(mod, position, wChance, batteries);
        else
            return hVehicleFactory.AssembleVehicle(mod, position, wChance, batteries);
    }

    private int getBatteries(){
        return rvRef.getRemainingBatteries();
    }
}
