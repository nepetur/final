using UnityEditor;
using UnityEngine;

class Firable : MonoBehaviour{
    #region Editor
    [Header("Required Components")]
    [SerializeField] ParticleSystem fire;
    [SerializeField] ParticleSystem smoke;
    Collider hitbox;

    [Header("Fields")]
    [SerializeField] float fireSpreadRadius = 1;
    [SerializeField, Range(0, 1)] float damage;

    #if UNITY_EDITOR
    void OnDrawGizmosSelected(){
        if(!hitbox) hitbox = GetComponent<Collider>();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitbox.bounds.center, fireSpreadRadius);
    }
    [CustomEditor(typeof(Firable))] class FireEditor : Editor{
        Firable fire => target as Firable;

        string label{
            set => GUILayout.Label(value);
        }

        string header{
            set => GUILayout.Label(value, EditorStyles.boldLabel);
        }

        bool fireSwitched => GUILayout.Button("Switch Fire");

        public override void OnInspectorGUI(){
            header = $"On Fire: {fire.OnFire}\n";

            base.OnInspectorGUI();

            header = "\nActions";

            if(fireSwitched) fire.SetFire(!fire.OnFire);
        }
    }
    #endif
    #endregion
    
    public float Damage => damage;
    public bool OnFire => fire ? fire.isEmitting : false;
    const float fireDamageSpeed = .075f, fireSpreadInterval = 3;
    float lastFireSpread;

    void Awake(){
        hitbox = GetComponent<Collider>();
    }

    void Update(){
        if(OnFire){
            if(lastFireSpread < fireSpreadInterval){
                lastFireSpread += Time.deltaTime;

                if(lastFireSpread >= fireSpreadInterval) SpreadFire();
            }

            damage = Mathf.MoveTowards(damage, 1, Time.deltaTime * fireDamageSpeed);

            if(damage == 1) SetFire(false);
        }

        foreach(var render in GetComponentsInChildren<MeshRenderer>()){
            render.material.color = Color.Lerp(Color.white, Color.black, damage);
        }
    }

    public void SetFire(bool active){
        active = active ? damage < 1 : false;
        if(OnFire == active) return;

        if(active){
            if(fire) fire.Play();

            if(smoke) smoke.Play();
        }
        else{
            if(fire) fire.Stop();

            if(smoke) smoke.Stop();
        }
    }

    void SpreadFire(){
        var spherecast = Physics.SphereCastAll(hitbox.bounds.center, fireSpreadRadius, transform.forward, 0);

        foreach(var hit in spherecast){
            hit.transform.GetComponent<Firable>()?.SetFire(true);
        }
    }
}