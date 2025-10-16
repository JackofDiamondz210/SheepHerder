using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

namespace GDD3400.Project01
{
    public class Dog : MonoBehaviour
    {
        
        private bool _isActive = true;
        public bool IsActive 
        {
            get => _isActive;
            set => _isActive = value;
        }

        // Required Variables (Do not edit!)
        private float _maxSpeed = 5f;
        private float _sightRadius = 7.5f;

        // Layers - Set In Project Settings
        public LayerMask _targetsLayer;
        public LayerMask _obstaclesLayer;

        // Tags - Set In Project Settings
        private string friendTag = "Friend";
        private string threatTag = "Thread";
        private string safeZoneTag = "SafeZone";

        //making the dog follow a trained path
        private int currentWaypointIndex = 0;
        [SerializeField] private List<Transform> waypoints = new List<Transform>(); //for all point the bot will travel

        //when dog sees sheep it will use this reduce speed variable
        private float _currentSpeed;



        public void Awake()
        {
            // Find the layers in the project settings
            _targetsLayer = LayerMask.GetMask("Targets");
            _obstaclesLayer = LayerMask.GetMask("Obstacles");

        }

        private void Update()
        {
            if (!_isActive) return;
            
            Perception();
            DecisionMaking();

            GameObject[] sheep = GameObject.FindGameObjectsWithTag("Friend");

            bool nearSheep = false;

            foreach (GameObject S in sheep)
            {
                //ignoring the friend tag with gameobjects attached to dog
                if (IsMyOwnFriendTag(S))
                    continue;

                //if sheep are close enough to sight distance change bool to true
                float distance = Vector3.Distance(transform.position, S.transform.position);
                if(distance < _sightRadius)
                {
                    nearSheep = true;
                }

            }

            //changing speed based on distance bool
            if (nearSheep)
            {
                _currentSpeed = _maxSpeed / 3f;
            }
            else
            {
                _currentSpeed = _maxSpeed;
            }

        }

        //creating a bool to check and see if a child gameobject of dog has friend tag to ignore
        private bool IsMyOwnFriendTag(GameObject Dog)
        {
            // Check if the friend tag object is part of the dog
            return Dog.transform.IsChildOf(transform);
        }

        //not needed making dog follow a path
        private void Perception()
        {
            
        }

        private void DecisionMaking()
        {
            //dog will follow trained path and will change speed if sheep are close using _currentSpeed
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, _currentSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Count)
                {
                    currentWaypointIndex = 0; // Loop back to start
                }
            }
        }

        /// <summary>
        /// Make sure to use FixedUpdate for movement with physics based Rigidbody
        /// You can optionally use FixedDeltaTime for movement calculations, but it is not required since fixedupdate is called at a fixed rate
        /// </summary>
        private void FixedUpdate()
        {
            if (!_isActive) return;
            
        }
    }
}
