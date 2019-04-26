using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Unit class, details the behavior of the players' units, implements IMovable, ISelectable, IDamageable, IBuyable, and ISpawnable
 * 
 * Allows the players' units to move according to A Star Pathfinding, attack other units, and die.
 */
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

    [HideInInspector] public Player owner = null;

    protected bool selected = false;
    private bool moving = false;
    public GameObject attackTarget = null;
    private float radius = .5f;
    private Animator anim = null;
    private SpriteRenderer sprite = null;

    private AStarGrid aStarGrid;
    private Node currentLocation;

    protected float currentHealth;

    /**
     * Start method, initializes Unit's starting values
     */
    protected virtual void Start()
    {
        currentHealth = maxHealth;
        anim = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        aStarGrid = FindObjectOfType<AStarGrid>();
    }

    /**
     * Update is called every frame, and moves the selection circle around this units if it's selected. 
     */
    protected virtual void Update() {
        if (selected && targetIndex > 0) {
            gameObject.UpdateCircleDraw(radius);
        }
    }

    /**
     * Starts the attack coroutine against the given gameobject.
     */
    public void SetAttackTarget(GameObject attackTarget) {
        this.attackTarget = attackTarget;
        if (attackTarget != null) {
            StopCoroutine("Attack");
            this.attackTarget = attackTarget;
            StartCoroutine("Attack");
        }
    }

    /**
     * Coroutine that trigger the attack animation and deal damage to the attacked IDamageable implementor.
     */
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

    /**
     * Calls the PathRequestManager to provide a path from the unit's position to the target destination
     */
    public void Move(Vector2 target) {
        PathRequestManager.RequestPath(GetComponent<Rigidbody2D>().position, target, OnPathFound);
    }

    /**
     * When a path is found by the PathRequestManager, this method will be called, giving this unit a path to the requested destination.
     * The method itself calls FollowPath to get to each step of the path.
     */
    private void OnPathFound(Vector2[] path, bool success) {
        if (this != null)
        {
            if (success)
            {
                StopCoroutine("FollowPath");
                this.path = path;
                StartCoroutine("FollowPath");
            }
        }
    }

    /**
     * Coroutine that moves the unit to the next waypoint of the path based on the A Star Pathfinding algorithm and the level grid.
     */
    IEnumerator FollowPath() {
        if (currentLocation == null) {
            currentLocation = aStarGrid.NodeFromWorldPoint(GetComponent<Rigidbody2D>().position);
        }

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

            Node newLocation = aStarGrid.NodeFromWorldPoint(GetComponent<Rigidbody2D>().position);
            if (!newLocation.IsEqual(currentLocation)) {
                FindObjectOfType<UnitController>().UpdateDamageable(gameObject, currentLocation);
                currentLocation = newLocation;
            }

            gameObject.UpdateCircleDraw(radius);
            yield return null;
        }
    }

    /**
     * Sets selected to true, drawing a circle when newly selected and destroying it if being deselected
     * 
     * Implementation Required by ISelectable
     */
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

    /**
     * Returns the selected boolean
     * 
     * Implementation Required by ISelectable
     */
    public bool IsSelected() {
        return selected;
    }

    /**
     * Returns boolean signifying if the units is moving or not
     * 
     * Implementation required by IMoveable
     */
    public bool IsMoving() {
        return moving;
    }

    /**
     * OnDamageTaken method that decrements health by the given float, then calls OnDeath if 
     * the resulting health value is equal to or less than zero.
     * 
     * Implementation required by IDamageable
     */
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

    /*
     * OnDeath deselects the unit, removes it from its owner's unit list, and destroys the game object.
     */
    public virtual void OnDeath()
    {
        selected = false;
        if (owner != null)
        {
            owner.units.Remove(gameObject);
        }
        FindObjectOfType<UnitController>().RemoveDamageable(gameObject);
        Destroy(gameObject);
    }

    /**
     * GetCost returns the monetary cost of buildign this unit.
     * 
     * Implementation required by IBuyable
     */
    public int GetCost()
    {
        return buildCost;
    }

    /**
     * SetOwningPlayerNum sets the owning player id of this unit.
     * 
     * Implementation required by ISpawnable
     */
    public void SetOwningPlayerNum(int owningPlayerNum) {
        this.owningPlayerNum = owningPlayerNum;
    }

    /**
     * GetOwningPlayerNum returns the owning player id of this unit.
     * 
     * Implementation required by ISpawnable
     */
    public int GetOwningPlayerNum() {
        return owningPlayerNum;
    }
}
