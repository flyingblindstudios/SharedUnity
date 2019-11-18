using UnityEngine;

namespace Shared.Manager
{
    public class UpdateManager : Singleton<UpdateManager> 
    {
        public delegate void UPDATE_DELEGATE( float _DeltaTime );

        /*UNITY_UPDATES*/
        //Use a lacy update of 0.0003333f
        float lacyUpdateTime = 0.03333333333f;
        float lacyDelta = 0.0f;

        public UPDATE_DELEGATE OnLacyUpdate;

        public UPDATE_DELEGATE OnUpdatePre;
        public UPDATE_DELEGATE OnUpdate;
        public UPDATE_DELEGATE OnUpdatePost;

        public UPDATE_DELEGATE OnLateUpdatePre;
        public UPDATE_DELEGATE OnLateUpdate;
        public UPDATE_DELEGATE OnLateUpdatePost;

        public UPDATE_DELEGATE OnFixedUpdatePre;
        public UPDATE_DELEGATE OnFixedUpdate;
        public UPDATE_DELEGATE OnFixedUpdatePost;


        public static float m_TimeScale = 1.0f;

        private void Update()
        {
            float delta = Time.deltaTime * m_TimeScale;
            OnUpdatePre?.Invoke(delta);
            OnUpdate?.Invoke(delta);
            OnUpdatePost?.Invoke(delta);

            lacyDelta += delta;

            if (lacyDelta >= lacyUpdateTime)
            {
                OnLacyUpdate(lacyDelta);
                lacyDelta = 0.0f;
            }
        }

        private void LateUpdate()
        {
            float delta = Time.deltaTime * m_TimeScale;
            OnLateUpdatePre?.Invoke(delta);
            OnLateUpdate?.Invoke(delta);
            OnLateUpdatePost?.Invoke(delta);
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime * m_TimeScale;
            OnFixedUpdatePre?.Invoke(delta);
            OnFixedUpdate?.Invoke(delta);
            OnFixedUpdatePost?.Invoke(delta);
        }


        public void SetTimeScale(float _timeScale)
        {
            Time.timeScale = _timeScale;
        }

    }
}
