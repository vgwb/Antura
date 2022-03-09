using UnityEngine;

namespace Antura.LivingLetters
{
    // TODO obsolete: remove this?
    public class ShadowController : MonoBehaviour
    {
        /*
        Vector3 shadowDimNormal = new Vector3(6, 6, 6);
        Vector3 shadowDimHang = new Vector3(8, 8, 8);

        Hangable hangableController;

        // Use this for initialization
        void Awake()
        {
            hangableController = GetComponentInParent<Hangable>();
            if (hangableController)
                hangableController.ObserveEveryValueChanged(x => x.OnDrag).Subscribe(onDrag => {
                    bool isOnDrag = onDrag;
                    if (isOnDrag)
                        transform.localScale = shadowDimHang;
                    else
                        transform.localScale = shadowDimNormal;
                });
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
        }
        */
    }
}
