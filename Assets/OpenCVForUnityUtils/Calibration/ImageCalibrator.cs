using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgcodecsModule;
using OpenCVForUnity.UnityUtils;

public class ImageCalibrator : MonoBehaviour
{
    [SerializeField]
    string directory;
    [SerializeField]
    string[] targetExtensions;
    [SerializeField]
    Calibrator calibrator;


    [ContextMenu("Calibrate")]
    void Calibrate()
    {
        if (!Directory.Exists(directory)) return;

        var files = Directory.GetFiles(directory);
        files = files.Where(e => targetExtensions.Contains(Path.GetExtension(e).ToLower())).ToArray();
        if (files.Length < 1) return;

        calibrator.Setup();
        var hasInit = false;
        foreach (var file in files)
        {
            using (Mat gray = Imgcodecs.imread(file, Imgcodecs.IMREAD_GRAYSCALE))
            {
                if (!hasInit)
                {
                    calibrator.Init(gray);
                    hasInit = true;
                }
                calibrator.Calibrate(gray);
            }
        }
        calibrator.Save();
        calibrator.Clear();
    }
}
