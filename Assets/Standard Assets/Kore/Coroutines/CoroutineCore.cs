// Author: Dario Oliveri
// License Copyright 2016 (c) Dario Oliveri
// https://github.com/Darelbi/Kore.Utils

using Kore.Utils;
using System.Collections;
using UnityEngine;

namespace Kore.Coroutines
{
    public class CoroutineCore 
        : SceneScopedSingletonI< CoroutineCore, ICoroutine>, ICoroutine  // Singleton through inheritance
    {
        public override void Init()
        {
            base.Init();
            CreateCoroutineRunners();
            StartRunners();
        }

        public override void OnDestroyCalled()
        {
            base.OnDestroyCalled();
            coroutineCoreRunning = false;
            updateRunner = null;
            fixedRunner = null;
            lateRunner = null;
        }

        CoroutineEngine updateRunner;
        CoroutineEngine fixedRunner;
        CoroutineEngine lateRunner;
        WaitForFixedUpdate waitFixed;
        WaitForEndOfFrame waitLate;

        private void CreateCoroutineRunners()
        {
            updateRunner = new CoroutineEngine( Method.Update);
            fixedRunner = new CoroutineEngine( Method.FixedUpdate);
            lateRunner = new CoroutineEngine( Method.LateUpdate);
        }

        // Use Unity coroutines to keep a execution order consistent with old Unity Coroutines
        private void StartRunners()
        {
            waitFixed = new WaitForFixedUpdate();
            waitLate = new WaitForEndOfFrame();
            coroutineCoreRunning = true;
            StartCoroutine( UpdateCoroutine());
            StartCoroutine( FixedCoroutine());
            StartCoroutine( LateCoroutine());
        }

        bool coroutineCoreRunning;

        protected IEnumerator UpdateCoroutine()
        {
            while (coroutineCoreRunning)
            {
                yield return null;
                updateRunner.Tick();
            }
        }

        protected IEnumerator FixedCoroutine()
        {
            while (coroutineCoreRunning)
            {
                yield return waitFixed;
                fixedRunner.Tick();
            }
        }

        protected IEnumerator LateCoroutine()
        {
            while (coroutineCoreRunning)
            {
                yield return waitLate;
                lateRunner.Tick();
            }
        }

        public void Run( IEnumerator enumerator, Method method)
        {
            switch (method)
            {
                case Method.Update:
                    updateRunner.Run( enumerator);
                    break;

                case Method.FixedUpdate:
                    fixedRunner.Run( enumerator);
                    break;

                case Method.LateUpdate:
                    lateRunner.Run( enumerator);
                    break;
            }
        }
    }
}
