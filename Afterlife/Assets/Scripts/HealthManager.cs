using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField, Tooltip("Health of the game object."), Range(0.1f, 999.0f)]
    private float health;


	/// <summary>
	/// Get object's health.
	/// </summary>
    public float GetHealth(){
        return health;
    }

    /// <summary>
	/// Set object's health.
	/// </summary>
    public void TakeDamage(float damage){
        this.health -= damage;

        // Kill object if health is <= 0
        if(this.health <= 0){
            // Kill object
        }
    }
    
}
