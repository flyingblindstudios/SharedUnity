using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.Manager;

namespace Shared.AI
{ 
    public class GoapWorldManager : Singleton<GoapWorldManager>
    {
        public delegate void WorldStateChange(HashSet<string> _state);
        public WorldStateChange OnWorldStateChanged;
    #if UNITY_EDITOR
        [SerializeField] List<string> m_DebugWorldState = new List<string> ();
    #endif
        HashSet<string> worldState = new HashSet<string>();

        HashSet<string> lastWorldState = new HashSet<string>();

        Dictionary<int, List<GoapObject>> goapObjects = new Dictionary<int, List<GoapObject>>();


        // Update is called once per frame
        void Update()
        {
            lastWorldState.Clear();
            lastWorldState.UnionWith(worldState);

            //Clear world state
            worldState.Clear();

            //Update world state
            List<I_EnvironmentNode> pois = EnvironmentQueryManager.GetInstance().QueryNodesOfType<Field>().GetNodes();
            for (int i = 0; i < pois.Count; i++)
            {
                Field f = (Field)pois[i];
                if (f.m_FieldState == Field.STATE.PLANTING)
                {
                    worldState.Add("Planting");
                }
                else if (f.m_FieldState == Field.STATE.HARVESTING)
                {
                    worldState.Add("Harvesting");
                }
                else if (f.m_FieldState == Field.STATE.GROWING)
                {
                    //worldState.Add("Growing");
                }
            }

            /*int resourcePickups = EnvironmentQueryManager.GetInstance().QueryNodesOfType<ResourcePickup>().Count();
            if (resourcePickups > 0)
            {
                worldState.Add("Pickup");
            }*/
            //i need to rewrite this
            foreach (List<GoapObject> objects in goapObjects.Values )
            {
                foreach (GoapObject goapObject in objects)
                {
                    foreach (GoapEffect effect in goapObject.GetWorldEffects())
                    {
                        worldState.Add(GoapEffect.IdNames[effect.value]);
                    }
                }
            }

            if (!lastWorldState.SetEquals(worldState))
            {
                OnWorldStateChanged?.Invoke(worldState);

    #if UNITY_EDITOR
                UpdateDebugList();
    #endif

            }

        }

        public void AddObject(GoapObject _object)
        {
            if(!goapObjects.ContainsKey(_object.GetObjectLayer().value))
            {
                goapObjects[_object.GetObjectLayer().value] = new List<GoapObject>();
            }

            goapObjects[_object.GetObjectLayer().value].Add(_object);
        }

        public void RemoveObject(GoapObject _object)
        {
            if (!goapObjects.ContainsKey(_object.GetObjectLayer().value))
            {
                return;
            }

            goapObjects[_object.GetObjectLayer().value].Remove(_object);
        }

        public List<GoapObject> QueryObjectsWithID(GoapID _ID)
        {
            if (!goapObjects.ContainsKey(_ID.value))
            {
                goapObjects[_ID.value] = new List<GoapObject>();
            }

            return goapObjects[_ID.value];
        }

        public GoapObject QueryClosestObjectWithID(GoapID _queryObjectsID, int _tag,Vector2 _agentPosition,  out float _sqrDistanceToTarget)
        {
            _sqrDistanceToTarget = float.MaxValue;
            if (!goapObjects.ContainsKey(_queryObjectsID.value))
            {
                return null;
            }

            List<GoapObject> objects = goapObjects[_queryObjectsID.value];

            
            GoapObject targetObject = null;

            for (int i = 0; i < objects.Count; i++)
            {

                if (!objects[i].CanBeUsed() || objects[i].GetTagHash() != _tag)
                {
                    continue;
                }

                float sqrDist = Vector2.SqrMagnitude(objects[i].GetPositionXZ()- _agentPosition);
                if(sqrDist < _sqrDistanceToTarget)
                {
                    _sqrDistanceToTarget = sqrDist;
                    targetObject = objects[i];
                }
            }

            return targetObject;
        }



#if UNITY_EDITOR
        void UpdateDebugList()
        {
            m_DebugWorldState.Clear();
            m_DebugWorldState = new List<string>(worldState);
        }
    #endif

        public HashSet<string> GetWorldState()
        {
            return worldState;
        }
    }

}