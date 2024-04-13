using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphShader
{
    public enum PieChartValueType { TotalToOne, Floating, Percentage }
    public class PieChart : MonoBehaviour
    {
        public PieChartValueType valueType;
        [Range(0f, 1f)]
        public float start = 0;
        [Range(0f, 1f)]
        public float end = 1;
        [Range(0f, 1f)]
        public float size = 1;
        [Range(0f, 1f)]
        public float hole = 0;
        [Range (0, 30)]
        public int currentStep = 0;
        [Range(0f, 1f)]
        public float stepProgress = 0;
        public Material material;
        [TextAreaAttribute(5, 10)]
        public string values;
        public float progress
        {
            get { return (currentStep + stepProgress) / Y; }
            set
            {
                float v = value * (Y - 1);
                currentStep = (int)v;
                stepProgress = v % 1;
                UpdateValues();
            }
        }
        Texture2D data;
        string lastValues;
        int X, Y;
        int lastStep;
        float lastProgress, lastStart, lastEnd, lastSize, lastHole;
        // Start is called before the first frame update
        void Start()
        {
            data = (Texture2D)material.GetTexture("_Data");
            lastValues = values;
            lastStep = currentStep;
            lastProgress = stepProgress;
            lastStart = end;
            lastStart = start;
           lastHole = hole;
            lastSize = size;
            UpdateValues();
        }

        // Update is called once per frame
        void Update()
        {
            bool changed = values != lastValues;
            if (changed)
            {
                UpdateValues();
                lastValues = values;
            }
            else if (currentStep != lastStep || stepProgress != lastProgress || start != lastStart || end != lastEnd || hole != lastHole || size != lastSize)
            {
                if (currentStep > Y - 2) currentStep = Y - 2;
                lastStep = currentStep;
                lastProgress = stepProgress;
                lastStart = end;
                lastStart = start;
                lastSize=size;
                lastHole=hole;
                UpdateGeometry();
            }
        }
        void UpdateValues()
        {
            string[] lines = values.Split('\n');
            float[,] v = new float[16, 32];
            int x = 0;
            X = Y = 0;
            float f;
            for (int j = 0; j < lines.Length; j++)
                if (Y < 32)
                {
                    x = 0;
                    string[] split = lines[j].Split(new char[] { ' ', ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < split.Length; i++)
                        if (i < 16) if (float.TryParse(split[i], out f))
                            {
                                v[x++, Y] = valueType == PieChartValueType.Percentage ? f / 100 : f;
                            }
                    if (x > 0)
                    {
                        X = X < x ? x : X;
                        Y++;
                    }
                }
            float[,] a = new float[X, Y];
            float total;
            if (valueType == PieChartValueType.TotalToOne)
                for (int j = 0; j < Y; j++)
                {
                    total = 0;
                    for (int i = 0; i < X; i++)
                    {
                        a[i, j] = total;
                        total += v[i, j];
                    }
                    for (int i = 0; i < X; i++)
                        a[i, j] /= total;
                }
            else
                for (int j = 0; j < Y; j++)
                {
                    total = 0;
                    for (int i = 0; i < X; i++)
                    {
                        a[i, j] = total;
                        total = Mathf.Min(total + v[i, j], 1);
                    }
                }
            for (int j = 0; j < Y; j++)
                for (int i = 0; i < X; i++)
                {
                    float r = Mathf.Floor(a[i, j] * 255) / 255;
                    float g = (a[i, j] * 255) % 1;
                    data.SetPixel(i, j, new Color(r, g, 0));
                    Debug.Log(i + ", " + j + " : " + r + ", " + g);
                }
            Debug.Log("item count = " + X + " x " + Y);
            for (int i = 0; i < X; i++)
                Debug.Log("item " + i + ": " + a[i, 0] + " > " + a[i, 1]);

            data.Apply();
            material.SetFloat("_X", X);
            material.SetFloat("_Y", Y);
            lastStep = currentStep = Mathf.Min(currentStep, Y - 2);
            UpdateGeometry();
        }
        void UpdateGeometry()
        {
            material.SetFloat("_Hole", hole);
            material.SetFloat("_Size", size);
            material.SetFloat("_Start", start);
            material.SetFloat("_End", end);
            material.SetFloat("_CurrentStep", currentStep);
            material.SetFloat("_StepProgress", stepProgress);
        }
    }
}