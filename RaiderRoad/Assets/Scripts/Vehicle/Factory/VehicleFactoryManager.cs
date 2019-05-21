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

	/// <summary>
	/// Call to create a vehicle to be spawned into the game
	/// </summary>
	/// <param name="type">The weight class of vehicle to spawn - light, medium, or heavy</param>
	/// <param name="mod">The desired difficulty modifier of the vehicle to spawn</param>
	/// <param name="position">The location at which the vehicle is to be spawned</param>
	/// <param name="wChance">The chance of spawning a weapon on the vehicle</param>
	/// <returns></returns>
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

	/// <summary>
	/// Gets the number of batteries currently placed on the RV
	/// </summary>
	/// <returns>The number of batteries currently active on the RV</returns>
    private int getBatteries(){
        return rvRef.getRemainingBatteries();
    }
}
