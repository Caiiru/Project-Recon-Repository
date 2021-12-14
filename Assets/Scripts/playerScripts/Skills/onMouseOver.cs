using UnityEngine;

public class onMouseOver : MonoBehaviour
{
    private Animator _animator;

    public LayerMask SkillMask;

    public GameObject TrueCollider;

    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }
    
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity, SkillMask);
        
        if(hit2d && hit2d.collider)
        {            
            if (hit2d.collider.CompareTag("Skill") && hit2d.collider.name.Contains(gameObject.name))
            {
                _animator.SetBool("slashOver",true);
            }
            else
            {
                _animator.SetBool("slashOver",false);
            }
        }
        else
        {
            _animator.SetBool("slashOver",false);
        }
    }

    public void EnableTrueCollider()
    {
        if (_animator.GetBool("slashOver"))
        {
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            TrueCollider.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void EnableFakeCollider()
    {
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        TrueCollider.SetActive(false);
    }
}
