using Assets.Scripts.Manager;
using Assets.Scripts.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class LoadingManager:MonoBehaviour
    {
        public GameObject UITips;
        public GameObject UILoading;
        public GameObject UILogin;

        public Slider progressBar;
        public Text progressText;

        IEnumerator Start()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
            TestManager.Instance.Init();
            MapService.Instance.Init();
            UITips.SetActive(true);
            UILoading.SetActive(false);
            UILogin.SetActive(false);
            yield return new WaitForSeconds(2f);
            UILoading.SetActive(true);
            yield return new WaitForSeconds(1f);
            UITips.SetActive(false);

            yield return DataManager.Instance.LoadData();
            
            // Fake Loading Simulate
            for (float i = 50; i < 100;)
            {
                i += Random.Range(0.1f, 1.5f);
                progressBar.value = i;
                progressText.text = ((int)(progressBar.value / progressBar.maxValue * 100)) + "%";
                yield return new WaitForEndOfFrame();
            }

            UILoading.SetActive(false);
            UILogin.SetActive(true);
            yield return null;
        }
    }
}
