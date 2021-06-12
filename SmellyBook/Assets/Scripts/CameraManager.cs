using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject target = null;
    private Vector2 targetPos;

    private float shakeIntensity, shakeDuration;
    private float shakeDelay = 0.05f;
    private float lastShake = 0;
    void Start()
    {
        targetPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        DoCameraPerFrameUpdate();
    }

    public void DoCameraPerFrameUpdate() {
        if (target != null)
            targetPos = target.transform.position;

        Vector2 shake = Vector2.zero;

        if ((shakeDuration -= Time.deltaTime) > 0) {
            lastShake += Time.deltaTime;
            if (lastShake >= shakeDelay) {
                lastShake = 0;
                float shakex, shakey;
                if (Random.Range(-1f, 1f) > 0)
                    shakex = Random.Range(0.5f, 1f);
                else
                    shakex = Random.Range(-1f, -0.5f);
                if (Random.Range(-1f, 1f) > 0)
                    shakey = Random.Range(0.5f, 1f);
                else
                    shakey = Random.Range(-1f, -0.5f);

                shake = shakeIntensity * new Vector2(shakex, shakey);
            }
        }

        Vector2 offset = shake + 0.25f * (targetPos - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
        gameObject.transform.position = new Vector3(offset.x, offset.y, gameObject.transform.position.z);
    }

    public void DoScreenShake(float intensity, float duration) {
        shakeIntensity = intensity;
        shakeDuration = duration;
    }
}
