using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

	public VehicleFactoryManager factory;

	private int difficultyRating;
	private int queueDifficultySum; 
	private Queue<Event> eventQueue;

	public float TimeBetweenEvents;
	public float TimeBetweenDifficultyAdjustment;

	// Equation coefficients
	public float expectedGameLengthModifier;
	public float sinFrequencyModifier;
	public float sinAmplitudeModifier;
	public float difficultySlopeModifier;
	public float baseDifficultyRating;

	// Variation coefficients
	public float randomModifierMin;
	public float randomModifierMax;

	// event generation constants
	public int minEventDifficulty;
	public int maxEventDifficulty;

	// Use this for initialization
	void Start () {
		StartCoroutine(events());
		StartCoroutine(difficultyManager());
	}
	
	// Update is called once per frame
	void Update () {
	}

	// control when events are added to the queue
	IEnumerator events() {
		while (true) {
			yield return new WaitUntil(delegate { return queueDifficultySum < difficultyRating; });

			addEventsToQueue(difficultyRating - queueDifficultySum);
		}
	}

	// control game difficulty rating
	IEnumerator difficultyManager() {
		while (true) {
			difficultyRating = calculateDifficultyRating();
			Debug.Log(difficultyRating);

			yield return new WaitForSecondsRealtime(TimeBetweenDifficultyAdjustment);
		}
	}

	// Use difficulty equation to calculate event difficulty rating based on current time
	private int calculateDifficultyRating() {
		float timeMinutes = GameManager.GameManagerInstance.getGameTime()/60;
		double calculatedDifficulty;

		System.Random rand = new System.Random();
		double randomModifier = (rand.NextDouble() * (randomModifierMax - randomModifierMin)) + randomModifierMin;

		// Equation to calculate difficulty rating. Has base linear slope modified by a sin function to give peaks and valleys
		// to difficulty
		// diff = diffSlope*x + sin(frequencyModifier*x) + baseDifficulty
		calculatedDifficulty = ((difficultySlopeModifier*timeMinutes) + (sinAmplitudeModifier*Math.Sin(sinFrequencyModifier*timeMinutes)) + baseDifficultyRating);

		// add modifier to calculated difficulty +/- some percent
		calculatedDifficulty += calculatedDifficulty * randomModifier;

		// return rating rounded to nearest whole number
		return Convert.ToInt32(calculatedDifficulty);
	}

	// while there is space left in the queue as per the difficulty of the events, add them to the end
	private void addEventsToQueue(int spaceToAdd) {
		System.Random rand = new System.Random();
		int eventDifficulty;

		while (spaceToAdd > 0) {
			if (spaceToAdd >= 5) {
				eventDifficulty = rand.Next(minEventDifficulty, maxEventDifficulty);
				spaceToAdd -= eventDifficulty;
				queueDifficultySum += eventDifficulty;
				// add event to queue
			}
			else {
				eventDifficulty = rand.Next(minEventDifficulty, spaceToAdd);
				spaceToAdd -= eventDifficulty;
				queueDifficultySum += eventDifficulty;
				// add event to queue
			}
		} 
	}

	// generate an event based on the difficulty generated
	private void generateEvent(int eventDifficulty) {
		if (eventDifficulty == 1) {
			// create light vehicle
		}
		else if (eventDifficulty == 2) {
			// create obstacle in road
		}
		else if (eventDifficulty == 3) {
			// create medium vehicle
		}
		else if (eventDifficulty == 4) {
			// create fork in road
		}
		else if (eventDifficulty == 5) {
			// create heavy vehicle
		}
	}
}
