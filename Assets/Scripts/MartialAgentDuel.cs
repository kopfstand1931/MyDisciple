using System.IO;
using System.Collections;
using Unity.Barracuda;
using UnityEngine;
using SimpleFileBrowser;
using Unity.MLAgents.Policies;
using Unity.MLAgents;

public class MartialAgentDuel : MartialAgent
{


    public string modelFilePath;
    string defaultPath = "results/";
    public NNModel m_model;


    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.parent.GetChild(2).gameObject.SetActive(false);

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

        // Check if a file was selected
        if (modelFilePath.Length > 0)
        {
            // Load the model
            byte[] modelBytes = File.ReadAllBytes(modelFilePath);
            var modelData = ScriptableObject.CreateInstance<NNModelData>();
            modelData.Value = modelBytes;

            m_model = ScriptableObject.CreateInstance<NNModel>();
            m_model.modelData = modelData;

            transform.GetChild(0).gameObject.SetActive(true);
            transform.parent.GetChild(2).gameObject.SetActive(true);

            base.SetModel("Martial", m_model, InferenceDevice.CPU);
        }
    }

}
