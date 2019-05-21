using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class StealEnemy : EnemyAI {


    //enemy, speed
    private GameObject cObject;
    public bool hasStolen = false;
    private GameObject[] drops;
    private GameObject drop;
    private GameObject stealIcon;
    private bool maxDisplay = false;
    private bool minDisplay = false;

    public void StartSteal(GameObject enemy, GameObject _stealIcon) {
        cObject = enemy;
        drops = GameObject.FindGameObjectsWithTag("Drops");		// oh god this is horrendous
        drop = Closest(drops);
        stealIcon = _stealIcon;
    }

    public void Steal() {
        //Set wall gameobject
        //Set movement speed of enemy
        float movement = speed * Time.deltaTime;

        if (cObject.GetComponent<StatefulEnemyAI>().getDamaged()) {
            decreaseIconSize();

            cObject.GetComponent<StatefulEnemyAI>().ExitStealState(StatefulEnemyAI.State.Fight);
			cObject.GetComponent<StatefulEnemyAI>().EnterFight();
        }

        //If there are no more drops, go to Escape state, else keep going for drops
        if (hasStolen && cObject.transform.GetComponentInChildren<ItemDrop>()) {
            displayIcon();
            movement /= 2;
			cObject.GetComponent<StatefulEnemyAI>().ExitStealState(StatefulEnemyAI.State.Escape);
			cObject.GetComponent<StatefulEnemyAI>().EnterEscape();
        }
        else {
            //Find wall and go to it
            if(drop != null) {
                cObject.transform.LookAt(drop.transform);
                cObject.transform.position = Vector3.MoveTowards(cObject.transform.position, drop.transform.position, movement);
            }
            else {
                movement /= 2;
				cObject.GetComponent<StatefulEnemyAI>().ExitStealState(StatefulEnemyAI.State.Destroy);
				cObject.GetComponent<StatefulEnemyAI>().EnterDestroy();
            }
        }
    }

    private void displayIcon()
    {
        RectTransform icon = stealIcon.GetComponent<RectTransform>();
        float maxHeight = 20;
        float maxWidth = 15;
        float increaseValue = 1f;
        if (icon.rect.height < maxHeight && icon.rect.width < maxWidth && !maxDisplay)
        {
            stealIcon.SetActive(true);
            stealIcon.GetComponent<Image>().color = Color.red;
            icon.sizeDelta = new Vector2(icon.rect.height + increaseValue, icon.rect.width + increaseValue);
            stealIcon.GetComponent<RectTransform>().sizeDelta = icon.sizeDelta;
        }
        else
        {
            maxDisplay = true;
        }
    }

    private void decreaseIconSize()
    {
        RectTransform icon = stealIcon.GetComponent<RectTransform>();
        if (icon.rect.height > 0 && icon.rect.width > 0 && !minDisplay)
        {
            stealIcon.SetActive(false);
        }
        else
        {
            minDisplay = true;
        }
    }



}
