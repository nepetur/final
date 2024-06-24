using UnityEngine;
using UnityEngine.Events;

class Smoke : MonoBehaviour{
    static public void OnDetecterCreated(Collider collider) => onDetecterCreated?.Invoke(collider);
    static public void OnDetecterDestroyed(Collider collider) => onDetecterDestroyed?.Invoke(collider);
    void AddDetecterToTrigger(Collider collider) => smoke.trigger.AddCollider(collider);
    void RemoveDetecterFromTrigger(Collider collider) => smoke.trigger.RemoveCollider(collider);
    static UnityEvent<Collider> onDetecterCreated = new UnityEvent<Collider>(), onDetecterDestroyed = new UnityEvent<Collider>();    

    ParticleSystem smoke;
    void Awake(){
        smoke = GetComponent<ParticleSystem>();

        onDetecterCreated.AddListener(AddDetecterToTrigger);

        onDetecterDestroyed.AddListener(RemoveDetecterFromTrigger);
    }
    void OnDestroy(){
        onDetecterCreated.RemoveListener(AddDetecterToTrigger);

        onDetecterDestroyed.RemoveListener(RemoveDetecterFromTrigger);
    }

    void OnParticleTrigger(){
        smoke.trigger.GetCollider(0).GetComponent<SmokeDetecter>()?.Trigger();
    }
}