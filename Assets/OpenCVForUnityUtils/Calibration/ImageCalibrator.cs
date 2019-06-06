using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgcodecsModule;
using OpenCVForUnity.UnityUtils;
using UtilPack4Unity;

[RequireComponent(typeof(Calibrator))]
public class ImageCalibrator : MonoBehaviour
{
    [SerializeField]
    string directory;
    [SerializeField]
    string[] targetExtensions = new string[] {".jpg", ".png" };
    [SerializeField]
    Calibrator calibrator;
    [SerializeField]
    string fileName;
    [SerializeField]
    bool autoSave;
    [SerializeField]
    float interval;

    [SerializeField]
    bool draw;
    [SerializeField]
    Renderer renderer;
    Mat rgbMat;
    Texture2D texture;

    [ContextMenu("Calibrate")]
    void Calibrate()
    {
        StartCoroutine(CalibrateRoutine());
    }

    private void Reset()
    {
        calibrator = GetComponent<Calibrator>();
    }

    private void Init(Mat mat)
    {
        Clear();
        rgbMat = new Mat(mat.rows(), mat.cols(), CvType.CV_8UC3);
        calibrator.Init(mat.width(), mat.height());
        texture = new Texture2D(mat.cols(), mat.rows(), TextureFormat.RGB24, false);
    }

    private void Clear()
    {
        if (rgbMat != null)
        {
            rgbMat.Dispose();
        }
        if (texture != null)
        {
            DestroyImmediate(texture);
        }
    }

    IEnumerator CalibrateRoutine()
    {
        if (!Directory.Exists(directory)) yield break; ;

        var files = Directory.GetFiles(directory);
        files = files.Where(e => targetExtensions.Contains(Path.GetExtension(e).ToLower())).ToArray();
        if (files.Length < 1) yield break; ;

        calibrator.Setup();
        for (var i = 0; i < files.Length; i++)
        {
            using (Mat gray = Imgcodecs.imread(files[i], Imgcodecs.IMREAD_GRAYSCALE))
            {
                if (i == 0)
                {
                    Init(gray);
                }
                calibrator.Calibrate(gray);
                if (draw)
                {
                    Imgproc.cvtColor(gray, rgbMat, Imgproc.COLOR_GRAY2RGB);
                    calibrator.Draw(gray, rgbMat);
                    Utils.matToTexture2D(rgbMat, texture);
                    renderer.material.mainTexture = texture;
                }
               
                print("progress : " + (i + 1) + " / " + files.Length);
                yield return new WaitForSeconds(interval);
            }
        }
        if (autoSave)
        {
            calibrator.Save(IOHandler.IntoStreamingAssets(fileName));
        }

        calibrator.Clear();

        print("Complete Calibration");
        yield break;
    }
}
