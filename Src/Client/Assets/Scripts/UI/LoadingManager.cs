﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
public class LoadingManager : MonoBehaviour {
    public GameObject UITips;
    public GameObject UILoading;
    public GameObject UILogin;

    public Slider progessBar;
    //public Text progessText;
    public Text progressNumber;

	// Use this for initialization
	IEnumerator Start () {
        log4net.Config.XmlConfigurator.ConfigureAndWatch(new System.IO.FileInfo("log4net.xml"));
        UnityLogger.Init();
        Common.Log.Init("Unity");
        Common.Log.Info("LoadingManager start");

        UITips.SetActive(true);
        UILoading.SetActive(false);
        UILogin.SetActive(false);
        yield return new WaitForSeconds(2f);
        UILoading.SetActive(true);
        yield return new WaitForSeconds(1f);
        UITips.SetActive(false);

        //yield return DataManager.Instance.LoadData();

        UserService.Instance.Init();


        for(float i = 0; i < 100;)
        {
            i += Random.Range(0.1f, 1.5f);
            progessBar.value = i;
            progressNumber.text = string.Format("{0}%", Mathf.Round(i));
            yield return new WaitForEndOfFrame();
        }

        UILoading.SetActive(false);
        UILogin.SetActive(true);
        yield return null;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}