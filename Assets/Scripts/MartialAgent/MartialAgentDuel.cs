using System.IO;
using System.Collections;
using Unity.Barracuda;
using UnityEngine;
using SimpleFileBrowser;
using Unity.MLAgents.Policies;
using Unity.MLAgents;
using System;
using Unity.Barracuda.ONNX;
using Unity.VisualScripting.FullSerializer.Internal.Converters;
using UnityEditor;

public class MartialAgentDuel : MartialAgent
{


    public string modelFilePath;
    string defaultPath = "results/";
    public NNModel m_model;
    private Model m_RuntimeModel;


    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.parent.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

        StartCoroutine(SelectFilePath());
        
    }

    private IEnumerator SelectFilePath()
    {
        // open file path selection window
        // Open a file selection window to get the model file path

        // Set the filters and initial directory for the file dialog
        FileBrowser.SetFilters(true, new FileBrowser.Filter("ONNX files", ".onnx"));
        FileBrowser.SetDefaultFilter(".onnx");

        // Display the file dialog and wait for user input
        yield return FileBrowser.WaitForLoadDialog(0, false, defaultPath, "Open ONNX Model", "Select");
        modelFilePath = FileBrowser.Result[0];

        /*
        // Check if a file was selected
        if (modelFilePath.Length > 0)
        {
            // Load the model
            byte[] modelBytes = File.ReadAllBytes(modelFilePath);

            m_model = ScriptableObject.CreateInstance<NNModel>();
            m_model.modelData = ScriptableObject.CreateInstance<NNModelData>();
            m_model.modelData.Value = modelBytes;
            m_model.modelData.name = "Martial";

            modelFilePath = "results/Martial.onnx";

            m_model = (Unity.Barracuda.NNModel)AssetDatabase.LoadAssetAtPath(modelFilePath, typeof(Unity.Barracuda.NNModel));

            


            transform.GetChild(0).gameObject.SetActive(true);
            transform.parent.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);

            // base.SetModel("Martial", m_model, InferenceDevice.CPU);
        }
        */
    }

}
