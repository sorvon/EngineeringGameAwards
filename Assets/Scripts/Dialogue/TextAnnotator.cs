using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextAnnotator : MonoBehaviour
{
    public Canvas canvas;
    public Text text;
    public string strFragment;
    public Object original;
    public GameObject original1;
    public Quaternion quaternion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Object AddAnnotation(string strFragment, string annoText)
    {
        Object annotator=original1;
        original1.GetComponent<HoverUIShowName>().AnnotationText = annoText;
        //
        Debug.Log(strFragment.Length+"字体大小："+text.fontSize);
        original1.GetComponent<RectTransform>().sizeDelta = new Vector2(strFragment.Length* text.fontSize, text.fontSize);
        Vector3 stringPos = Vector3.zero;
        stringPos = GetPosAtText(canvas, text, strFragment);
        //Instantiate(original, stringPos, quaternion, canvas.transform);
        annotator = Instantiate(original, stringPos, quaternion, canvas.transform);
        Debug.Log("添加透明方块");
        return annotator;

    }
    public void AddAnnotation(string strFragment/*,string annoText*/)
    {
        //original1.GetComponent<HoverUIShowName>().AnnotationText = annoText;
        //original1.GetComponent<RectTransform>().rect.width
        Vector3 stringPos = Vector3.zero;
        stringPos=GetPosAtText(canvas, text, strFragment);
        Instantiate(original, stringPos, quaternion,canvas.transform);
        Debug.Log("添加透明方块");
        /*Rect rect = new Rect(stringPos.x, stringPos.x, 100, 100);
        GUI.DrawTexture(rect, texture);*/


    }
    public Vector3 GetPosAtText(Canvas canvas, Text text, string strFragment)
    {
        //<color>等标签渲染不占格数，在此删去，最后再恢复
        string text_temp=text.text;
        text.text = text.text.Replace("<color=red>", "");
        text.text = text.text.Replace("</color>", "");
        text.text = text.text.Replace("<b>", "");
        text.text = text.text.Replace("</b>", "");
        int strFragmentIndex = text.text.IndexOf(strFragment);//-1表示不包含strFragment
        Vector3 stringPos = Vector3.zero;
        if (strFragmentIndex > -1)
        {
            Vector3 firstPos = GetPosAtText(canvas, text, strFragmentIndex + 1);
            Vector3 lastPos = GetPosAtText(canvas, text, strFragmentIndex + strFragment.Length);
            stringPos = (firstPos + lastPos) * 0.5f;
        }
        else
        {
            stringPos = GetPosAtText(canvas, text, strFragmentIndex);
        }
        Debug.Log("stringPos:" + stringPos);
        text.text = text_temp;
        return stringPos;
    }
    public Vector3 GetPosAtText(Canvas canvas, Text text, int charIndex)
    {
        string textStr = text.text;
        Vector3 charPos = Vector3.zero;
        if (charIndex <= textStr.Length && charIndex > 0)
        {
            TextGenerator textGen = new TextGenerator(textStr.Length);
            Vector2 extents = text.gameObject.GetComponent<RectTransform>().rect.size;
            //Debug.Log("extents:"+extents);
            textGen.Populate(textStr, text.GetGenerationSettings(extents));

            int newLine = textStr.Substring(0, charIndex).Split('\n').Length - 1;
            int whiteSpace = textStr.Substring(0, charIndex).Split(' ').Length - 1;
            int indexOfTextQuad = (charIndex * 4) + (newLine * 4) - 4;
            if (indexOfTextQuad < textGen.vertexCount)
            {
                charPos = (textGen.verts[indexOfTextQuad].position +
                    textGen.verts[indexOfTextQuad + 1].position +
                    textGen.verts[indexOfTextQuad + 2].position +
                    textGen.verts[indexOfTextQuad + 3].position) / 4f;


            }
        }
        charPos /= canvas.scaleFactor;//适应不同分辨率的屏幕
        charPos = text.transform.TransformPoint(charPos);//转换为世界坐标
        Debug.Log("charPos:" + charPos);
        return charPos;
        
    }

}

