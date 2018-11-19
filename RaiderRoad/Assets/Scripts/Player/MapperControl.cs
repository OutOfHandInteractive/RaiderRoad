using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MapperControl : MonoBehaviour {

    private IList<Player> players;

	// Use this for initialization
	void Start () {
        players = ReInput.players.GetPlayers(false);
    }
	
	public void EnableControls()
    {
        foreach (Player player in players)
        {
            player.controllers.maps.SetMapsEnabled(true, "Default");
            player.controllers.maps.SetMapsEnabled(false, "UI");
        }
    }

    public void DiableControls()
    {
        foreach (Player player in players)
        {
            player.controllers.maps.SetMapsEnabled(false, "Default");
            player.controllers.maps.SetMapsEnabled(true, "UI");
        }
    }
}
