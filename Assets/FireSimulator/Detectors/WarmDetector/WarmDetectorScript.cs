using UnityEngine;

class WarmDetector : MonoBehaviour{
    const float temperatureSensetivity = .25f;
    [SerializeField] float detectRadius;
    [SerializeField, Range(0, 1) ]float temperatureDetected;

    Collider hitbox;
    void Awake(){
        hitbox = GetComponent<SphereCollider>();
    }

    void Update(){
        DetectFire();
    }

    void DetectFire(){
        var spherecast = Physics.SphereCastAll(hitbox.bounds.center, detectRadius, transform.forward, 0);

        foreach(var hit in spherecast){
            var fire = hit.transform.GetComponent<Firable>();

            if(fire ? fire.OnFire : false){
                temperatureDetected = Mathf.MoveTowards(temperatureDetected, 1, Time.deltaTime * temperatureSensetivity);

                if(temperatureDetected == 1){
                    hitbox.enabled = false;

                    FireManager.Instance?.ForceEnd();

                    GetComponent<Animator>()?.SetTrigger("trigger");
                }

                return;
            }
        }
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected(){
        if(!hitbox) hitbox = GetComponent<SphereCollider>();

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(hitbox.bounds.center, detectRadius);
    }
    #endif
}