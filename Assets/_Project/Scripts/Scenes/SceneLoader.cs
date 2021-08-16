/*
 * Copyright 2021 Doubling Technolgoies

 *
 *  Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using DoubTech.OpenPath.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DoubTech.OpenPath.Scenes
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Image progressBar;
        [SerializeField] private Fader fader;

        private void Start()
        {
            fader.Visible = false;
        }

        public void StartLoading()
        {
            StartCoroutine(LoadScene((int) SceneConfiguration.nextScene));
        }

        IEnumerator LoadScene(int scene)
        {
            yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            //When the load is still in progress, output the Text and progress bar
            while (!asyncOperation.isDone)
            {
                //Output the current progress
                if(progressBar) progressBar.fillAmount = asyncOperation.progress;

                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    yield return new WaitForSeconds(3);
                    fader.OnVisible.AddListener(() => asyncOperation.allowSceneActivation = true);
                    fader.Visible = true;
                    SceneConfiguration.currentScene = (SceneConfiguration.Scenes) scene;
                }

                yield return null;
            }
        }
    }
}
