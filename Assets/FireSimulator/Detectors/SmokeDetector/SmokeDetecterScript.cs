using UnityEngine;

class SmokeDetecter : MonoBehaviour{
    SphereCollider hitbox;
    void Awake(){
        hitbox = GetComponent<SphereCollider>();
    }

    void Start(){
        Smoke.OnDetecterCreated(hitbox);
    }

    void OnDestroy(){
        Smoke.OnDetecterDestroyed(hitbox);
    }

    public void Trigger(){
        hitbox.enabled = false;

        FireManager.Instance?.ForceEnd();

        GetComponent<Animator>()?.SetTrigger("trigger");
    }
}