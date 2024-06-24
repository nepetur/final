using UnityEngine;
using System.Collections;
using UnityEngine.Events;

class FireManager : MonoBehaviour{
    static public FireManager Instance;
    void Awake(){
        Instance = this;
    }

    public bool simulationRunning => simulationStarted & !simulationEnded;

    bool simulationStarted;

    IEnumerator Start(){
        objectsOnFire = FindObjectsOfType<Firable>();

        yield return new WaitUntil( () => simulationStarted );
        startTime = Time.time;
        OnSimulationStart?.Invoke();

        randomFirable.SetFire(true);

        yield return new WaitUntil( () => simulationEnded );
        endTime = Time.time;
        OnSimulationEnd?.Invoke();
    }

    public void StartSimulation() => simulationStarted = true;

    float startTime, endTime;
    public float seconds => endTime - startTime;

    public float damagePrecent{
        get{
            float damage = 0;

            foreach(var firable in objectsOnFire){
                damage += firable.Damage;
            }

            return damage / objectsOnFire.Length * 100;
        }
    }

    Firable[] objectsOnFire;
    Firable randomFirable => objectsOnFire.Length == 0 ? null : objectsOnFire[ Random.Range(0, objectsOnFire.Length) ];
    bool simulationEnded{
        get{
            foreach(var firable in objectsOnFire){
                if(firable.OnFire) return false;
            }

            return true;
        }
    }
    public void ForceEnd(){
        foreach(var firable in objectsOnFire){
            firable.SetFire(false);
        }
    }

    public UnityEvent OnSimulationStart, OnSimulationEnd;
}