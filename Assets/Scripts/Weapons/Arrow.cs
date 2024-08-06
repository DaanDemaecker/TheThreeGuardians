using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : BasicProjectile
{
    private bool _released = false;

    private Rigidbody _rigidbody = null;

    // Start is called before the first frame update
    public override void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetRoot(Transform root)
    {
        transform.SetParent(root);
    }

    public void SetTransform(Transform root)
    {
        transform.rotation = root.transform.rotation;
        transform.position = root.transform.position;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (_released)
        {
            if (_rigidbody != null)
            {
                float lookatDistance = 5;
                transform.LookAt(transform.position + _rigidbody.velocity.normalized * lookatDistance);
            }
            WallDetection();

        }
    }

    public void Release(float speed, float animationPercentage, Vector3 direction)
    {
        if (_rigidbody != null)
        {
            _rigidbody.useGravity = true;
            _rigidbody.velocity = direction.normalized * speed * animationPercentage;
        }

        transform.parent = null;
        _released = true;
        _damage *= animationPercentage;
        
        Invoke(KILL_METHODNAME, _lifeTime);
    }
}
