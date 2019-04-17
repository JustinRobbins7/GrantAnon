using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IMoveable, ISelectable, IDamageable, IBuyable, ISpawnable
{
    int owningPlayerNum = 0;

    Vector2[] path;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] protected float maxHealth = 5f;
    [SerializeField] protected GameObject healthBar = null;
    [SerializeField] protected int buildCost;
    [SerializeField] float attackDamage = .1f;
    int targetIndex;

    protected bool selected = false;
    private bool moving = false;
    public GameObject attackTarget = null;
    private float radius = .5f;
    private Animator anim = null;
    private SpriteRenderer sprite = null;

    protected float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        anim = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    protected virtual void Update() {
        if (selected && targetIndex > 0) {
            gameObject.UpdateCircleDraw(radius);
        }
    }

    public void SetAttackTarget(GameObject attackTarget) {
        this.attackTarget = attackTarget;
        if (attackTarget != null) {
            StopCoroutine("Attack");
            this.attackTarget = attackTarget;
            StartCoroutine("Attack");
        }
    }

    IEnumerator Attack() {
        anim.SetTrigger("Attack");
        while (true) {
            if (attackTarget == null) {
                yield break;
            }

            attackTarget.GetComponent<IDamageable>().OnDamageTaken(attackDamage);
            yield return null;
        }
    }

    public void Move(Vector2 target) {
        PathRequestManager.RequestPath(GetComponent<Rigidbody2D>().position, target, OnPathFound);
    }

    private void OnPathFound(Vector2[] path, bool success) {
        if (success) {
            StopCoroutine("FollowPath");
            this.path = path;
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath() {
        if (path.Length == 0) {
            yield break;
        }

        moving = true;
        anim.SetBool("Moving", moving);
        Vector2 currentWaypoint = path[0];
        targetIndex = 0;

        if (anim != null)
        {
            anim.SetBool("Right", GetComponent<Rigidbody2D>().position.x < currentWaypoint.x);
            anim.SetBool("Up", GetComponent<Rigidbody2D>().position.y < currentWaypoint.y);
        }

        if (sprite != null)
        {
            sprite.flipX = GetComponent<Rigidbody2D>().position.x < currentWaypoint.x;
        }

        while (true) {
            if (GetComponent<Rigidbody2D>().position == currentWaypoint) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    targetIndex = 0;
                    moving = false;

                    if (anim != null)
                    {
                        anim.SetBool("Moving", moving);
                    }
                    
                    yield break;
                }
                currentWaypoint = path[targetIndex];

                if (anim != null)
                {
                    anim.SetBool("Right", GetComponent<Rigidbody2D>().position.x < currentWaypoint.x);
                    anim.SetBool("Up", GetComponent<Rigidbody2D>().position.y < currentWaypoint.y);
                }

                if (sprite != null)
                {
                    sprite.flipX = GetComponent<Rigidbody2D>().position.x < currentWaypoint.x;
                }
            }

            GetComponent<Rigidbody2D>().position = Vector2.MoveTowards(GetComponent<Rigidbody2D>().position, currentWaypoint, movementSpeed * Time.deltaTime);
            gameObject.UpdateCircleDraw(radius);
            yield return null;
        }
    }

    public void SetSelected(bool selected) {
        // If no change then don't update
        if (this.selected == selected) {
            return;
        }

        if (this.selected) {
            gameObject.DestroyCircleDraw();

            this.selected = false;

        } else {
            gameObject.CreateCircleDraw(radius);

            this.selected = true;
        }
    }

    public bool IsSelected() {
        return selected;
    }

    public bool IsMoving() {
        return moving;
    }

    public virtual void OnDamageTaken(float damageTaken)
    {
        currentHealth -= damageTaken;

        if (healthBar != null)
        {
            healthBar.transform.localScale = new Vector3(currentHealth / maxHealth, 1f, 1f);
        }

        if (currentHealth <= 0)
        {
            OnDeath();
        }
    }

    public virtual void OnDeath()
    {
        selected = false;
        Destroy(gameObject);
    }

    public int GetCost()
    {
        return buildCost;
    }

    public void SetOwningPlayerNum(int owningPlayerNum) {
        this.owningPlayerNum = owningPlayerNum;
    }

    public int GetOwningPlayerNum() {
        return owningPlayerNum;
    }
}
