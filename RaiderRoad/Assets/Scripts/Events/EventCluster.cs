using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCluster : MonoBehaviour {

        public  List<Event> events = new List<Event>();
        private int difficulty;
        public float initSize;
        public float complete;
        [SerializeField]
        private float weight;
        [SerializeField]
        private float delay;
        private int i = 0;          //needed to have this outside a function so that the coroutine doesn't mess up its value

        //constructor - need list of events
        public EventCluster(List<Event> sequence){
            events = sequence;
        }

        //on start
        void start()
        {
            initSize = events.Count;        //get number of events in cluster
            weight = 1 / initSize;             //determine weight of a single event for completeness
            foreach (Event element in events)
            {           //sum of event difficulties
                difficulty += element.difficultyRating;
            }

            //start spawning
            StartCoroutine(dispense());

        }

        IEnumerator dispense(){
            callEvent();
            yield return new WaitForSeconds(delay);     //call next event after delay
        }

        //increase completeness of cluster - called from vehicle on destroy
        public void updatePercent(){
            complete += weight;     
        }

        void callEvent(){
            events[i].spawn();
            i++;
        }

}
