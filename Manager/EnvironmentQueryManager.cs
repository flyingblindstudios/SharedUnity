using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_HasPosition
{
    Vector2 GetPositionXZ();
   // Vector3 GetPosition();
}

public interface I_EnvironmentNode : I_HasPosition
{
    
}
namespace Shared.Manager
{

    //environment query system
    //maybe add a service factory which can instantiate services
    public class EnvironmentQueryManager : Singleton<EnvironmentQueryManager>
    {
        public class EnvironmentQueryResult
        {
            private List<I_EnvironmentNode> nodes = new List<I_EnvironmentNode>();
            private List<Vector2> nodePositionsXZ = new List<Vector2>(); //since Vector2 is a value type so the array should allocate space together?

            public EnvironmentQueryResult()
            {

            }

            public void Add(I_EnvironmentNode _poi)
            {
                nodes.Add(_poi);
                nodePositionsXZ.Add(_poi.GetPositionXZ());
            }

            public void Remove(I_EnvironmentNode _poi)
            {
                int index = nodes.IndexOf(_poi);
                if (index < 0)
                {
                    return;
                }
                nodes.RemoveAt(index);
                nodePositionsXZ.RemoveAt(index);
            }

            public I_EnvironmentNode AtIndex(int _index)
            {
                return nodes[_index];
            }

            public T AtIndex<T>(int _index) where T : I_EnvironmentNode
            {
                return (T)nodes[_index];
            }

            public int Count()
            {
                return nodes.Count;
            }

            public Vector2 GetPositionXZ(int _index)
            {
                return nodePositionsXZ[_index];
            }

            public Vector2[] GetPositionsCopy()
            {
                return nodePositionsXZ.ToArray();
            }

            public List<I_EnvironmentNode> GetNodes()
            {
                return nodes;
            }

        };


        Dictionary<System.Type, EnvironmentQueryResult> m_EnvironmentNodesDict = new Dictionary<System.Type, EnvironmentQueryResult>();



        public void RemoveNode(I_EnvironmentNode _poi)
        {
            if(m_EnvironmentNodesDict.ContainsKey(_poi.GetType()))
            {
                m_EnvironmentNodesDict[_poi.GetType()].Remove(_poi);
            }
        }

        public void ClearNodeType<T>() where T : I_Service
        {
            m_EnvironmentNodesDict.Remove(typeof(T));
        }

        public void RegisterNodeAsType(I_EnvironmentNode _poi, System.Type _type)
        {
            if (!m_EnvironmentNodesDict.ContainsKey(_type))
            {
                m_EnvironmentNodesDict[_type] = new EnvironmentQueryResult();
            }

            Debug.Log("[POIManager] Registerd POI " + _type.Name);

            m_EnvironmentNodesDict[_type].Add(_poi);
        }

        public void AddNode(I_EnvironmentNode _poi)
        {
            RegisterNodeAsType(_poi, _poi.GetType());
        }

        public Vector2[] QueryXZPositionsOfType<T>()
        {
            if (!m_EnvironmentNodesDict.ContainsKey(typeof(T)))
            {
                m_EnvironmentNodesDict[typeof(T)] = new EnvironmentQueryResult();
            }

            return ((EnvironmentQueryResult)m_EnvironmentNodesDict[typeof(T)]).GetPositionsCopy();
        }

        public EnvironmentQueryResult QueryNodesOfType<T>() where T : I_EnvironmentNode
        {
            if (!m_EnvironmentNodesDict.ContainsKey(typeof(T)))
            {
                m_EnvironmentNodesDict[typeof(T)] = new EnvironmentQueryResult();
            }

            return ((EnvironmentQueryResult) m_EnvironmentNodesDict [typeof(T)]);
        }

        public T QueryRandomNodeOfType<T>() where T : I_EnvironmentNode
        {
            EnvironmentQueryResult pois = QueryNodesOfType<T>();
            if (pois.Count() > 0)
            {
                I_EnvironmentNode randomPoi = pois.AtIndex(Random.Range(0, pois.Count()));
                return (T)randomPoi;
            }

            return default(T);
        }

        public T QueryClosestNodeOfType<T>(Vector2 _pos, out float _sqrDistance) where T : I_EnvironmentNode
        {
            EnvironmentQueryResult pois = QueryNodesOfType<T>();
            float closestDistance = float.MaxValue;
            int closestIndex = 0;
            for (int i = 0; i < pois.Count(); i++)
            {
                Vector2 posXZ = pois.GetPositionXZ(i);
                float distance = Vector2.SqrMagnitude(_pos - posXZ);
                closestIndex = i;
            }

            _sqrDistance = closestDistance;

            return (T)pois.AtIndex(closestIndex);
        }

        /*public I_Service RequestService(System.Type _type)
        {
            return m_Services[_type];
        }

        public T RequestService<T>() where T: I_Service 
        {
            return (T)m_Services[typeof(T)];
        }*/
    }
}