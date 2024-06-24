using UnityEngine;

class ViewPoint : MonoBehaviour{
    [SerializeField] Transform target;

    void OnEnable(){
        transform.LookAt(target);        
    }

    void Update(){
        if(pressed){
            transform.LookAt(target);

            transform.RotateAround(target.position, Vector3.up * mouseDelta, Time.deltaTime * 125);
        }
    }

    bool pressed => Input.GetMouseButton(0);
    float mouseDelta{
        get{
            var x = Input.GetAxis("Mouse X");

            return x > 0 ? 1 : x < 0 ? -1 : 0;
        }
    }
}